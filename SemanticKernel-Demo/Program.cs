using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var model = config["OpenAI:Model"];
var apiKey = config["OpenAI:ApiKey"];

var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(model, apiKey)
    .Build();

var result = await kernel.InvokePromptAsync("What is the capital of France?");
Console.WriteLine(result);