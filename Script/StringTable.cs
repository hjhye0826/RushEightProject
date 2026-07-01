using System.Text.Json;

namespace RushEightProject
{
    public static class StringTable
    {
        private class StringTableData
        {
            public string Key { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
        }
        private static Dictionary<string, string> _stringTable = new Dictionary<string, string>();

        public static void InitStringTable()
        {
            var fileName = "StringTable.json";
            var path = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
            var json = File.ReadAllText(path);

            _stringTable = JsonSerializer.Deserialize<List<StringTableData>>(json)?.ToDictionary(d => d.Key, d => d.Value)
                           ?? throw new InvalidOperationException(string.Format(StringTable.GetText("LoadFaile"), fileName));

        }

        public static string GetText(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            var str = _stringTable.GetValueOrDefault(key);
            str = str?.Replace("\\n", "\n");

            return str ?? string.Empty;
        }
    }
}
