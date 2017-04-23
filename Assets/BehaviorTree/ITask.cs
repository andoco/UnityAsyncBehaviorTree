using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITask
{
    IList<ITask> Children { get; }

    Task<TaskResult> Run();
}
