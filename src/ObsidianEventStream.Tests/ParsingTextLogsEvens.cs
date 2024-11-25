using Xunit;

namespace ObsidianEventStream.Tests;

[Collection("emptyCanvas")]
public class ParsingTextLogsEvens
{
    [Fact]
    public void JSON_object_can_be_found_in_log_line_with_delimiter()
    {
        TestSystem.UsingEmptyCanvas()
            .AnalyzeEventsStream("""
                                 2024-11-24 21:00:01|POST|{"name": "John", "age": 20}|https://google.com
                                 2024-11-24 21:00:02|POST|{"name": "Mark", "age": 20}|https://google.com
                                 """)
            .TakeEvent(x => x.Split('|')[2])
            .WithTitle("name")
            .Gives()
            .FirstCard("# John")
            .SecondCard("# Mark");
    }
    
    [Fact]
    public void JSON_object_can_be_found_in_log_line_with_brackets()
    {
        TestSystem.UsingEmptyCanvas()
            .AnalyzeEventsStream("""
                                 [2024-11-24 21:00:01][POST][{"name": "John", "age": 20}][https://google.com]
                                 [2024-11-24 21:00:02][POST][{"name": "Mark", "age": 20}][https://google.com]
                                 """)
            .TakeEvent(x => x.Split("][")[2])
            .WithTitle("name")
            .Gives()
            .FirstCard("# John")
            .SecondCard("# Mark");
    }
    
    [Fact]
    public void JSON_object_can_be_found_in_log_line_in_different_places()
    {
        TestSystem.UsingEmptyCanvas()
            .AnalyzeEventsStream("""
                                 [2024-11-24 21:00:01][TYPE1][{"name": "John", "age": 20}][https://google.com]
                                 [2024-11-24 21:00:02][TYPE2][not json][{"name": "Mark", "age": 20}][https://google.com]
                                 """)
            .TakeEvent(x => x.Contains("[not json]") ? x.Split("][")[3] : x.Split("][")[2])
            .WithTitle("name")
            .Gives()
            .FirstCard("# John")
            .SecondCard("# Mark");
    }
    
    [Fact]
    public void Different_JSON_event_formats_can_be_found_in_different_places()
    {
        TestSystem.UsingEmptyCanvas()
            .AnalyzeEventsStream("""
                                 [2024-11-24 21:00:01][TYPE1][POST][{"name": "John", "age": 20}][https://google.com]
                                 [2024-11-24 21:00:02][TYPE2][{"friend": "Mark", "age": 20}][https://google.com]
                                 """)
            .ForType("[TYPE1]")
            .TakeEvent(x => x.Split("][")[3])
            .WithTitle("name")
            .ForType("[TYPE2]")
            .TakeEvent(x => x.Split("][")[2])
            .WithTitle("friend")
            .Gives()
            .FirstCard("# John")
            .SecondCard("# Mark");
    }
}