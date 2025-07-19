using System.Reflection;

namespace LVK.Bootstrapping.CommandLineArguments;

internal class StringCommandLineArgumentProperty : ICommandLineArgumentProperty
{
    private readonly string _option;
    private readonly PropertyInfo _property;
    private readonly object _commandLineArguments;

    public StringCommandLineArgumentProperty(string option, PropertyInfo property, object commandLineArguments)
    {
        _option = option;
        _property = property;
        _commandLineArguments = commandLineArguments;
    }

    public (bool success, ICommandLineArgumentProperty? property) HandleArgument(string arg)
    {
        _property.SetValue(_commandLineArguments, arg);
        return (true, null);
    }

    public bool ValidateEnd()
    {
        Console.Error.WriteLine($"Command line option '{_option}' was not given a value");
        return false;
    }

    public string GetArgumentHelp() => "<value>";

    public IEnumerable<string> GetHelpLines()
    {
        yield return "provide the value to add to this option, the value is mandatory";

        if (_property.GetValue(_commandLineArguments) is string value && !string.IsNullOrEmpty(value))
        {
            yield return $"default: '{value}'";
        }
    }
}