using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("calc-sum")]
public class CalculateSumJob : Job
{
    public int Result { get; set; }

    public required CalculateOperand1Job Operand1 { get; init; }

    public required CalculateOperand2Job Operand2 { get; init; }
}