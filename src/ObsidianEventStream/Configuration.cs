namespace ObsidianEventStream;

public class Configuration
{
    class TypeConfiguration
    {
        public string TypeFilter { get; set; }
        public string Title { get; set; }
        public string Extract { get; set; }
        public string Promote { get; set; }
        public Func<string, string>? FindJson { get; set; }
    }

    private const string DEFAULT_TYPE = "_DEFAULT_TYPE";
    private readonly Dictionary<string, TypeConfiguration> _typeConfigurations = new();

    public Configuration(string title, string extract, string promote, Func<string,string>? findJson = null)
    {
        _typeConfigurations.Add(DEFAULT_TYPE, new TypeConfiguration
        {
            TypeFilter = DEFAULT_TYPE,
            Title = title,
            Extract = extract,
            Promote = promote,
            FindJson = findJson
        });
    }

    public string title => _typeConfigurations[DEFAULT_TYPE].Title;
    public string extract => _typeConfigurations[DEFAULT_TYPE].Extract;
    public string promote => _typeConfigurations[DEFAULT_TYPE].Promote;
    public Func<string, string>? findJson => _typeConfigurations[DEFAULT_TYPE].FindJson;
}
