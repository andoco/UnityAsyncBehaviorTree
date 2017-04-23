using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

public class CustomBehaviorReader
{
    private const string CommentPrefix = "#";
    private const string MappingsArgKey = "mappings";

    private readonly ILineParser parser = new RegexLineParser();
    private string indent;

    public TaskSpec BuildRoot(Stream source)
    {
        return this.BuildRoot(source, new Stack<TaskSpec>());
    }

    private TaskSpec BuildRoot(Stream source, Stack<TaskSpec> stack)
    {
        using (source)
        {
            using (var reader = new StreamReader(source))
            {
                var lastIndent = -1;

                stack.Clear();
                stack.Push(new TaskSpec("ROOT"));

                while (!reader.EndOfStream)
                {
                    Debug.Assert(stack.Count > 0, "Task stack should not be empty");

                    var line = reader.ReadLine();

                    this.EnsureIndentDetected(line);

                    var trimmedLine = this.TrimIndentation(line);

                    // Skip comments.
                    if (trimmedLine.Length == 0 || trimmedLine.StartsWith(CommentPrefix))
                        continue;

                    var task = this.BuildTask(line);

                    var currentIndent = this.CountIndent(line);

                    if (currentIndent > lastIndent)
                    {
                        // We're adding the first child of a task.
                        this.AddToParent(task, stack);

                        stack.Push(task);
                    }
                    else if (currentIndent == lastIndent)
                    {
                        // We're adding a sibling, so first pop the previous child from the stack.
                        stack.Pop();

                        this.AddToParent(task, stack);
                        stack.Push(task);
                    }
                    else
                    {
                        // Pop stack to the corresponding indent level. We add 1 to the indent to account for the RootTask.
                        while (stack.Count > (currentIndent + 1))
                        {
                            stack.Pop();
                        }

                        this.AddToParent(task, stack);
                        stack.Push(task);
                    }

                    lastIndent = currentIndent;
                }

                // Pop stack to get back to the root task.
                while (stack.Count > 1)
                {
                    stack.Pop();
                }

                var root = stack.Peek();
                stack.Clear();

                return root;
            }
        }
    }

    private void EnsureIndentDetected(string line)
    {
        if (!string.IsNullOrEmpty(this.indent))
            return;

        if (line.Length == 0)
            return;

        switch (line[0])
        {
            case '\t':
                this.indent = "\t";
                break;
            case ' ':
                this.indent = line.Substring(0, line.Length - line.TrimStart(' ').Length);
                break;
        }
    }

    private string TrimIndentation(string line)
    {
        if (string.IsNullOrEmpty(this.indent)) return line;

        while (line.StartsWith(this.indent))
        {
            line = line.Substring(this.indent.Length);
        }

        return line;
    }

    private int CountIndent(string line)
    {
        if (string.IsNullOrEmpty(this.indent)) return 0;

        return Regex.Matches(line, this.indent).Count;
    }

    private TaskSpec BuildTask(string line)
    {
        string name;
        IDictionary<string, string> args;

        this.parser.Parse(line, out name, out args);

        var spec = new TaskSpec(name);
        foreach (var item in args) spec.Args.Add(item);

        return spec;
    }

    private void AddToParent(TaskSpec task, Stack<TaskSpec> stack)
    {
        if (stack.Count == 0)
            throw new InvalidOperationException(string.Format("Cannot add the task {0} to its parent as the task stack is empty and no parent can be found", task));

        var parent = stack.Peek();
        parent.Children.Add(task);
    }
}