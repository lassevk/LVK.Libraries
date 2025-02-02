using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class CalculateSumJobHandler : IJobHandler<CalculateSumJob>
{
    public Task HandleAsync(CalculateSumJob job, CancellationToken cancellationToken)
    {
        job.Result = job.Operand1.Operand + job.Operand2.Operand;
        System.Console.WriteLine("CalculateSumJobHandler.HandleAsync executed");
        System.Console.WriteLine($"Sum = {job.Result}");

        return Task.CompletedTask;
    }
}