using System.Text.Json;

using SerchKey = (string name, int lineNum);

namespace RushEightProject
{
    struct SearchResult
    {
        public string Route;
        public int TotalTime;

        public SearchResult(string route, int totalTime)
        {
            Route = route;
            TotalTime = totalTime;
        }
    }

    internal class SubwayManager
    {
        public const int TransTime = 180;   // 초

        public Dictionary<string, Station> Stations = new Dictionary<string, Station>();

        public void InitSubwayInfo()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "SubwayInfo.json");
            var json = File.ReadAllText(path);
            
            Stations = JsonSerializer.Deserialize<List<Station>>(json)?.ToDictionary(d => d.Name, d => d)
                            ?? throw new InvalidOperationException("SubwayInfo.json 로드 실패");
        }

        public void SearchSubway()
        {
            var start = string.Empty;
            while (true)
            {
                Console.Write("출발 역 : ");
                start = Console.ReadLine();

                if (SearchStation(start))
                    break;

                Console.WriteLine("존재하지 않는 역입니다, 다시 입력해 주세요");
            }

            var end = string.Empty;
            while (true)
            {
                Console.Write("도착 역 : ");
                end = Console.ReadLine();

                if (SearchStation(end))
                    break;

                Console.WriteLine("존재하지 않는 역입니다, 다시 입력해 주세요");
            }

            if (start.Equals(end))
            {
                Console.WriteLine("출발/도착역이 동일합니다. 다시 입력해 주세요");
                SearchSubway();
                return;
            }

            if (SearchSubwayRoute(start, end, out var result))
            {
                Console.WriteLine($"[탐색 결과]: {start} -> {end}");
                Console.WriteLine($"이동 경로: {result.Route}");
                Console.WriteLine($"총 소요시간: {result.TotalTime.TimeString()}");
            }
            else
            {
                Console.WriteLine("경로를 찾을 수 없습니다.");
            }
        }

        private bool SearchSubwayRoute(string start, string end, out SearchResult result)
        {
            result = new SearchResult();

            var routes = new Dictionary<SerchKey, SearchResult>();
            var queue = new PriorityQueue<SerchKey, int>();

            var startStation = Stations[start];
            var lines = startStation.NextStations.Select(d => d.LineNum).Distinct().ToList();

            // 다른 호선은 다른역으로 취급
            foreach (var line in lines)
            {
                queue.Enqueue((startStation.Name, line), 0);
            }

            while (queue.Count > 0)
            {
                var q = queue.TryDequeue(out var value, out var priority);

                var stationName = value.Item1;
                var lineNum = value.Item2;

                // 목적지에 도착하면 종료
                if (stationName == end)
                {
                    var route = routes[(stationName, lineNum)];
                    result.Route = route.Route;
                    result.TotalTime = route.TotalTime;
                    return true;
                }

                // 다음 역 탐색
                var station = Stations[stationName];
                foreach (var nextStation in station.NextStations)
                {
                    var nextTime = priority + nextStation.Time;
                    var transfer = lineNum != nextStation.LineNum;

                    if (transfer)
                    {
                        nextTime += TransTime;
                    }

                    var key = (nextStation.Name, nextStation.LineNum);
                    if (false == routes.TryGetValue(key, out var old) || nextTime < old.TotalTime)
                    {
                        var route = string.Empty;
                        if (routes.ContainsKey((stationName, lineNum)))
                        {
                            route = $"{routes[(stationName, lineNum)].Route} -> {nextStation.Name}";
                        }
                        else
                        {
                            route = $"{stationName} -> {nextStation.Name}";
                        }

                        if (transfer)
                        {
                            route = $"{route}(환승)";
                        }

                        routes[key] = new SearchResult(route, nextTime);
                        queue.Enqueue((nextStation.Name, nextStation.LineNum), nextTime);
                    }
                }
            }

            return false;
        }

        private bool SearchStation(string stationName)
        {
            if (stationName == null) return false;

            return Stations.ContainsKey(stationName);
        }
    }
}
