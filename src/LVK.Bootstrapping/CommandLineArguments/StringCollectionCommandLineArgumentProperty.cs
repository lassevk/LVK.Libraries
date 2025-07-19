namespace LVK.Bootstrapping.CommandLineArguments;

internal class StringCollectionCommandLineArgumentProperty : ICommandLineArgumentProperty
{
    private readonly string _option;
    private readonly ICollection<string> _stringCollection;

    private bool _valuesAdded;

    public StringCollectionCommandLineArgumentProperty(string option, ICollection<string> stringCollection)
    {
        _option = option;
        _stringCollection = stringCollection;
    }

    public (bool success, ICommandLineArgumentProperty? property) HandleArgument(string arg)
    {
        _stringCollection.Add(arg);
        _valuesAdded = true;

        return (true, this);
    }

    public bool ValidateEnd()
    {
        if (_valuesAdded)
        {
            return true;
        }

        Console.Error.WriteLine($"Command line option '{_option}' was not given a value");
        return false;
    }

    public string GetArgumentHelp() => "<value> [<value> ...]";

    public IEnumerable<string> GetHelpLines()
    {
        yield return "provide values to add to this option, at least one value must be provided";

        if (_stringCollection.Any())
        {
            yield return "default: " + string.Join(", ", _stringCollection.Select(s => $"'{s}'"));
        }
    }
}