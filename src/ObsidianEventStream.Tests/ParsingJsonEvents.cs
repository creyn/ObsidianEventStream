using Xunit;

namespace ObsidianEventStream.Tests;

public class ParsingJsonEvents
{
    [Fact]
    public void Card_title_is_heading_with_JSON_property()
    {
     TestSystem.UsingEmptyCanvas().AnalyzeEventsStream("""
                                                    [
                                                     {
                                                      "name": "Test event name",
                                                      "description": "Description"
                                                     }
                                                    ]
                                                    """)
      .WithTitle("name")
      .Gives()
      .SingleCard("# Test event name");
    }
    
    [Fact]
    public void Card_extra_details_are_below_heading_and_has_property_name()
    {
     TestSystem.UsingEmptyCanvas().AnalyzeEventsStream("""
                                                    [
                                                     {
                                                      "name": "Name",
                                                      "description": "This is description"
                                                     }
                                                    ]
                                                    """)
      .WithTitle("name")
      .WithExtraDetails("description")
      .Gives()
      .SingleCard("""
                  # Name
                  description = This is description
                  """);
    }
    
    [Fact]
    public void Multiple_properties_can_be_extracted()
    {
     TestSystem.UsingEmptyCanvas().AnalyzeEventsStream("""
                                                    [
                                                     {
                                                      "name": "Name",
                                                      "description": "This is description",
                                                      "color": "red"
                                                     }
                                                    ]
                                                    """)
      .WithTitle("name")
      .WithExtraDetails("description;color")
      .Gives()
      .SingleCard("""
                  # Name
                  description = This is description
                  color = red
                  """);
    }
    
    [Fact]
    public void Title_can_be_taken_from_nested_object()
    {
     TestSystem.UsingEmptyCanvas().AnalyzeEventsStream("""
                                                    [
                                                     {
                                                      "name": {
                                                        "first": "Adam",
                                                        "second": "James"
                                                      }
                                                     }
                                                    ]
                                                    """)
      .WithTitle("name.first")
      .Gives()
      .SingleCard("# Adam");
    }
    
    public void Extra_details_can_be_taken_from_nested_object()
    {
     TestSystem.UsingEmptyCanvas().AnalyzeEventsStream("""
                                                    [
                                                     {
                                                      "name": {
                                                        "first": "Adam",
                                                        "second": "James"
                                                      }
                                                     }
                                                    ]
                                                    """)
      .WithTitle("name.first")
      .WithExtraDetails("name.second")
      .Gives()
      .SingleCard("""
                  # Adam
                  second = James
                  """);
    }
    
    [Fact]
    public void Extra_details_can_be_taken_from_array()
    {
     TestSystem.UsingEmptyCanvas().AnalyzeEventsStream("""
                                                    [
                                                     {
                                                      "name": "Adam",
                                                      "friends": [
                                                       { "name": "First" }, { "name": "Second" }, { "name": "Third" } 
                                                      ]
                                                     }
                                                    ]
                                                    """)
      .WithTitle("name")
      .WithExtraDetails("friends.name")
      .Gives()
      .SingleCard("""
                  # Adam
                  friends.name = First;Second;Third
                  """);
    }
    
    [Fact]
    public void Promotion_gives_second_card_with_given_JSON_property()
    {
     TestSystem.UsingEmptyCanvas().AnalyzeEventsStream("""
                                                    [
                                                     {
                                                      "name": "Adam",
                                                      "phone": "+12345"
                                                     }
                                                    ]
                                                    """)
      .WithTitle("name")
      .WithPromotion("phone")
      .Gives()
      .FirstCard("# Adam")
      .LinkedWith("phone = +12345");
    }
}