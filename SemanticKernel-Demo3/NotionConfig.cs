namespace SemanticKernel_Demo3;

public class NotionConfig
{
    public const string SectionName = "Notion";
    public string ApiToken { get; set; } = string.Empty;
    public string DefaultDatabaseId { get; set; } = string.Empty;
    
    // Property names in your Notion database
    // Title property is the database's title column (often named "Name")
    public string TitlePropertyName { get; set; } = "Name";
    // Rich text property to store article content
    public string ContentPropertyName { get; set; } = "Content";
    // URL property to store the source URL
    public string SourceUrlPropertyName { get; set; } = "Source URL";
}