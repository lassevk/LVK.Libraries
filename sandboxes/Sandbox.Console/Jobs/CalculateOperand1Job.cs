using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("calc-1")]
public class CalculateOperand1Job : Job
{
    public int Operand { get; set; }
}