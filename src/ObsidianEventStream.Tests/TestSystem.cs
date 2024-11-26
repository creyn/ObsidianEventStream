using FluentAssertions;

namespace ObsidianEventStream.Tests;

public class TestSystem
{
    private string _filePath;
    private string _title = "";
    private string _extract = "";
    private string _promote = "";
    private Canvas _canvas;
    private string _currentCardIdForLinking;
    private Func<string, string> _findJsonInDelimitedTextLine;
    private string _currentLogType;
    private bool _isLogMode = false;

    private TypeConfiguration _currentTypeConfiguration = new TypeConfiguration() {TypeFilter = TypeConfiguration.DEFAULT};
    private List<TypeConfiguration> _currentTypeConfigurations = new();
    private string _filterLogsBy;

    public static TestSystem UsingEmptyCanvas(string path = "empty_file_for_tests.canvas")
    {
        File.WriteAllText(path, "{}");
        var testSystem = new TestSystem {_filePath = path}; 
        return testSystem;
    }
    
    public TestSystem AnalyzeEvents(string eventsStream)
    {
        File.WriteAllText("events_file_for_tests.txt", eventsStream);
        _isLogMode = false;
        return this;
    }

    public TestSystem AnalyzeEventsLogs(string eventsStream)
    {
        File.WriteAllText("events_file_for_tests.txt", eventsStream);
        _isLogMode = true;
        return this;
    }

    public TestSystem WithTitle(string titleProperty)
    {
        _currentTypeConfiguration.Title = titleProperty;
        return this;
    }
 
    public TestSystem WithExtraDetails(string extractProperty)
    {
        _currentTypeConfiguration.Extract = extractProperty;
        return this;
    }
    public TestSystem WithPromotion(string promoteProperty)
    {
        _currentTypeConfiguration.Promote = promoteProperty;
        return this;
    }

    public TestSystem Gives()
    {
        _canvas= Canvas.Initialize(_filePath);
        _currentTypeConfigurations.Add(_currentTypeConfiguration);
        var config = new Configuration(_currentTypeConfigurations, _isLogMode, _filterLogsBy);
        _canvas.AnalyzeEvents("events_file_for_tests.txt", config);
        return this;
    }

    public TestSystem SingleCard(string expectedLines)
    {
        _canvas.EnumerateCards().Should().HaveCount(1);
        var lines = expectedLines.Split(Environment.NewLine);
        var cardLines = _canvas.EnumerateCards().Single().text.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].Should().BeEquivalentTo(cardLines[i]);
        }

        return this;
    }
 
    public TestSystem FirstCard(string expectedLines)
    {
        return AssertCardLines(expectedLines);
    }

    private TestSystem AssertCardLines(string expectedLines, int cardNumber = 1)
    {
        var lines = expectedLines.Split(Environment.NewLine);
        var cardLines = _canvas.EnumerateCards().Skip(cardNumber - 1).First().text.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].Should().BeEquivalentTo(cardLines[i]);
        }

        _currentCardIdForLinking = _canvas.EnumerateCards().First().id;

        return this;
    }

    public TestSystem LinkedWith(string linkedCardExpectedLines)
    {
        var linkedNodeId = _canvas.EnumerateLinks().Single(x => x.fromNode == _currentCardIdForLinking).toNode;
        var linkedNode = _canvas.EnumerateCards().Single(x => x.id == linkedNodeId);
  
        var lines = linkedCardExpectedLines.Split(Environment.NewLine);
        var cardLines = linkedNode.text.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].Should().BeEquivalentTo(cardLines[i]);
        }
  
        return this;
    }

    public TestSystem SecondCard(string expectedLines)
    {
        return AssertCardLines(expectedLines, cardNumber: 2);
    }

    public TestSystem ThirdCard(string expectedLines)
    {
        return AssertCardLines(expectedLines, cardNumber: 3);
    }

    public TestSystem TakeEvent(Func<string, string> findJsonInDelimitedTextLine)
    {
        _currentTypeConfiguration.FindJson = findJsonInDelimitedTextLine;
        return this;
    }

    public TestSystem ForType(string typeFilter)
    {
        _currentTypeConfigurations.Add(_currentTypeConfiguration);
        _currentTypeConfiguration = new TypeConfiguration
        {
            TypeFilter = typeFilter
        };
        return this;
    }

    public TestSystem Filter(string loggername)
    {
        _filterLogsBy = loggername;
        return this;
    }
}