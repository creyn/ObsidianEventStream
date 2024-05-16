namespace ObsidianEventStream.Canvas;

internal class CanvasStructure
{
    internal List<Node> nodes = new List<Node>();
    internal List<Edge> edges = new List<Edge>();
};

internal class Node
{
    internal string id { get; set; }
    internal int x { get; set; }
    internal int y { get; set; }
    internal int width { get; set; }
    internal int height { get; set; }
    internal string color { get; set; }
    internal string type { get; set; }
    internal string label { get; set; }
};

internal class Edge
{
    internal string id { get; set; }
    internal string fromNode { get; set; }
    internal string fromSide { get; set; }
    internal string toNode { get; set; }
    internal string toSide { get; set; }
    internal string label { get; set; }
};
