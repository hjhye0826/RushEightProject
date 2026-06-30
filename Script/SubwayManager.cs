using System.Text.Json;

namespace RushEightProject
{
    internal class SubwayManager
    {
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
                Console.WriteLine("출발/도착역기 동일합니다. 다시 입력해 주세요");
                SearchSubway();
                return;
            }

            // todo : 여기에 루트 찾기 로직 추가
        }

        private bool SearchStation(string stationName)
        {
            if (stationName == null) return false;

            return Stations.ContainsKey(stationName);
        }
    }
}
