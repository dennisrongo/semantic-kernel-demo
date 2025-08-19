using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SemanticKernel_Demo3;
using SemanticKernel_Demo3.Plugins;

namespace SemanticKernel_Demo3.Services;

public class ArticleSummarizerService
{
    private readonly Kernel _kernel;
    private readonly ILogger<ArticleSummarizerService> _logger;
    private readonly KernelPlugin _webScrapingPlugin;
    private readonly KernelPlugin _notionPlugin;

    public ArticleSummarizerService(
        Kernel kernel,
        ILogger<ArticleSummarizerService> logger,
        WebScrapingPlugin webScrapingPlugin,
        NotionPlugin notionPlugin)
    {
        _kernel = kernel;
        _logger = logger;
        
        // Import plugins from DI-constructed instances to ensure functions are discovered
        _webScrapingPlugin = _kernel.ImportPluginFromObject(webScrapingPlugin, nameof(WebScrapingPlugin));
        _notionPlugin = _kernel.ImportPluginFromObject(notionPlugin, nameof(NotionPlugin));
    }

    public async Task<string> SummarizeArticleToNotionAsync(string articleUrl, string notionDatabaseId)
    {
        try
        {
            _logger.LogInformation("Starting article summarization workflow for URL: {Url}", articleUrl);

            // Step 1: Extract article content
            var extractedContent = await _kernel.InvokeAsync(
                _webScrapingPlugin[nameof(WebScrapingPlugin.ExtractArticleContentAsync)],
                new KernelArguments { ["url"] = articleUrl }
            );

            var articleContent = extractedContent.GetValue<string>() ?? "";
            
            if (string.IsNullOrWhiteSpace(articleContent))
            {
                throw new InvalidOperationException("No content could be extracted from the article");
            }

            // Step 2: Create summarization function
            var summaryFunction = _kernel.CreateFunctionFromPrompt(@"
You are an expert at creating concise, well-structured summaries of articles.

Please create a comprehensive summary of the following article that includes:
1. A brief overview (2-3 sentences)
2. Key points (3-5 bullet points)
3. Main conclusions or takeaways
4. Any important quotes or statistics mentioned

Make the summary professional and easy to read.

Article Content:
{{$article_content}}

Summary:");

            // Step 3: Generate summary
            _logger.LogInformation("Generating AI summary...");
            var summaryResult = await _kernel.InvokeAsync(summaryFunction, new KernelArguments 
            { 
                ["article_content"] = articleContent 
            });

            var summary = summaryResult.GetValue<string>() ?? "";

            // Step 4: Extract title from content
            var title = ExtractTitleFromContent(articleContent);
            var noteTitle = $"Summary: {title}";

            // Step 5: Create Notion note
            _logger.LogInformation("Creating Notion note...");
            var notionPageId = await _kernel.InvokeAsync(
                _notionPlugin[nameof(NotionPlugin.CreateNoteAsync)],
                new KernelArguments
                {
                    ["title"] = noteTitle,
                    ["content"] = summary,
                    ["databaseId"] = notionDatabaseId,
                    ["sourceUrl"] = articleUrl
                }
            );

            var pageId = notionPageId.GetValue<string>() ?? "";
            
            _logger.LogInformation("Workflow completed successfully. Notion page ID: {PageId}", pageId);
            return pageId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to complete article summarization workflow");
            throw;
        }
    }

    private static string ExtractTitleFromContent(string content)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        // Look for "Title:" prefix
        var titleLine = lines.FirstOrDefault(line => line.StartsWith("Title:", StringComparison.OrdinalIgnoreCase));
        if (titleLine != null)
        {
            return titleLine.Substring(6).Trim();
        }

        // Fallback: use first non-empty line
        return lines.FirstOrDefault()?.Trim() ?? "Untitled Article";
    }
}