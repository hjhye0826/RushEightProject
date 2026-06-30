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
    }
}
