# SemanticKernel-Demo

A basic demonstration of Microsoft Semantic Kernel showing simple prompt execution with OpenAI.

## Overview

This project demonstrates the most fundamental use case of Semantic Kernel:
- Basic kernel setup with OpenAI integration
- Simple prompt execution
- Configuration management using appsettings.json

## Features

- Simple question-answering using OpenAI GPT models
- Configuration-driven API key and model management
- Minimal setup for getting started with Semantic Kernel

## Prerequisites

- .NET 9.0 SDK
- OpenAI API key

## Setup

1. **Clone the repository** (if not already done):
   ```bash
   git clone <repository-url>
   cd SemanticKernel-Demo
   ```

2. **Configure OpenAI API**:
   Create an `appsettings.json` file in the `SemanticKernel-Demo` directory:
   ```json
   {
     "OpenAI": {
       "Model": "gpt-4o-mini",
       "ApiKey": "YOUR-OPENAI-API-KEY-HERE"
     }
   }
   ```

3. **Install dependencies**:
   ```bash
   dotnet restore
   ```

## Running the Application

### From the solution root:
```bash
dotnet run --project SemanticKernel-Demo
```

### From the project directory:
```bash
cd SemanticKernel-Demo
dotnet run
```

## What It Does

The application will:
1. Load configuration from `appsettings.json`
2. Initialize a Semantic Kernel with OpenAI chat completion
3. Execute a simple prompt: "What is the capital of France?"
4. Display the AI's response

## Code Structure

```
SemanticKernel-Demo/
├── Program.cs              # Main application entry point
├── appsettings.json        # Configuration (create this file)
└── SemanticKernel-Demo.csproj
```

## Key Concepts Demonstrated

1. **Kernel Creation**: Setting up the basic Semantic Kernel instance
2. **AI Service Integration**: Connecting to OpenAI's chat completion service
3. **Configuration Management**: Using .NET configuration system for API keys
4. **Simple Prompt Execution**: Direct prompt invocation with `InvokePromptAsync`

## Expected Output

```
Paris
```

## Next Steps

- Check out [SemanticKernel-Demo1](../SemanticKernel-Demo1/README.md) for interactive user input and prompt functions
- See [SemanticKernel-Demo2](../SemanticKernel-Demo2/README.md) for chat completion with plugins
- Explore [SemanticKernel-Demo3](../SemanticKernel-Demo3/README.md) for advanced workflows with external integrations

## Troubleshooting

**Error: "OpenAI API key not configured"**
- Ensure your `appsettings.json` file exists and contains a valid OpenAI API key

**Error: "Unauthorized" or API errors**
- Verify your OpenAI API key is correct and has sufficient credits
- Check that the model name in configuration matches available OpenAI models

## Learn More

- [Microsoft Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/)
- [OpenAI API Documentation](https://platform.openai.com/docs)