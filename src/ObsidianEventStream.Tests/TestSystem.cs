namespace ObsidianEventStream.Tests;

public class TestSystem
{
    private string? _filePath;
    private Canvas? _canvas;
    public static TestSystem InitializeWithEmptyCanvas(string path = "empty_file_for_tests.canvas")
    {
        File.WriteAllText(path, "{}");
        var testSystem = new TestSystem {_filePath = path};
        return testSystem;
    }
    
    public static TestSystem InitializeWithExampleCanvas(string path)
    {
        var exampleContent =
            "{\n\t\"nodes\":[\n\t\t{\"id\":\"15552bd146acd62c\",\"type\":\"group\",\"x\":43,\"y\":-207,\"width\":587,\"height\":100,\"color\":\"2\",\"label\":\"This is a group\"},\n\t\t{\"id\":\"058452a8fa08898d\",\"type\":\"link\",\"url\":\"https://www.pawelkowalik.com/obsidian/\",\"x\":70,\"y\":140,\"width\":610,\"height\":400},\n\t\t{\"id\":\"ecb5392f059d1156\",\"type\":\"file\",\"file\":\"docs/_attachments/Pasted image 20240505123925.png\",\"x\":260,\"y\":-39,\"width\":400,\"height\":139},\n\t\t{\"id\":\"0671dbd17ef83214\",\"type\":\"text\",\"text\":\"# group 1\",\"x\":63,\"y\":-187,\"width\":250,\"height\":60},\n\t\t{\"id\":\"454c6e9d475df2ec\",\"type\":\"text\",\"text\":\"## group 2\",\"x\":360,\"y\":-187,\"width\":250,\"height\":60,\"color\":\"3\"},\n\t\t{\"id\":\"92d802fea7ceb339\",\"type\":\"text\",\"text\":\"This is card\",\"x\":-305,\"y\":-168,\"width\":250,\"height\":60},\n\t\t{\"id\":\"a8a6b51e70f9d17d\",\"type\":\"text\",\"text\":\"This is another card\",\"x\":-55,\"y\":0,\"width\":250,\"height\":60},\n\t\t{\"id\":\"9238c230c63c283e\",\"type\":\"file\",\"file\":\"README.md\",\"x\":-380,\"y\":140,\"width\":400,\"height\":400}\n\t],\n\t\"edges\":[\n\t\t{\"id\":\"4695a42756603a3d\",\"fromNode\":\"92d802fea7ceb339\",\"fromSide\":\"bottom\",\"toNode\":\"a8a6b51e70f9d17d\",\"toSide\":\"left\",\"label\":\"This is link\"}\n\t]\n}";
        File.WriteAllText(path, exampleContent);
        var testSystem = new TestSystem {_filePath = path};
        return testSystem;
    }

    public TestSystem SetEventsStream(string eventsStream)
    {
        File.WriteAllText("events_file_for_tests.txt", eventsStream);
        return this;
    }

    public TestSystem Build()
    {
        _canvas = Canvas.Initialize(_filePath);
        return this;
    }

    public void AnalyzeEventsStream(string cardTitle, string extract = "", string promote= "")
    {
        _canvas.AnalyzeEvents("events_file_for_tests.txt", cardTitle, extract, promote);
    }

    public Canvas? GetFinalCanvas()
    {
        return _canvas;
    }

    
}