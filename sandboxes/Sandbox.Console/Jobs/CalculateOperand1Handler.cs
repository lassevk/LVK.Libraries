using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class CalculateOperand1Handler : IJobHandler<CalculateOperand1Job>
{
    public async Task HandleAsync(CalculateOperand1Job job, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        job.Operand = 17;
        System.Console.WriteLine($"Operand 1 = {job.Operand}");
    }
}