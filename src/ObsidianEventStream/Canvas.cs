using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ObsidianEventStream;

public class Canvas
{
    public static Canvas? Initialize(string? canvasFilePath)
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

    private Canvas(string? filePath, CanvasStructure structure)
    {
        this.filePath = filePath;
        this.structure = structure;
    }

    private readonly string? filePath;
    private readonly CanvasStructure structure;

    public override string ToString()
    {
        return $"Canvas path: [{filePath}]. Nodes: [{structure.nodes.Count}]. Edges: [{structure.edges.Count}]";
    }

    // public void AnalyzeEvents(string eventsFilePath, string title, string extract, string promote, Func<string,string>? findJson = null)
    public void AnalyzeEvents(string eventsFilePath, Configuration configuration)
    {
        if (File.Exists(eventsFilePath) == false)
        {
            throw new Exception("Events file missing");
        }

        var fileContent = File.ReadAllText(eventsFilePath);

        var events = new List<StreamEvent>();
        
        ParseStreamEvents(fileContent, configuration, events);
        ShowOnCanvas(events);

        foreach (var streamEvent in events)
        {
            Console.WriteLine(streamEvent.Title);
            foreach (var key in streamEvent.Extracted.Keys)
            {
                Console.WriteLine(streamEvent.Extracted[key]);
            }
        }
    }

    private void ShowOnCanvas(List<StreamEvent> events)
    {
        int x = 0;
        int y = 0;
        int width = 800;
        int height = 400;
        int shift_x = 0;
        int shift_y = height + 50;
        int shift_x_for_promotion = 1000;
        for (int i = 0; i < events.Count; i++)
        {
            var streamEvent = events[i];
            Console.WriteLine("Adding " + streamEvent.Title);
            var thisNodeId = Guid.NewGuid().ToString(); 
            this.structure.nodes.Add(new Node
            {
                text = GetTextForStreamEvent(streamEvent),
                id = thisNodeId,
                x = x + i * shift_x,
                y = y + i * shift_y,
                width = width,
                height = height,
                type = "text"
            });
            var promoted = string.Join("\r\n", streamEvent.Promoted.Select(x => $"{x.Key} = {x.Value}"));
            if (promoted.Length > 0)
            {
                var promotedNodeId = Guid.NewGuid().ToString();
                this.structure.nodes.Add(new Node
                {
                    text = promoted,
                    id = promotedNodeId,
                    x = x + i * shift_x + shift_x_for_promotion,
                    y = y + i * shift_y,
                    width = width / 2,
                    height = height,
                    type = "text"
                });
                structure.edges.Add(new Edge
                {
                    id = Guid.NewGuid().ToString(),
                    fromNode = thisNodeId,
                    toNode =promotedNodeId,
                    fromSide = "right",
                    toSide = "left"
                });
            }
        }

        Show();
    }

    private static string GetTextForStreamEvent(StreamEvent streamEvent)
    {
        string text = "";
        text += $"# {streamEvent.Title}\r\n";
        foreach (var extractedKey in streamEvent.Extracted.Keys)
        {
            text += $"{extractedKey} = {streamEvent.Extracted[extractedKey]}\r\n";
        }

        text += "```json\r\n" +
                $"{streamEvent.FullJson}" +
                "\r\n" +
                "```\r\n" +
                "";
        return text;
    }

    private void Show()
    {
        Console.WriteLine(this.structure.nodes.Count);
        var content = JsonSerializer.Serialize(structure);
        File.WriteAllText(this.filePath, content);
        Console.WriteLine(">>>>>" + this.filePath + content);
    }

    private void ParseStreamEvents(string fileContent, Configuration configuration, List<StreamEvent> events)
    {
        try
        {
            if (configuration.findJson != null)
            {
                var lines = fileContent.Split(Environment.NewLine);
                foreach (var line in lines)
                {
                    var element = configuration.findJson(line);
                    var json = JsonDocument.Parse(element);
                    if (json.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        var streamEvent = new StreamEvent();

                        streamEvent.Title = GetPropertyWithPath(json.RootElement, configuration.title);
                        var toExtract = configuration.extract.Split(';', StringSplitOptions.RemoveEmptyEntries);
                        var toPromote = configuration.promote.Split(';', StringSplitOptions.RemoveEmptyEntries);

                        foreach (var extractThis in toExtract)
                        {
                            streamEvent.Extracted.Add(extractThis, GetPropertyWithPath(json.RootElement, extractThis));
                        }

                        foreach (var promoteThis in toPromote)
                        {
                            streamEvent.Promoted.Add(promoteThis, GetPropertyWithPath(json.RootElement, promoteThis));
                        }

                        streamEvent.FullJson = json.RootElement.GetRawText();

                        events.Add(streamEvent);
                    }
                }
            }
            else
            {
                var json = JsonDocument.Parse(fileContent);
                Console.WriteLine(json.RootElement.ValueKind);
                if (json.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in json.RootElement.EnumerateArray())
                    {
                        // Console.WriteLine(element.ValueKind);
                        if (element.ValueKind == JsonValueKind.Object)
                        {
                            var streamEvent = new StreamEvent();

                            streamEvent.Title = GetPropertyWithPath(element, configuration.title);
                            var toExtract = configuration.extract.Split(';', StringSplitOptions.RemoveEmptyEntries);
                            var toPromote = configuration.promote.Split(';', StringSplitOptions.RemoveEmptyEntries);

                            foreach (var extractThis in toExtract)
                            {
                                streamEvent.Extracted.Add(extractThis, GetPropertyWithPath(element, extractThis));
                            }

                            foreach (var promoteThis in toPromote)
                            {
                                streamEvent.Promoted.Add(promoteThis, GetPropertyWithPath(element, promoteThis));
                            }

                            streamEvent.FullJson = element.GetRawText();

                            events.Add(streamEvent);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private string GetPropertyWithPath(JsonElement element, string propPath)
    {
        if (propPath.Contains('.') == false)
        {
            var propValue = element.GetProperty(propPath);
            if (propValue.ValueKind == JsonValueKind.String) return propValue.ToString();
            throw new Exception($"Cannot find property [{propPath}], kind is [{propValue.ValueKind}]");
        }
        
        var names = propPath.Split('.', StringSplitOptions.RemoveEmptyEntries);
        var firstLevelProp = element.GetProperty(names[0]);
        if (firstLevelProp.ValueKind == JsonValueKind.Array)
        {
            var listExtracted = new List<string>();
            foreach (var arrayValues in firstLevelProp.EnumerateArray())
            {
                listExtracted.Add(GetPropertyWithPath(arrayValues, string.Join('.', names.Skip(1))));
            }

            return string.Join(';', listExtracted);
        }
        if (firstLevelProp.ValueKind == JsonValueKind.Object)
        {
            return firstLevelProp.GetProperty(names[1]).ToString();
        }
        
        return "";
    }

    public IEnumerable<Node> EnumerateCards()
    {
        return structure.nodes;
    }

    public IEnumerable<Edge> EnumerateLinks()
    {
        return structure.edges;
    }
}

public class StreamEvent
{
    public string Title { get; set; }
    public string FullJson { get; set; }
    public Dictionary<string, string> Extracted { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> Promoted { get; set; } = new Dictionary<string, string>();
}