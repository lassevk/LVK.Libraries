namespace LVK;

public class AssertionFailedException : InvalidOperationException
{
    public AssertionFailedException(string message)
        : base(message)
    {
    }
}