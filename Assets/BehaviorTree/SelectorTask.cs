using System.Threading.Tasks;

public class SelectorTask : BaseTask
{
    public override async Task<TaskResult> Run()
    {
        foreach (var child in Children)
        {
            var result = await child.Run();

            if (result == TaskResult.Success)
            {
                return TaskResult.Success;
            }
        }

        return TaskResult.Failure;
    }
}
