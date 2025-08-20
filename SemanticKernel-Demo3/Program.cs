using Microsoft.Extensions.Configuration;
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
        builder.Services.Configure<AIServiceConfig>(
            builder.Configuration.GetSection(AIServiceConfig.SectionName));
        
        // Add Semantic Kernel services
        builder.Services.AddKernel();
        
        // Configure OpenAI from appsettings.json via AIServiceConfig
        var aiConfig = builder.Configuration
            .GetSection(AIServiceConfig.SectionName)
            .Get<AIServiceConfig>() ?? new AIServiceConfig();
        if (string.IsNullOrWhiteSpace(aiConfig.ApiKey))
        {
            throw new InvalidOperationException("OpenAI:ApiKey not configured in appsettings.json");
        }
        builder.Services.AddOpenAIChatCompletion(
            modelId: aiConfig.Model,
            apiKey: aiConfig.ApiKey
        );
        
        // Register plugins and services
        builder.Services.AddSingleton<WebScrapingPlugin>();
        builder.Services.AddSingleton<NotionPlugin>();
        builder.Services.AddSingleton<ArticleSummarizerService>();
        
        var host = builder.Build();
        
        var summarizerService = host.Services.GetRequiredService<ArticleSummarizerService>();
        var notionConfig = host.Services.GetRequiredService<IOptions<NotionConfig>>().Value;
        
        // Get URL from command line args or prompt user
        string articleUrl;
        if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
        {
            articleUrl = args[0];
            Console.WriteLine($"Using URL from command line: {articleUrl}");
        }
        else
        {
            Console.Write("Enter the article URL to summarize: ");
            articleUrl = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrWhiteSpace(articleUrl))
            {
                Console.WriteLine("No URL provided. Exiting.");
                return;
            }
        }
        
        var databaseId = notionConfig.DefaultDatabaseId;
        
        try
        {
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