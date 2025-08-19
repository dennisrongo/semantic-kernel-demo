using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using SemanticKernel_Demo3.Plugins;
using SemanticKernel_Demo3.Services;

namespace SemanticKernel_Demo3;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        
        // Configure logging
        builder.Logging.AddConsole();
        
        // Bind configuration sections
        builder.Services.Configure<NotionConfig>(
            builder.Configuration.GetSection(NotionConfig.SectionName));
        builder.Services.Configure<WebScrapingConfig>(
            builder.Configuration.GetSection(WebScrapingConfig.SectionName));
        
        // Add Semantic Kernel services
        builder.Services.AddKernel();
        
        // Configure OpenAI from appsettings.json (section: "OpenAI")
        var openAiSection = builder.Configuration.GetSection("OpenAI");
        var openAiApiKey = openAiSection["ApiKey"];
        var openAiModel = openAiSection["Model"] ?? "gpt-4o-mini";
        if (string.IsNullOrWhiteSpace(openAiApiKey))
        {
            throw new InvalidOperationException("OpenAI:ApiKey not configured in appsettings.json");
        }
        builder.Services.AddOpenAIChatCompletion(
            modelId: openAiModel,
            apiKey: openAiApiKey
        );
        
        // Register plugins and services
        builder.Services.AddSingleton<WebScrapingPlugin>();
        builder.Services.AddSingleton<NotionPlugin>();
        builder.Services.AddSingleton<ArticleSummarizerService>();
        
        var host = builder.Build();
        
        var summarizerService = host.Services.GetRequiredService<ArticleSummarizerService>();
        var notionConfig = host.Services.GetRequiredService<IOptions<NotionConfig>>().Value;
        
        var articleUrl = "https://www.anthropic.com/engineering/claude-code-best-practices"; // Replace with actual URL
        var databaseId = notionConfig.DefaultDatabaseId; // Uses default from config
        
        try
        {
            var notionPlugin = host.Services.GetRequiredService<NotionPlugin>();
            var testPageId = await notionPlugin.CreateNoteAsync("Test Note", "This is a test note", databaseId);
            Console.WriteLine($"Test page created: {testPageId}");
            
            // If test succeeds, try the full workflow
            await summarizerService.SummarizeArticleToNotionAsync(articleUrl, databaseId);
            Console.WriteLine("Article successfully summarized and saved to Notion!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
    }
}