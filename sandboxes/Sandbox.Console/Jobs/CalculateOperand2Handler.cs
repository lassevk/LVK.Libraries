using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class CalculateOperand2Handler : IJobHandler<CalculateOperand2Job>
{
    public async Task HandleAsync(CalculateOperand2Job job, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        job.Operand = 42;
        System.Console.WriteLine("CalculateOperand2Handler.HandleAsync executed");
        System.Console.WriteLine($"Operand 2 = {job.Operand}");
    }
}