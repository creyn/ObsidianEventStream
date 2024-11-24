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
        var testSystem = TestSystem.InitializeWithEmptyCanvas("empty_file.canvas").Build();
        Assert.NotNull(testSystem.GetFinalCanvas());
    }

    [Fact]
    public void Canvas_can_be_initialized_in_existing_canvas()
    {
        var testSystem = TestSystem.InitializeWithExampleCanvas("example.canvas").Build();
        Assert.NotNull(testSystem.GetFinalCanvas());
    }
    
}