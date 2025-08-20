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

Console.WriteLine("Semantic Kernel Demo - Interactive Chat");
Console.WriteLine("Type your questions (or 'exit' to quit):");
Console.WriteLine();

while (true)
{
    Console.Write("You: ");
    var userInput = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(userInput) || userInput.ToLower() == "exit")
        break;
    
    try
    {
        var result = await kernel.InvokePromptAsync(userInput);
        Console.WriteLine($"AI: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    
    Console.WriteLine();
}