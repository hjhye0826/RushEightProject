using System.Text.Json;

namespace RushEightProject
{
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

            SearchSubwayRoute(start, end);
        }

        private void SearchSubwayRoute(string start, string end)
        {
            var accTimes = new Dictionary<(string, int), int>();
            var queue = new PriorityQueue<(string, int), int>();

            var startStation = Stations[start];
            var lines = startStation.Nodes.Select(d => d.LineNum).Distinct().ToList();

            foreach (var line in lines)
            {
                queue.Enqueue((startStation.Name, line), 0);
            }

            while (queue.Count > 0)
            {
                var q = queue.TryDequeue(out var value, out var priority);

                var stationName = value.Item1;
                var lineNum = value.Item2;

                if (stationName == end)
                {
                    Console.WriteLine($"총 소요 시간 : {priority.TimeString()}");
                    return;
                }

                var station = Stations[stationName];

                foreach (var nextStation in station.Nodes)
                {
                    var nextTime = priority + nextStation.Time;
                    var transfer = lineNum != nextStation.LineNum;

                    if (transfer)
                    {
                        nextTime += TransTime;
                    }

                    var key = (nextStation.Name, nextStation.LineNum);
                    if (false == accTimes.TryGetValue(key, out var old) || nextTime < old)
                    {
                        accTimes[key] = nextTime;
                        queue.Enqueue((nextStation.Name, nextStation.LineNum), nextTime);
                    }
                }
            }
        }

        private bool SearchStation(string stationName)
        {
            if (stationName == null) return false;

            return Stations.ContainsKey(stationName);
        }
    }
}
