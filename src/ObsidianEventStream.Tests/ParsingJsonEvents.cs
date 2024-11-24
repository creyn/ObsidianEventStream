using FluentAssertions;
using Xunit;

namespace ObsidianEventStream.Tests;

public class ParsingJsonEvents
{
    [Fact]
    public void Simple_JSON_property_from_single_event_displayed_in_single_card_as_heading()
    {
        var jsonWithSimpleProperty = """
                               [
                                {
                                 "name": "Test event name",
                                 "description": "Description"
                                }
                               ]
                               """;
        var testSystem = TestSystem.InitializeWithEmptyCanvas().SetEventsStream(jsonWithSimpleProperty).Build();
        
        testSystem.AnalyzeEventsStream(cardTitle: "name");
        
        testSystem.GetFinalCanvas().EnumerateCards().Should().HaveCount(1);
        testSystem.GetFinalCanvas().EnumerateCards().Single().text.Split(Environment.NewLine).First().Should().Be("# Test event name");
    }
    
    [Fact]
    public void Simple_JSON_property_from_single_event_extracted_from_json_and_displayed_in_single_card_below_heading()
    {
        var jsonWithSimpleProperty = """
                                     [
                                      {
                                       "name": "Name",
                                       "description": "This is description"
                                      }
                                     ]
                                     """;
        var testSystem = TestSystem.InitializeWithEmptyCanvas().SetEventsStream(jsonWithSimpleProperty).Build();
        
        testSystem.AnalyzeEventsStream(cardTitle: "name", "description");
        
        testSystem.GetFinalCanvas().EnumerateCards().Should().HaveCount(1);
        testSystem.GetFinalCanvas().EnumerateCards().Single().text.Split(Environment.NewLine).Skip(1).First().Should().Be("description = This is description");
    }
    
    [Fact]
    public void Multiple_properties_can_be_extracted()
    {
        var jsonWithSimpleProperty = """
                                     [
                                      {
                                       "name": "Name",
                                       "description": "This is description",
                                       "color": "red"
                                      }
                                     ]
                                     """;
        var testSystem = TestSystem.InitializeWithEmptyCanvas().SetEventsStream(jsonWithSimpleProperty).Build();
        
        testSystem.AnalyzeEventsStream(cardTitle: "name", extract: "description;color");
        
        testSystem.GetFinalCanvas().EnumerateCards().Should().HaveCount(1);
        testSystem.GetFinalCanvas().EnumerateCards().Single().text.Split(Environment.NewLine).Skip(1).First().Should().Be("description = This is description");
        testSystem.GetFinalCanvas().EnumerateCards().Single().text.Split(Environment.NewLine).Skip(2).First().Should().Be("color = red");
    }
    
    [Fact]
    public void Nested_JSON_property_can_be_title()
    {
     var jsonWithNestedProperty = """
                                  [
                                   {
                                    "name": {
                                      "first": "Adam",
                                      "second": "James"
                                    }
                                   }
                                  ]
                                  """;
     var testSystem = TestSystem.InitializeWithEmptyCanvas().SetEventsStream(jsonWithNestedProperty).Build();
        
     testSystem.AnalyzeEventsStream(cardTitle: "name.first");
        
     testSystem.GetFinalCanvas().EnumerateCards().Should().HaveCount(1);
     testSystem.GetFinalCanvas().EnumerateCards().Single().text.Split(Environment.NewLine).First().Should().Be("# Adam");
    }
    
    public void Nested_JSON_property_can_be_extracted()
    {
     var jsonWithNestedProperty = """
                                  [
                                   {
                                    "name": {
                                      "first": "Adam",
                                      "second": "James"
                                    }
                                   }
                                  ]
                                  """;
     var testSystem = TestSystem.InitializeWithEmptyCanvas().SetEventsStream(jsonWithNestedProperty).Build();
        
     testSystem.AnalyzeEventsStream(cardTitle: "name.first", extract: "name.second");
        
     testSystem.GetFinalCanvas().EnumerateCards().Should().HaveCount(1);
     testSystem.GetFinalCanvas().EnumerateCards().Single().text.Split(Environment.NewLine).Skip(1).First().Should().Be("second = James");
    }
    
    [Fact]
    public void Array_JSON_properties_can_be_extracted()
    {
     var jsonWithNestedProperty = """
                                  [
                                   {
                                    "name": "Adam",
                                    "friends": [
                                     { "name": "First" }, { "name": "Second" }, { "name": "Third" } 
                                    ]
                                   }
                                  ]
                                  """;
     var testSystem = TestSystem.InitializeWithEmptyCanvas().SetEventsStream(jsonWithNestedProperty).Build();
        
     testSystem.AnalyzeEventsStream(cardTitle: "name", extract: "friends.name");
        
     testSystem.GetFinalCanvas().EnumerateCards().Should().HaveCount(1);
     testSystem.GetFinalCanvas().EnumerateCards().Single().text.Split(Environment.NewLine).Skip(1).First().Should().Be("friends.name = First;Second;Third");
    }
    
    [Fact]
    public void Simple_JSON_property_can_be_promoted_to_compainion_card()
    {
     var jsonWithNestedProperty = """
                                  [
                                   {
                                    "name": "Adam",
                                    "phone": "+12345"
                                   }
                                  ]
                                  """;
     var testSystem = TestSystem.InitializeWithEmptyCanvas().SetEventsStream(jsonWithNestedProperty).Build();
        
     testSystem.AnalyzeEventsStream(cardTitle: "name", promote: "phone");
        
     testSystem.GetFinalCanvas().EnumerateCards().Should().HaveCount(2);
     testSystem.GetFinalCanvas().EnumerateCards().Skip(1).Single().text.Split(Environment.NewLine).First().Should().Be("phone = +12345");
    }

    [Fact]
    public void Test()
    {
     Testing.UsingEmptyCanvas().AnalyzeEventsStream("""
                                                    [
                                                     {
                                                      "name": "Adam"
                                                     }
                                                    ]
                                                    """)
      .WithTitle("name")
      .Gives()
      .SingleCardTitle("# Adam");
    }
    
}

public class Testing
{
 private string _filePath;
 // private string _eventsStream;
 private string _title;
 private Canvas _canvas;

 public static Testing UsingEmptyCanvas(string path = "empty_file_for_tests.canvas")
 {
  File.WriteAllText(path, "{}");
  var testSystem = new Testing {_filePath = path}; 
  return testSystem;
 }

 public Testing AnalyzeEventsStream(string eventsStream)
 {
  File.WriteAllText("events_file_for_tests.txt", eventsStream);
  // _eventsStream = eventsStream;
  return this;
 }

 public Testing WithTitle(string titleProperty)
 {
  _title = titleProperty;
  return this;
 }

 public Testing Gives()
 {
  _canvas= Canvas.Initialize(_filePath);
  _canvas.AnalyzeEvents("events_file_for_tests.txt", title: _title, extract: "", promote: "");
  return this;
 }

 public void SingleCardTitle(string expectedTitle)
 {
  _canvas.EnumerateCards().Should().HaveCount(1);
  _canvas.EnumerateCards().Single().text.Split(Environment.NewLine).First().Should().Be(expectedTitle);
 }
}
