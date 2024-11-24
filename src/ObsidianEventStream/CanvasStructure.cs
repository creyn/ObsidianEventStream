namespace ObsidianEventStream;

public class CanvasStructure
{
    public List<Node> nodes { get; set; } = new List<Node>();
    public List<Edge> edges { get; set; } = new List<Edge>();
};

public class Node
{
    public string id { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public string color { get; set; }
    public string type { get; set; }
    public string label { get; set; }
    public string text { get; set; }
};

public class Edge
{
    public string id { get; set; }
    public string fromNode { get; set; }
    public string fromSide { get; set; }
    public string toNode { get; set; }
    public string toSide { get; set; }
    public string label { get; set; }
};
