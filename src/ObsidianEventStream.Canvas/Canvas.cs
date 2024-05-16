using System.Text.Json;

namespace ObsidianEventStream.Canvas;

public class Canvas
{
    public static Canvas Initialize(string canvasFilePath)
    {
        var file = new FileInfo(canvasFilePath);
        if (file.Extension != ".canvas")
        {
            throw new Exception($"Canvas filepath: [{canvasFilePath}] should have the '.canvas' extension");
        }
        
        if (file.Exists == false)
        {
            throw new Exception($"Please create empty file in Obsidian first, path: [{canvasFilePath}]");
        }

        var content = File.ReadAllText(canvasFilePath);
        var structure = JsonSerializer.Deserialize<CanvasStructure>(content)!;

        var canvas = new Canvas(canvasFilePath, structure);
        return canvas;
    }

    private Canvas(string filePath, CanvasStructure structure)
    {
        this.filePath = filePath;
        this.structure = structure;
    }

    private readonly string filePath;
    private readonly CanvasStructure structure;

    public override string ToString()
    {
        return $"Canvas path: [{filePath}]. Nodes: [{structure.nodes.Count}]. Edges: [{structure.edges.Count}]";
    }
}