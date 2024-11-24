using Xunit;

namespace ObsidianEventStream.Tests;

[Collection("emptyCanvas")]
public class ParsingMultipleJsonEvents
{
    [Fact]
    public void Multiple_JSON_events_produces_many_cards()
    {
        TestSystem.UsingEmptyCanvas().AnalyzeEventsStream("""
                                                          [
                                                            { "name": "First" },
                                                            { "name": "Second" },
                                                            { "name": "Third" }
                                                          ]
                                                          """)
            .WithTitle("name")
            .Gives()
            .FirstCard("# First")
            .SecondCard("# Second")
            .ThirdCard("# Third");
    }
}
