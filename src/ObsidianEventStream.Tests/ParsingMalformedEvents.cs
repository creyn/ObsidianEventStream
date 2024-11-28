using Xunit;

namespace ObsidianEventStream.Tests;

[Collection("emptyCanvas")]
public class ParsingMalformedEvents
{
    [Fact]
    public void JSON_event_can_be_found_within_malformed_data()
    {
        TestSystem.UsingEmptyCanvas().AnalyzeEventsLogs("""
                                                        malformed start [Logger][malformed["log:on",{"name":"Adam", "phone": 123}]malformed] malformed end
                                                        """)
            .TakeEvent(x => $"[{EventFiltering.ExtractTextBetweenBrackets(x, 3)}]")
            .WithTitle("[0]")
            .WithExtraDetails("[1].name")
            .WithPromotion("[1].phone")
            .Gives()
            .FirstCard("""
                       # log:on
                       name = Adam
                       """)
            .LinkedWith("phone = 123");
    }
}
