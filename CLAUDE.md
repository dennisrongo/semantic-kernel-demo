# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

This is a Microsoft Semantic Kernel learning repository with multiple demo projects showcasing different aspects of the Semantic Kernel framework. The solution contains four main projects that progressively demonstrate more complex AI integration patterns.

## Architecture

The repository follows a multi-project structure within a single solution:

- **SemanticKernel-Demo**: Basic prompt execution demo
- **SemanticKernel-Demo1**: Interactive AI to-do list assistant
- **SemanticKernel-Demo2**: Chat-based AI assistant with function calling and plugins
- **SemanticKernel-Demo3**: Advanced AI workflow with web scraping, article summarization, and Notion integration

All projects use .NET 9.0 and the Microsoft Semantic Kernel framework for AI functionality.

## Common Commands

### Build
```bash
dotnet build
```

### Run Individual Projects
```bash
# Basic demo
dotnet run --project SemanticKernel-Demo

# To-do list assistant
dotnet run --project SemanticKernel-Demo1

# Chat assistant with plugins
dotnet run --project SemanticKernel-Demo2

# Article summarizer with Notion integration
dotnet run --project SemanticKernel-Demo3
```

### Build Specific Projects
```bash
dotnet build SemanticKernel-Demo
dotnet build SemanticKernel-Demo1
dotnet build SemanticKernel-Demo2
dotnet build SemanticKernel-Demo3
```

## Configuration

### Required Configuration Files
Each project requires an `appsettings.json` file with OpenAI API configuration:

```json
{
  "OpenAI": {
    "Model": "gpt-4o-mini",
    "ApiKey": "YOUR-API-KEY"
  }
}
```

### Demo3 Additional Configuration
SemanticKernel-Demo3 requires additional configuration for Notion and web scraping:
- Copy `appsettings.example.json` to `appsettings.json`
- Configure Notion API token and database ID
- Configure web scraping settings

## Key Components

### SemanticKernel-Demo3 Architecture
- **Program.cs**: Dependency injection setup and main workflow orchestration
- **Services/ArticleSummarizerService.cs**: Core service that orchestrates web scraping → AI summarization → Notion integration
- **Plugins/NotionPlugin.cs**: Semantic Kernel plugin for Notion API integration
- **Plugins/WebScrapingPlugin.cs**: Plugin for extracting content from web articles
- **Configuration classes**: AIServiceConfig, NotionConfig, WebScrapingConfig for typed configuration

### Plugin System
SemanticKernel-Demo2 and Demo3 demonstrate the Semantic Kernel plugin architecture:
- Functions decorated with `[KernelFunction]` and `[Description]` attributes
- Auto-invocation of functions during chat completion
- Type-safe function parameters with descriptions

### Dependency Injection Integration
Demo3 shows full integration with Microsoft.Extensions.Hosting:
- Service registration for all components
- Configuration binding from appsettings.json
- Logging integration
- Proper DI container usage for plugin instantiation

## Development Notes

### API Keys and Security
- All projects require OpenAI API keys in appsettings.json
- Demo3 also requires Notion API token
- Use appsettings.example.json as template for required configuration structure
- Never commit actual API keys to the repository

### Error Handling
- Demo3 implements comprehensive error handling with logging
- Failed content addition to Notion pages doesn't fail the entire operation
- Configuration validation at startup prevents runtime failures

### Semantic Kernel Patterns
- Basic prompt execution (Demo 1)
- Function creation from prompts (Demo 1)
- Plugin development with function calling (Demo 2)
- Complex workflows with multiple AI services (Demo 3)
- Chat completion with auto tool invocation (Demo 2)