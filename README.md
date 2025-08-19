# Microsoft Semantic Kernel Demo Collection

A comprehensive collection of four progressive demo projects showcasing different aspects and capabilities of the Microsoft Semantic Kernel framework with OpenAI integration.

## 🚀 Quick Start

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd SemanticKernel-Demo
   ```

2. **Get your OpenAI API Key**:
   - Sign up at [OpenAI Platform](https://platform.openai.com/)
   - Generate an API key from your dashboard

3. **Choose a demo project** and follow its setup instructions

## 📚 Demo Projects Overview

### [SemanticKernel-Demo](./SemanticKernel-Demo/README.md) - Basic Prompt Execution
**Difficulty: Beginner** | **Setup Time: 2 minutes**

The simplest possible Semantic Kernel implementation.
- Basic kernel setup with OpenAI
- Simple prompt execution
- Configuration management
- Perfect starting point for newcomers

```bash
dotnet run --project SemanticKernel-Demo
```

---

### [SemanticKernel-Demo1](./SemanticKernel-Demo1/README.md) - Interactive AI Assistant
**Difficulty: Beginner** | **Setup Time: 3 minutes**

Interactive AI-powered to-do list assistant.
- User input handling
- Custom prompt functions with templates
- Parameter substitution
- Real-time AI assistance

```bash
dotnet run --project SemanticKernel-Demo1
```

---

### [SemanticKernel-Demo2](./SemanticKernel-Demo2/README.md) - Chat with Function Calling
**Difficulty: Intermediate** | **Setup Time: 5 minutes**

Chat-based AI assistant with plugin system.
- Conversational AI with memory
- Custom plugin development
- Auto-function calling
- Task management capabilities

```bash
dotnet run --project SemanticKernel-Demo2
```

---

### [SemanticKernel-Demo3](./SemanticKernel-Demo3/README.md) - Advanced Workflow Integration
**Difficulty: Advanced** | **Setup Time: 15 minutes**

Production-ready workflow with external integrations.
- Web scraping and content extraction
- AI summarization with limits
- Notion API integration
- Dependency injection and logging
- Command-line and interactive modes

```bash
dotnet run --project SemanticKernel-Demo3 "https://example.com/article"
```

## 🛠️ Prerequisites

- **[.NET 9.0 SDK](https://dotnet.microsoft.com/download)** or later
- **[OpenAI API Key](https://platform.openai.com/)** (required for all demos)
- **[Notion Account](https://notion.so/)** (only for Demo3)

## 📋 Common Setup

All projects require an `appsettings.json` file with OpenAI configuration:

```json
{
  "OpenAI": {
    "Model": "gpt-4o-mini",
    "ApiKey": "YOUR-OPENAI-API-KEY-HERE"
  }
}
```

## 🏗️ Solution Structure

```
SemanticKernel-Demo/
├── README.md                          # This file
├── CLAUDE.md                          # Development guidelines
├── SemanticKernel-Demo.sln           # Solution file
├── SemanticKernel-Demo/              # Basic prompt execution
│   ├── README.md
│   ├── Program.cs
│   └── appsettings.json (create)
├── SemanticKernel-Demo1/             # Interactive to-do assistant
│   ├── README.md
│   ├── Program.cs
│   └── appsettings.json (create)
├── SemanticKernel-Demo2/             # Chat with function calling
│   ├── README.md
│   ├── Program.cs
│   ├── TaskPlugin.cs
│   └── appsettings.json (create)
└── SemanticKernel-Demo3/             # Advanced workflow
    ├── README.md
    ├── Program.cs
    ├── Services/
    ├── Plugins/
    ├── appsettings.example.json
    └── appsettings.json (create)
```

## 🎯 Learning Path

### 1. Start Here: Basic Concepts
Begin with **SemanticKernel-Demo** to understand:
- Kernel initialization
- OpenAI integration
- Basic prompt execution

### 2. Add Interactivity: User Input
Move to **SemanticKernel-Demo1** to learn:
- Custom prompt functions
- Template variables
- User input handling

### 3. Enable Conversations: Chat & Plugins
Progress to **SemanticKernel-Demo2** for:
- Chat completion services
- Plugin development
- Function calling

### 4. Build Production Workflows: Integration
Complete with **SemanticKernel-Demo3** to master:
- Complex multi-step workflows
- External API integration
- Production architecture patterns

## 🔧 Build & Run Commands

### Build All Projects
```bash
dotnet build
```

### Run Specific Projects
```bash
# Basic demo
dotnet run --project SemanticKernel-Demo

# Interactive assistant
dotnet run --project SemanticKernel-Demo1

# Chat with plugins
dotnet run --project SemanticKernel-Demo2

# Advanced workflow
dotnet run --project SemanticKernel-Demo3

# Advanced workflow with URL
dotnet run --project SemanticKernel-Demo3 "https://example.com/article"
```

### Build Specific Projects
```bash
dotnet build SemanticKernel-Demo
dotnet build SemanticKernel-Demo1
dotnet build SemanticKernel-Demo2
dotnet build SemanticKernel-Demo3
```

## 💡 Key Concepts Demonstrated

| Concept | Demo | Demo1 | Demo2 | Demo3 |
|---------|------|-------|-------|-------|
| Basic Kernel Setup | ✅ | ✅ | ✅ | ✅ |
| Prompt Execution | ✅ | ✅ | ✅ | ✅ |
| Custom Functions | ❌ | ✅ | ✅ | ✅ |
| User Input | ❌ | ✅ | ✅ | ✅ |
| Chat Completion | ❌ | ❌ | ✅ | ✅ |
| Plugin Development | ❌ | ❌ | ✅ | ✅ |
| Function Calling | ❌ | ❌ | ✅ | ✅ |
| External APIs | ❌ | ❌ | ❌ | ✅ |
| Dependency Injection | ❌ | ❌ | ❌ | ✅ |
| Production Patterns | ❌ | ❌ | ❌ | ✅ |

## 🚨 Troubleshooting

### Common Issues

**"OpenAI API key not configured"**
- Ensure `appsettings.json` exists in the project directory
- Verify your API key is correct and has sufficient credits

**Build errors**
- Run `dotnet restore` to install dependencies
- Ensure you have .NET 9.0 SDK installed

**Poor AI responses**
- Check your internet connection
- Verify the OpenAI model name in configuration
- Try different prompts or inputs

### Getting Help

1. Check the individual project README files for specific guidance
2. Review the error messages carefully - they often contain helpful information
3. Ensure all prerequisites are properly installed and configured

## 🔗 Additional Resources

- **[Microsoft Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/)**
- **[OpenAI API Documentation](https://platform.openai.com/docs)**
- **[.NET 9.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)**
- **[Notion API Documentation](https://developers.notion.com/)** (for Demo3)

## 🤝 Contributing

This repository is designed for learning and experimentation. Feel free to:
- Fork and modify the examples
- Add your own demo projects
- Improve documentation
- Share your experiences

## 📄 License

This project is open source and available under the MIT License.

---

**Happy Learning with Semantic Kernel! 🚀**