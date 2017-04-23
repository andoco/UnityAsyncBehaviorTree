using System.Collections.Generic;
using System.Threading.Tasks;

public abstract class BaseTask : ITask
{
    protected BaseTask()
    {
        this.Children = new List<ITask>();
    }

    public IList<ITask> Children { get; }

    public abstract Task<TaskResult> Run();
}
