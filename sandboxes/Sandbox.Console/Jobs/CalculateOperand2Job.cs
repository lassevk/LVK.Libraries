using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("calc-2")]
public class CalculateOperand2Job : Job
{
    public int Operand { get; set; }
}