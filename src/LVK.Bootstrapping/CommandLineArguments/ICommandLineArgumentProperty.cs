namespace LVK.Bootstrapping.CommandLineArguments;

internal interface ICommandLineArgumentProperty
{
    (bool success, ICommandLineArgumentProperty? property) HandleArgument(string arg);
    bool ValidateEnd();

    string? GetArgumentHelp();
    IEnumerable<string> GetHelpLines();
}