using System.Text.Json;

using SerchKey = (string name, int lineNum);

namespace RushEightProject
{
    internal class SubwayManager
    {
        public const int TransTime = 180;   // 초

        public Dictionary<string, StationData> Stations = new Dictionary<string, StationData>();

        public void InitSubwayInfo()
        {
            var fileName = "SubwayInfo.json";
            var path = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
            var json = File.ReadAllText(path);
            
            Stations = JsonSerializer.Deserialize<List<StationData>>(json)?.ToDictionary(d => d.Name, d => d)
                            ?? throw new InvalidOperationException(string.Format(StringTable.GetText("LoadFaile"), fileName));
        }

        public void SearchSubway()
        {
            var start = string.Empty;
            while (true)
            {
                Console.Write($"{StringTable.GetText("StartStation")} : ");
                start = Console.ReadLine() ?? string.Empty;

                if (SearchStation(start))
                    break;

                Console.WriteLine(StringTable.GetText("StationNotFound"));
            }

            var end = string.Empty;
            while (true)
            {
                Console.Write($"{StringTable.GetText("EndStation")} : ");
                end = Console.ReadLine() ?? string.Empty;

                if (SearchStation(end))
                    break;

                Console.WriteLine(StringTable.GetText("StationNotFound"));
            }

            if (start.Equals(end))
            {
                Console.WriteLine(StringTable.GetText("SameStation"));
                SearchSubway();
                return;
            }

            if (SearchSubwayRoute(start, end, out var result))
            {
                Console.WriteLine(string.Format(StringTable.GetText("SearchResultLabel"), start, end));
                Console.WriteLine(string.Format(StringTable.GetText("RouteLabel"), result.Route));
                Console.WriteLine(string.Format(StringTable.GetText("TotalTimeLabel"), result.TotalTime.TimeString()));
            }
            else
            {
                Console.WriteLine(StringTable.GetText("Route Not Found"));
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

                var stationName = value.name;
                var lineNum = value.lineNum;

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

                    // 호선이 다를 경우 환승 시간 누적
                    if (transfer)
                    {
                        nextTime += TransTime;
                    }

                    var key = (nextStation.Name, nextStation.LineNum);
                    if (false == routes.TryGetValue(key, out var old) || nextTime < old.TotalTime)
                    {
                        var routeString = string.Empty;
                        if (routes.ContainsKey((stationName, lineNum)))
                        {
                            var prevRoute = routes[(stationName, lineNum)].Route;
                            var routeKey = transfer ? "TransferRoute" : "Route";

                            routeString = string.Format(StringTable.GetText(routeKey), prevRoute, nextStation.Name);
                        }
                        else
                        {
                            routeString = string.Format(StringTable.GetText("Route"), stationName, nextStation.Name);
                        }

                        routes[key] = new SearchResult(routeString, nextTime);
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
