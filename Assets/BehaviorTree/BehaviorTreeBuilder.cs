using System.Collections.Generic;

public class BehaviorTreeBuilder
{
    private readonly IDictionary<string, ITask> taskMap;

    public BehaviorTreeBuilder(IDictionary<string, ITask> taskMap)
    {
        this.taskMap = taskMap;
    }

    public ITask Build(TaskSpec rootSpec)
    {
        return BuildSubtree(rootSpec.Children[0]);
    }

    private ITask BuildSubtree(TaskSpec currentSpec)
    {
        var task = taskMap[currentSpec.Name];

        foreach (var childSpec in currentSpec.Children)
        {
            var childTask = BuildSubtree(childSpec);
            task.Children.Add(childTask);
        }

        return task;
    }
}
