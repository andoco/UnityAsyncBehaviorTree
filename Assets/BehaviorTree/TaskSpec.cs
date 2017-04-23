using System.Collections.Generic;

public class TaskSpec
{
    public TaskSpec(string name)
    {
        this.Name = name;
        this.Children = new List<TaskSpec>(0);
        this.Args = new Dictionary<string, string>();
    }

    public string Name { get; }

    public IList<TaskSpec> Children { get; }

    public IDictionary<string, string> Args { get; }
}
