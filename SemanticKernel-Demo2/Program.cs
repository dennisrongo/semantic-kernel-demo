using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using SemanticKernel_Demo2;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var model = config["OpenAI:Model"];
var apiKey = config["OpenAI:ApiKey"];

var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(model, apiKey)
    .Build();

kernel.Plugins.AddFromType<TaskPlugin>("Task");

// Chat completion with auto tool invocation
var chat = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();
history.AddSystemMessage("You are a helpful task assistant. Use available tools to add tasks or show tasks when appropriate.");

var settings = new OpenAIPromptExecutionSettings
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

Console.WriteLine("Type 'exit' or press Enter on an empty line to quit.\n");
while (true)
{
    Console.Write("You: ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    history.AddUserMessage(input);
    var response = await chat.GetChatMessageContentAsync(history, settings, kernel);
    var content = response.Content ?? string.Empty;
    Console.WriteLine($"AI: {content}\n");
    history.AddAssistantMessage(content);
}