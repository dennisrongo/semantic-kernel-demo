# SemanticKernel-Demo2

A chat-based AI assistant with function calling capabilities using Microsoft Semantic Kernel plugins.

## Overview

This project demonstrates advanced Semantic Kernel features:
- Chat completion with conversation history
- Custom plugin development with function calling
- Auto-invocation of kernel functions during chat
- Interactive chat loop with persistent context

## Features

- **Interactive Chat Interface**: Continuous conversation with the AI assistant
- **Function Calling**: AI can automatically invoke functions based on user requests
- **Task Management Plugin**: Add and view tasks through natural language
- **Conversation History**: Maintains context throughout the chat session
- **Auto Tool Invocation**: AI automatically calls appropriate functions when needed

## Prerequisites

- .NET 9.0 SDK
- OpenAI API key

## Setup

1. **Navigate to the project directory**:
   ```bash
   cd SemanticKernel-Demo2
   ```

2. **Configure OpenAI API**:
   Create an `appsettings.json` file in the `SemanticKernel-Demo2` directory:
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
dotnet run --project SemanticKernel-Demo2
```

### From the project directory:
```bash
cd SemanticKernel-Demo2
dotnet run
```

## How to Use

1. **Start the application** - you'll see a chat interface
2. **Type your requests** in natural language, such as:
   - "Add a task to buy groceries"
   - "Show me all my tasks"
   - "I need to remember to call my dentist"
   - "What tasks do I have?"
3. **Exit the chat** by typing "exit" or pressing Enter on an empty line

## Example Usage

```
Type 'exit' or press Enter on an empty line to quit.

You: Add a task to prepare for the presentation
AI: Added: prepare for the presentation

You: I also need to buy groceries and call my mom
AI: I've added both tasks for you:
Added: buy groceries
Added: call my mom

You: What tasks do I have?
AI: Here are your current tasks:
prepare for the presentation
buy groceries
call my mom

You: exit
```

## Code Structure

```
SemanticKernel-Demo2/
├── Program.cs                    # Main chat application
├── TaskPlugin.cs                 # Custom plugin with task management functions
├── appsettings.json             # Configuration (create this file)
└── SemanticKernel-Demo2.csproj
```

## Plugin System

### TaskPlugin Class

The `TaskPlugin` demonstrates how to create custom Semantic Kernel plugins:

```csharp
public class TaskPlugin
{
    [KernelFunction, Description("Add a task to the list")]
    public string AddTask(string task) { ... }

    [KernelFunction, Description("Show all tasks in the list")]
    public string ShowTasks() { ... }
}
```

**Key Features:**
- `[KernelFunction]` attribute exposes methods to the AI
- `[Description]` helps the AI understand when to use each function
- Functions are automatically invoked based on user intent

## Key Concepts Demonstrated

1. **Chat Completion Service**: Using `IChatCompletionService` for conversational AI
2. **Plugin Development**: Creating custom plugins with decorated functions
3. **Auto Function Calling**: AI automatically determines when to call functions
4. **Conversation History**: Maintaining context with `ChatHistory`
5. **Tool Call Behavior**: Configuring automatic function invocation

## Technical Implementation

### Auto Tool Invocation
```csharp
var settings = new OpenAIPromptExecutionSettings
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};
```

This setting enables the AI to automatically call appropriate functions based on user requests without explicit prompting.

### System Message
The application sets up the AI with a system message that defines its role:
```
"You are a helpful task assistant. Use available tools to add tasks or show tasks when appropriate."
```

## Advanced Features

- **Persistent Task Storage**: Tasks are stored in memory during the session
- **Natural Language Understanding**: AI interprets user intent to call appropriate functions
- **Context Awareness**: AI remembers previous conversations and tasks
- **Error Handling**: Graceful handling of invalid inputs and function call failures

## Next Steps

- Explore [SemanticKernel-Demo3](../SemanticKernel-Demo3/README.md) for advanced workflows with web scraping and Notion integration
- Return to [SemanticKernel-Demo1](../SemanticKernel-Demo1/README.md) for simpler prompt-based interactions
- Check out [SemanticKernel-Demo](../SemanticKernel-Demo/README.md) for basic prompt execution

## Troubleshooting

**Functions not being called**
- Ensure your `[Description]` attributes clearly describe when to use each function
- Check that your OpenAI model supports function calling (GPT-3.5-turbo and GPT-4 models do)

**Error: "OpenAI API key not configured"**
- Verify your `appsettings.json` file exists and contains a valid API key

**Poor function selection**
- Improve function descriptions to be more specific about their use cases
- Consider the system message and make it more explicit about available tools

## Extending the Plugin

You can easily extend the `TaskPlugin` with additional functions:

```csharp
[KernelFunction, Description("Remove a task from the list")]
public string RemoveTask(string task) { ... }

[KernelFunction, Description("Mark a task as completed")]
public string CompleteTask(string task) { ... }

[KernelFunction, Description("Set a priority for a task")]
public string SetTaskPriority(string task, string priority) { ... }
```

## Learn More

- [Microsoft Semantic Kernel Plugins](https://learn.microsoft.com/en-us/semantic-kernel/concepts/plugins/)
- [Function Calling with OpenAI](https://platform.openai.com/docs/guides/function-calling)
- [Chat Completion API](https://platform.openai.com/docs/api-reference/chat)