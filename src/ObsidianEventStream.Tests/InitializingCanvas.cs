using Xunit;

namespace ObsidianEventStream.Tests;

public class InitializingCanvas
{
    [Fact]
    public void Initializing_in_non_existing_file_is_not_possible()
    {
        Assert.ThrowsAny<Exception>(() => Canvas.Initialize("not existing path"));
    }

    [Fact]
    public void Canvas_can_be_initialized_in_empty_canvas_file()
    {
        File.WriteAllText("empty_file.canvas", "{}");
        var canvas = Canvas.Initialize("empty_file.canvas");
        Assert.NotNull(canvas);
    }

    [Fact]
    public void Canvas_can_be_initialized_in_existing_canvas()
    {
        var exampleContent =
            "{\n\t\"nodes\":[\n\t\t{\"id\":\"15552bd146acd62c\",\"type\":\"group\",\"x\":43,\"y\":-207,\"width\":587,\"height\":100,\"color\":\"2\",\"label\":\"This is a group\"},\n\t\t{\"id\":\"058452a8fa08898d\",\"type\":\"link\",\"url\":\"https://www.pawelkowalik.com/obsidian/\",\"x\":70,\"y\":140,\"width\":610,\"height\":400},\n\t\t{\"id\":\"ecb5392f059d1156\",\"type\":\"file\",\"file\":\"docs/_attachments/Pasted image 20240505123925.png\",\"x\":260,\"y\":-39,\"width\":400,\"height\":139},\n\t\t{\"id\":\"0671dbd17ef83214\",\"type\":\"text\",\"text\":\"# group 1\",\"x\":63,\"y\":-187,\"width\":250,\"height\":60},\n\t\t{\"id\":\"454c6e9d475df2ec\",\"type\":\"text\",\"text\":\"## group 2\",\"x\":360,\"y\":-187,\"width\":250,\"height\":60,\"color\":\"3\"},\n\t\t{\"id\":\"92d802fea7ceb339\",\"type\":\"text\",\"text\":\"This is card\",\"x\":-305,\"y\":-168,\"width\":250,\"height\":60},\n\t\t{\"id\":\"a8a6b51e70f9d17d\",\"type\":\"text\",\"text\":\"This is another card\",\"x\":-55,\"y\":0,\"width\":250,\"height\":60},\n\t\t{\"id\":\"9238c230c63c283e\",\"type\":\"file\",\"file\":\"README.md\",\"x\":-380,\"y\":140,\"width\":400,\"height\":400}\n\t],\n\t\"edges\":[\n\t\t{\"id\":\"4695a42756603a3d\",\"fromNode\":\"92d802fea7ceb339\",\"fromSide\":\"bottom\",\"toNode\":\"a8a6b51e70f9d17d\",\"toSide\":\"left\",\"label\":\"This is link\"}\n\t]\n}";
        File.WriteAllText("example.canvas", exampleContent);
        var canvas = Canvas.Initialize("example.canvas");
        Assert.NotNull(canvas);
    }
    
}