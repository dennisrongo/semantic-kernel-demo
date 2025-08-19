using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Notion.Client;

namespace SemanticKernel_Demo3;

public class NotionPlugin
{
    private readonly NotionClient _notionClient;
    private readonly ILogger<NotionPlugin> _logger;
    private readonly WebScrapingConfig _webConfig;
    private readonly string _authToken;
    private readonly NotionConfig _notionConfig;

    public NotionPlugin(IOptions<NotionConfig> notionConfig, IOptions<WebScrapingConfig> webConfig, ILogger<NotionPlugin> logger)
    {
        var config = notionConfig.Value;
        if (string.IsNullOrEmpty(config.ApiToken))
            throw new InvalidOperationException("Notion API token not configured");
            
        _authToken = config.ApiToken;
        _notionConfig = config;
        _notionClient = NotionClientFactory.Create(new ClientOptions
        {
            AuthToken = _authToken
        });
        _webConfig = webConfig.Value;
        _logger = logger;
    }

    [KernelFunction, Description("Create a note in Notion with title and content")]
    public async Task<string> CreateNoteAsync(
        [Description("The title of the note")] string title,
        [Description("The content of the note")] string content,
        [Description("The Notion database ID")] string databaseId,
        [Description("The source URL (optional)")] string sourceUrl = "")
    {
        try
        {
            _logger.LogInformation("Creating Notion page: {Title}", title);

            // Step 1: Create the page first (without content blocks for now)
            var properties = new Dictionary<string, PropertyValue>
            {
                [_notionConfig.TitlePropertyName] = new TitlePropertyValue
                {
                    Title = new List<RichTextBase>
                    {
                        new RichTextText
                        {
                            Text = new Text { Content = title }
                        }
                    }
                }
            };

            // Add optional properties if provided/configured
            if (!string.IsNullOrWhiteSpace(content) && !string.IsNullOrEmpty(_notionConfig.ContentPropertyName))
            {
                properties[_notionConfig.ContentPropertyName] = new RichTextPropertyValue
                {
                    RichText = new List<RichTextBase>
                    {
                        new RichTextText
                        {
                            Text = new Text
                            {
                                Content = content.Length > 2000 ? content.Substring(0, 2000) + "..." : content
                            }
                        }
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(sourceUrl) && !string.IsNullOrEmpty(_notionConfig.SourceUrlPropertyName))
            {
                properties[_notionConfig.SourceUrlPropertyName] = new UrlPropertyValue
                {
                    Url = sourceUrl
                };
            }

            var createPageParams = new PagesCreateParameters
            {
                Parent = new DatabaseParentInput
                {
                    DatabaseId = databaseId
                },
                Properties = properties
            };

            var page = await _notionClient.Pages.CreateAsync(createPageParams);
            _logger.LogInformation("Created page with ID: {PageId}", page.Id);

            // Step 2: Add content as a simple paragraph (simplified approach)
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    // Try to add content - if this fails, the page will still exist
                    await AddSimpleContent(page.Id, content);
                }
                catch (Exception contentEx)
                {
                    _logger.LogWarning(contentEx, "Failed to add content to page, but page was created successfully");
                    // Don't throw - we still successfully created the page
                }
            }

            _logger.LogInformation("Successfully created Notion page: {PageId}", page.Id);
            return page.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Notion note: {Message}", ex.Message);
            throw new InvalidOperationException($"Failed to create Notion note: {ex.Message}");
        }
    }

    private async Task AddSimpleContent(string pageId, string content)
    {
        // For now, just create a single paragraph block with the content
        // This is a simplified approach to get it working
        
        // We'll use raw HTTP call since the SDK classes are causing issues
        using var httpClient = new HttpClient();
        
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authToken}");
        httpClient.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");
        
        var requestBody = new
        {
            children = new[]
            {
                new
                {
                    @object = "block",
                    type = "paragraph",
                    paragraph = new
                    {
                        rich_text = new[]
                        {
                            new
                            {
                                type = "text",
                                text = new { content = content.Length > 2000 ? content.Substring(0, 2000) + "..." : content }
                            }
                        }
                    }
                }
            }
        };
        
        var json = JsonSerializer.Serialize(requestBody);
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        
        using var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://api.notion.com/v1/blocks/{pageId}/children")
        {
            Content = requestContent
        };

        var response = await httpClient.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Successfully added content to page");
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to add content: {StatusCode} - {Error}", response.StatusCode, errorContent);
        }
    }

    private async Task AddContentToPage(string pageId, string content)
    {
        // This method has been simplified to avoid SDK compatibility issues
        // Content addition is now handled in AddSimpleContent method
        await AddSimpleContent(pageId, content);
    }

    private static List<string> SplitContentIntoChunks(string content, int maxChunkSize)
    {
        if (string.IsNullOrWhiteSpace(content))
            return new List<string>();

        var chunks = new List<string>();
        var lines = content.Split('\n');
        var currentChunk = new StringBuilder();

        foreach (var line in lines)
        {
            if (currentChunk.Length + line.Length + 1 > maxChunkSize)
            {
                if (currentChunk.Length > 0)
                {
                    chunks.Add(currentChunk.ToString().Trim());
                    currentChunk.Clear();
                }
                
                // Handle very long single lines
                if (line.Length > maxChunkSize)
                {
                    var words = line.Split(' ');
                    var tempChunk = new StringBuilder();
                    
                    foreach (var word in words)
                    {
                        if (tempChunk.Length + word.Length + 1 > maxChunkSize)
                        {
                            if (tempChunk.Length > 0)
                            {
                                chunks.Add(tempChunk.ToString().Trim());
                                tempChunk.Clear();
                            }
                        }
                        tempChunk.Append(word + " ");
                    }
                    
                    if (tempChunk.Length > 0)
                    {
                        currentChunk.Append(tempChunk);
                    }
                }
                else
                {
                    currentChunk.AppendLine(line);
                }
            }
            else
            {
                currentChunk.AppendLine(line);
            }
        }

        if (currentChunk.Length > 0)
        {
            chunks.Add(currentChunk.ToString().Trim());
        }

        return chunks.Where(chunk => !string.IsNullOrWhiteSpace(chunk)).ToList();
    }
}