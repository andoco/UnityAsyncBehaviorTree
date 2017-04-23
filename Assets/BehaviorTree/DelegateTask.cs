using System;
using System.Threading.Tasks;

public class DelegateTask : BaseTask
{
    Func<Task<TaskResult>> taskFunc;

    public DelegateTask(Func<Task<TaskResult>> taskFunc)
    {
        this.taskFunc = taskFunc;
    }

    public override async Task<TaskResult> Run()
    {
        return await taskFunc();
    }
}
