namespace LVK.Bootstrapping.CommandLineArguments;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class CommandLineOptionAttribute : Attribute
{
    public CommandLineOptionAttribute(string option)
    {
        Option = option ?? throw new ArgumentNullException(nameof(option));
    }

    public string Option { get; }
}