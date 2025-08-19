# SemanticKernel-Demo1

An interactive AI-powered to-do list assistant using Microsoft Semantic Kernel with custom prompt functions.

## Overview

This project demonstrates:
- Interactive user input handling
- Custom prompt function creation using `CreateFunctionFromPrompt`
- Parameterized prompts with template variables
- Real-time AI assistance for task organization

## Features

- Interactive command-line interface
- AI-powered conversion of natural language into structured to-do lists
- Custom prompt engineering for task organization
- Template-based prompt system with user input substitution

## Prerequisites

- .NET 9.0 SDK
- OpenAI API key

## Setup

1. **Navigate to the project directory**:
   ```bash
   cd SemanticKernel-Demo1
   ```

2. **Configure OpenAI API**:
   Create an `appsettings.json` file in the `SemanticKernel-Demo1` directory:
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
dotnet run --project SemanticKernel-Demo1
```

### From the project directory:
```bash
cd SemanticKernel-Demo1
dotnet run
```

## How to Use

1. **Start the application** - you'll see a prompt asking what you need help with
2. **Enter your tasks** in natural language, for example:
   - "I need to grocery shop, clean the house, and call my mom"
   - "Plan a birthday party for next weekend"
   - "Prepare for my job interview tomorrow"
3. **Receive structured output** - the AI will convert your input into a numbered to-do list

## Example Usage

```
What do you need help with? I need to prepare for my presentation tomorrow

To-do list:
1. Research and gather relevant data for your presentation topic
2. Create an outline structure for your presentation
3. Design slides with key points and visuals
4. Practice your delivery and timing
5. Prepare backup materials and notes
6. Set up technical equipment and test connectivity
7. Get a good night's sleep before the presentation
```

## Code Structure

```
SemanticKernel-Demo1/
├── Program.cs                    # Main application with interactive loop
├── appsettings.json             # Configuration (create this file)
└── SemanticKernel-Demo1.csproj
```

## Key Concepts Demonstrated

1. **Custom Prompt Functions**: Using `CreateFunctionFromPrompt()` to create reusable AI functions
2. **Template Variables**: Using `{{$input}}` syntax for parameter substitution
3. **Interactive Input**: Collecting and processing user input in real-time
4. **Kernel Arguments**: Passing parameters to functions using `KernelArguments`

## Prompt Engineering

The application uses a structured prompt template:
```
You're an AI to-do list assistant. Convert the user input into a numbered to-do list.
---
User: {{$input}}
```

This template:
- Defines the AI's role and task
- Provides clear instructions for output format
- Uses parameter substitution for dynamic content

## Expected Output Format

The AI will convert any natural language input into a structured, numbered to-do list that breaks down complex tasks into actionable items.

## Next Steps

- Check out [SemanticKernel-Demo2](../SemanticKernel-Demo2/README.md) for chat completion with function calling and plugins
- Explore [SemanticKernel-Demo3](../SemanticKernel-Demo3/README.md) for advanced workflows with external API integrations
- Return to [SemanticKernel-Demo](../SemanticKernel-Demo/README.md) for the basic prompt execution example

## Troubleshooting

**Error: "OpenAI API key not configured"**
- Ensure your `appsettings.json` file exists in the project directory and contains a valid OpenAI API key

**Poor AI responses**
- Try being more specific in your input
- The AI works best with clear, actionable requests

**Application exits immediately**
- Check that you have the correct .NET version installed
- Ensure all dependencies are restored with `dotnet restore`

## Customization

You can modify the prompt template in `Program.cs` to change how the AI interprets and formats your tasks:
- Change the output format (bullets instead of numbers)
- Add time estimates for each task
- Include priority levels
- Add context-specific instructions

## Learn More

- [Microsoft Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/)
- [Prompt Engineering Best Practices](https://platform.openai.com/docs/guides/prompt-engineering)