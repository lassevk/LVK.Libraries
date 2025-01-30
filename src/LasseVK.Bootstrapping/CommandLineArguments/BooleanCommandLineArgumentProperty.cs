using System.Reflection;

namespace LasseVK.Bootstrapping.CommandLineArguments;

internal class BooleanCommandLineArgumentProperty : ICommandLineArgumentProperty
{
    private readonly PropertyInfo _property;
    private readonly object _commandLineArguments;

    public BooleanCommandLineArgumentProperty(PropertyInfo property, object commandLineArguments)
    {
        _property = property;
        _commandLineArguments = commandLineArguments;
    }

    public (bool success, ICommandLineArgumentProperty? property) HandleArgument(string arg)
    {
        bool? result = arg.ToLowerInvariant() switch
        {
            "+"    => true,
            "true" => true,
            "yes"  => true,
            "on"   => true,
            "1"    => true,

            "-"     => false,
            "false" => false,
            "no"    => false,
            "off"   => false,
            "0"     => false,

            _ => null,
        };

        if (result == null)
        {
            return (false, null);
        }

        _property.SetValue(_commandLineArguments, result.Value);
        return (true, null);
    }

    public bool ValidateEnd()
    {
        _property.SetValue(_commandLineArguments, true);
        return true;
    }

    public string GetArgumentHelp() => "<value>";

    public IEnumerable<string> GetHelpLines()
    {
        yield return "enable: provide value '+', 'true', 'yes', 'on', or '1' [default if no value is provided]";
        yield return "disable: provide value '-', 'false', 'no', 'off', or '0'";
    }
}