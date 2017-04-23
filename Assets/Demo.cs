using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Demo : MonoBehaviour
{
    ITask rootTask;

    public TextAsset behaviorSource;

    void Start()
    {
        using (var ms = new MemoryStream(behaviorSource.bytes))
        {
            var reader = new CustomBehaviorReader();
            var rootSpec = reader.BuildRoot(ms);

            var taskMap = new Dictionary<string, ITask>
            {
                { "Seq", new SequenceTask() },
                { "Sel", new SelectorTask() },
                { "Par", new ParallelTask() },
                { "Foo", new DelegateTask(FooAction) },
                { "Bar", new DelegateTask(BarAction) },
                { "Fail", new DelegateTask(() => Task.FromResult(TaskResult.Failure)) }
            };

            rootTask = new BehaviorTreeBuilder(taskMap).Build(rootSpec);
        }

        var result = rootTask.Run().Result;
        Debug.Log($"result = {result}");
    }

    Task<TaskResult> FooAction()
    {
        Debug.Log("Foo");
        return Task.FromResult(TaskResult.Success);
    }

    Task<TaskResult> BarAction()
    {
        Debug.Log("Bar");
        return Task.FromResult(TaskResult.Success);
    }
}
