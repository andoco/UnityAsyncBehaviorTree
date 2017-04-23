using System.Threading.Tasks;
using System.Linq;

public class ParallelTask : BaseTask
{
    public override async Task<TaskResult> Run()
    {
        var tasks = Children.Select(c => c.Run());
        var results = await Task.WhenAll(tasks);

        return results.Any(r => r == TaskResult.Success)
              ? TaskResult.Success
              : TaskResult.Failure;
    }
}