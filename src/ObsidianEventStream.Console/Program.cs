using ObsidianEventStream.Canvas;

Console.WriteLine("Testing ObsidianEventStream");

var canvasFilePath = @"C:\_projects\ObsidianEventStream\docs\Empty canvas created in Obsidian.canvas";
var canvas = Canvas.Initialize(canvasFilePath);

Console.WriteLine($"Canvas: {canvas}");
