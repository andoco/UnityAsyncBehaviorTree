using System.Threading.Tasks;

public class SequenceTask : BaseTask
{
    public override async Task<TaskResult> Run()
    {
        foreach (var child in Children)
        {
            var result = await child.Run();

            if (result == TaskResult.Failure)
            {
                return TaskResult.Failure;
            }
        }

        return TaskResult.Success;
    }
}
