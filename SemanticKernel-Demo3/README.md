# SemanticKernel-Demo3

An advanced AI workflow that scrapes web articles, generates AI summaries, and saves them to Notion using Microsoft Semantic Kernel.

## Overview

This project demonstrates production-ready Semantic Kernel features:
- Complex multi-step AI workflows
- Web scraping integration
- External API integration (Notion)
- Dependency injection with Microsoft.Extensions.Hosting
- Configuration management and validation
- Comprehensive error handling and logging

## Features

- **Web Article Scraping**: Extract content from web articles automatically
- **AI-Powered Summarization**: Generate comprehensive summaries with character limits
- **Notion Integration**: Save summaries directly to Notion databases
- **Flexible Input**: Accept URLs via command line or interactive prompt
- **Production Architecture**: Full DI container, logging, and configuration management
- **Error Resilience**: Comprehensive error handling with graceful degradation

## Prerequisites

- .NET 9.0 SDK
- OpenAI API key
- Notion API token
- Notion database for storing articles

## Setup

### 1. Navigate to the project directory
```bash
cd SemanticKernel-Demo3
```

### 2. Configure Application Settings

Copy the example configuration and update it with your credentials:
```bash
cp appsettings.example.json appsettings.json
```

Edit `appsettings.json`:
```json
{
  "OpenAI": {
    "Model": "gpt-4o-mini",
    "ApiKey": "YOUR-OPENAI-API-KEY"
  },
  "Notion": {
    "ApiToken": "YOUR-NOTION-API-TOKEN",
    "DefaultDatabaseId": "YOUR-DATABASE-ID",
    "TitlePropertyName": "Name",
    "ContentPropertyName": "Summary",
    "SourceUrlPropertyName": "Source URL"
  },
  "WebScraping": {
    "TimeoutSeconds": 30,
    "UserAgent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
    "MaxContentLength": 50000
  }
}
```

### 3. Set Up Notion Integration

1. **Create a Notion Integration**:
   - Go to [Notion Developers](https://developers.notion.com/)
   - Create a new integration
   - Copy the API token

2. **Create a Database**:
   - Create a new Notion database
   - Add properties:
     - `Name` (Title)
     - `Summary` (Rich text)
     - `Source URL` (URL)
   - Copy the database ID from the URL

3. **Share Database with Integration**:
   - Click "Share" on your database
   - Add your integration and give it edit permissions

### 4. Install Dependencies
```bash
dotnet restore
```

## Running the Application

### Method 1: Command Line with URL
```bash
# From solution root
dotnet run --project SemanticKernel-Demo3 "https://example.com/article"

# From project directory
cd SemanticKernel-Demo3
dotnet run "https://example.com/article"
```

### Method 2: Interactive Mode
```bash
# From solution root
dotnet run --project SemanticKernel-Demo3

# From project directory
cd SemanticKernel-Demo3
dotnet run
```

The application will prompt you for a URL:
```
Enter the article URL to summarize: https://example.com/article
```

## Example Usage

```bash
$ dotnet run --project SemanticKernel-Demo3 "https://www.anthropic.com/news/visible-extended-thinking"

Using URL from command line: https://www.anthropic.com/news/visible-extended-thinking
info: SemanticKernel_Demo3.Services.ArticleSummarizerService[0]
      Starting article summarization workflow for URL: https://www.anthropic.com/news/visible-extended-thinking
info: SemanticKernel_Demo3.Services.ArticleSummarizerService[0]
      Generating AI summary...
info: SemanticKernel_Demo3.Services.ArticleSummarizerService[0]
      Creating Notion note...
info: SemanticKernel_Demo3.NotionPlugin[0]
      Creating Notion page: Summary: Visible Extended Thinking
info: SemanticKernel_Demo3.NotionPlugin[0]
      Successfully added content to page
info: SemanticKernel_Demo3.Services.ArticleSummarizerService[0]
      Workflow completed successfully. Notion page ID: abc123def456
Article successfully summarized and saved to Notion!
```

## Architecture

### Project Structure
```
SemanticKernel-Demo3/
├── Program.cs                          # Application entry point and DI setup
├── Services/
│   └── ArticleSummarizerService.cs     # Main workflow orchestrator
├── Plugins/
│   ├── NotionPlugin.cs                 # Notion API integration plugin
│   └── WebScrapingPlugin.cs           # Web scraping plugin
├── Configuration/
│   ├── AIServiceConfig.cs              # AI service configuration
│   ├── NotionConfig.cs                 # Notion API configuration
│   └── WebScrapingConfig.cs           # Web scraping configuration
├── appsettings.json                    # Your configuration (create from example)
├── appsettings.example.json            # Configuration template
└── SemanticKernel-Demo3.csproj
```

### Workflow Overview

1. **URL Input**: Accept article URL from command line or user prompt
2. **Content Extraction**: Scrape article content using WebScrapingPlugin
3. **AI Summarization**: Generate structured summary with character limits
4. **Title Extraction**: Extract or generate appropriate title
5. **Notion Integration**: Save summary to Notion database with metadata

## Key Components

### ArticleSummarizerService
Orchestrates the entire workflow:
- Coordinates between plugins
- Manages error handling
- Provides logging throughout the process

### WebScrapingPlugin
- Extracts article content from URLs
- Handles various website formats
- Includes timeout and error handling

### NotionPlugin
- Creates pages in Notion databases
- Handles content truncation for API limits
- Supports optional metadata (source URL, etc.)

### AI Summarization
- Custom prompt engineering for consistent output
- 2000-character limit to fit Notion constraints
- Structured format with overview, key points, and conclusions

## Configuration

### OpenAI Settings
- `Model`: OpenAI model to use (recommended: "gpt-4o-mini")
- `ApiKey`: Your OpenAI API key

### Notion Settings
- `ApiToken`: Notion integration token
- `DefaultDatabaseId`: Database ID for storing articles
- `TitlePropertyName`: Database property for article titles
- `ContentPropertyName`: Database property for content/summary
- `SourceUrlPropertyName`: Database property for source URLs

### Web Scraping Settings
- `TimeoutSeconds`: Request timeout (default: 30)
- `UserAgent`: Browser user agent string
- `MaxContentLength`: Maximum content length to extract

## Error Handling

The application includes comprehensive error handling:

- **Configuration Validation**: Checks for required settings at startup
- **Network Timeouts**: Graceful handling of slow/unresponsive websites
- **API Errors**: Detailed logging of OpenAI and Notion API issues
- **Content Issues**: Fallback strategies for content extraction failures
- **Partial Success**: Notion page creation succeeds even if content addition fails

## Next Steps

- Return to [SemanticKernel-Demo2](../SemanticKernel-Demo2/README.md) for chat completion with plugins
- Check out [SemanticKernel-Demo1](../SemanticKernel-Demo1/README.md) for interactive prompt functions
- See [SemanticKernel-Demo](../SemanticKernel-Demo/README.md) for basic prompt execution

## Troubleshooting

### Configuration Issues
**Error: "OpenAI API key not configured"**
- Ensure `appsettings.json` exists and contains valid OpenAI credentials

**Error: "Notion API token not configured"**
- Verify your Notion integration token in `appsettings.json`
- Ensure the integration has access to your database

### Runtime Issues
**Web scraping failures**
- Some websites may block automated requests
- Check if the URL is accessible and contains readable content
- Try different articles if one consistently fails

**Notion API errors**
- Verify database ID is correct (copy from Notion URL)
- Ensure integration has edit permissions on the database
- Check that property names match your database schema

**Content too long errors**
- The application automatically truncates content to fit Notion limits
- Consider adjusting the AI prompt for shorter summaries

### Performance
**Slow response times**
- Web scraping can take time for large articles
- OpenAI API calls may vary in response time
- Check your internet connection and API rate limits

## Extending the Application

### Adding New Content Sources
Extend `WebScrapingPlugin` to support:
- PDFs
- Academic papers
- Social media posts
- Video transcriptions

### Enhancing Summarization
Modify the prompt in `ArticleSummarizerService` to:
- Add different summary styles (bullet points, executive summary, etc.)
- Include sentiment analysis
- Extract key quotes or statistics
- Generate tags or categories

### Additional Integrations
Create new plugins for:
- Other note-taking apps (Obsidian, Roam)
- Project management tools (Trello, Asana)
- Email systems
- Slack notifications

## Learn More

- [Microsoft Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/)
- [Notion API Documentation](https://developers.notion.com/)
- [OpenAI API Documentation](https://platform.openai.com/docs)
- [Microsoft Extensions Hosting](https://learn.microsoft.com/en-us/dotnet/core/extensions/hosting)