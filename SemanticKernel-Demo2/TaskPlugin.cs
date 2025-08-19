using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SemanticKernel_Demo2;
public class TaskPlugin
{
    private readonly List<string> _tasks = new();

    [KernelFunction, Description("Add a task to the list")]
    public string AddTask(string task)
    {
        _tasks.Add(task);
        return $"Added: {task}";
    }

    [KernelFunction, Description("Show all tasks in the list")]
    public string ShowTasks()
    {
        return _tasks.Count == 0 ? "No tasks yet." : string.Join("\n", _tasks);
    }
}
