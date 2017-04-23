using System.Threading.Tasks;

public class SuccessTask : BaseTask
{
    public override Task<TaskResult> Run()
    {
        return Task.FromResult(TaskResult.Success);
    }
}
