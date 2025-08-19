namespace SemanticKernel_Demo3;

public class WebScrapingConfig
{
    public const string SectionName = "WebScraping";
    public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
    public int TimeoutSeconds { get; set; } = 30;
    public int MaxContentChunkSize { get; set; } = 1900;
}