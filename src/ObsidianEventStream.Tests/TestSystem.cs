﻿using FluentAssertions;

namespace ObsidianEventStream.Tests;

public class TestSystem
{
    private string _filePath;
    private string _title = "";
    private string _extract = "";
    private string _promote = "";
    private Canvas _canvas;
    private string _currentCardIdForLinking;

    public static TestSystem UsingEmptyCanvas(string path = "empty_file_for_tests.canvas")
    {
        File.WriteAllText(path, "{}");
        var testSystem = new TestSystem {_filePath = path}; 
        return testSystem;
    }

    public TestSystem AnalyzeEventsStream(string eventsStream)
    {
        File.WriteAllText("events_file_for_tests.txt", eventsStream);
        return this;
    }

    public TestSystem WithTitle(string titleProperty)
    {
        _title = titleProperty;
        return this;
    }
 
    public TestSystem WithExtraDetails(string extractProperty)
    {
        _extract = extractProperty;
        return this;
    }
    public TestSystem WithPromotion(string promoteProperty)
    {
        _promote = promoteProperty;
        return this;
    }

    public TestSystem Gives()
    {
        _canvas= Canvas.Initialize(_filePath);
        _canvas.AnalyzeEvents("events_file_for_tests.txt", _title, _extract, _promote);
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
        var lines = expectedLines.Split(Environment.NewLine);
        var cardLines = _canvas.EnumerateCards().First().text.Split(Environment.NewLine);
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
}