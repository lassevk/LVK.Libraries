namespace LasseVK.Jobs;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class JobIdentifierAttribute : Attribute
{
    public JobIdentifierAttribute(string identifier)
    {
        Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
    }

    public string Identifier { get; }
}