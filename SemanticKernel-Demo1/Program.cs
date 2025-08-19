using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var model = config["OpenAI:Model"];
var apiKey = config["OpenAI:ApiKey"];

var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(model, apiKey)
    .Build();

string prompt = @"
You're an AI to-do list assistant. Convert the user input into a numbered to-do list.
---
User: {{$input}}
";

var todoFunc = kernel.CreateFunctionFromPrompt(prompt);

Console.Write("What do you need help with? ");
var input = Console.ReadLine();

var result = await kernel.InvokeAsync(todoFunc, new KernelArguments { ["input"] = input });
Console.WriteLine("\nTo-do list:\n" + result);