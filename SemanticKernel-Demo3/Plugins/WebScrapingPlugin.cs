using System.ComponentModel;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace SemanticKernel_Demo3.Plugins;

public class WebScrapingPlugin : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WebScrapingPlugin> _logger;
    private readonly WebScrapingConfig _config;

    public WebScrapingPlugin(ILogger<WebScrapingPlugin> logger, IOptions<WebScrapingConfig> config)
    {
        _config = config.Value;
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds)
        };
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _config.UserAgent);
        _logger = logger;
    }

    [KernelFunction, Description("Extract article content from a URL")]
    public async Task<string> ExtractArticleContentAsync(
        [Description("The URL of the article to extract")] string url)
    {
        try
        {
            _logger.LogInformation("Fetching content from URL: {Url}", url);
            
            var response = await _httpClient.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            // Extract title
            var titleNode = doc.DocumentNode.SelectSingleNode("//title") ??
                            doc.DocumentNode.SelectSingleNode("//h1");
            var title = titleNode?.InnerText?.Trim() ?? "Untitled Article";

            // Extract main content (common article selectors)
            var contentSelectors = new[]
            {
                "//article",
                "//main",
                "//*[contains(@class, 'content')]",
                "//*[contains(@class, 'article')]",
                "//*[contains(@class, 'post')]",
                "//div[contains(@class, 'entry')]"
            };

            string content = "";
            foreach (var selector in contentSelectors)
            {
                var contentNode = doc.DocumentNode.SelectSingleNode(selector);
                if (contentNode != null)
                {
                    content = ExtractTextFromNode(contentNode);
                    break;
                }
            }

            // Fallback: extract from body if no content found
            if (string.IsNullOrWhiteSpace(content))
            {
                var bodyNode = doc.DocumentNode.SelectSingleNode("//body");
                content = bodyNode != null ? ExtractTextFromNode(bodyNode) : "";
            }

            var result = $"Title: {title}\n\nContent:\n{content}";
            _logger.LogInformation("Successfully extracted {Length} characters", result.Length);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract content from URL: {Url}", url);
            throw new InvalidOperationException($"Failed to extract article content: {ex.Message}");
        }
    }

    private static string ExtractTextFromNode(HtmlNode node)
    {
        // Remove script and style elements
        var unwantedNodes = node.SelectNodes(".//script | .//style | .//nav | .//header | .//footer | .//aside")?.ToList();
        unwantedNodes?.ForEach(n => n.Remove());

        // Extract text and clean it up
        var text = node.InnerText;
        
        // Clean up whitespace
        var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();

        return string.Join("\n", lines);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}