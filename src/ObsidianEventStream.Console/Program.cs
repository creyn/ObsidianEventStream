using ObsidianEventStream;

Console.WriteLine("Testing ObsidianEventStream");

var canvasFilePath = @"C:\_projects\ObsidianEventStream\docs\Empty canvas created in Obsidian.canvas";
var canvas = Canvas.Initialize(canvasFilePath);

var eventsFilePath = @"C:\_projects\ObsidianEventStream\src\ObsidianEventStream.Console\TestFiles\SomeRandomJson.json";
canvas.AnalyzeEvents(eventsFilePath, title: "name", extract: "email;registered;friends.name;balance", promote: "company;phone");

Console.WriteLine($"Canvas: {canvas}");

