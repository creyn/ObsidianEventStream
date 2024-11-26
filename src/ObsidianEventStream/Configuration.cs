namespace ObsidianEventStream;

public class TypeConfiguration
{
    public const string DEFAULT = "_DEFAULT_TYPE_";
    public string TypeFilter { get; set; } = "";
    public string Title { get; set; } = "";
    public string Extract { get; set; } = "";
    public string Promote { get; set; } = "";
    public Func<string, string>? FindJson { get; set; }
}

public class Configuration
{
    private readonly Dictionary<string, TypeConfiguration> _typeConfigurations = new();

    public Configuration(string title, string extract, string promote, Func<string,string>? findJson = null, bool isLogMode = false, string filterLogsBy = "")
    {
        _isLogMode = isLogMode;
        _filterLogsBy = filterLogsBy;
        _typeConfigurations.Add(TypeConfiguration.DEFAULT, new TypeConfiguration
        {
            TypeFilter = TypeConfiguration.DEFAULT,
            Title = title,
            Extract = extract,
            Promote = promote,
            FindJson = findJson
        });
    }

    public Configuration(List<TypeConfiguration> typeConfigurations, bool isLogMode = true, string filterLogsBy = "")
    {
        _isLogMode = isLogMode;
        _filterLogsBy = filterLogsBy;
        typeConfigurations.ForEach(x => _typeConfigurations.Add(x.TypeFilter, x));
    }

    public string title => _typeConfigurations[TypeConfiguration.DEFAULT].Title;
    public string extract => _typeConfigurations[TypeConfiguration.DEFAULT].Extract;
    public string promote => _typeConfigurations[TypeConfiguration.DEFAULT].Promote;
    public Func<string, string>? findJson => _typeConfigurations[TypeConfiguration.DEFAULT].FindJson;

    public bool IsLogMode()
    {
        return _isLogMode;
    }
    private bool _isLogMode;
    private readonly string _filterLogsBy;

    public string FilterBy() => _filterLogsBy;

    public TypeConfiguration GetConfigurationForEventTypeInThisLine(string logLine)
    {
        foreach (var filter in _typeConfigurations.Values)
        {
            if (logLine.Contains(filter.TypeFilter)) return filter;
        }
        return _typeConfigurations.Single().Value;
    }
}
