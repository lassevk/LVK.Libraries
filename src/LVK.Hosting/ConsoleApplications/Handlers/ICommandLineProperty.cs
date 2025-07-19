namespace LVK.Hosting.ConsoleApplications.Handlers;

internal interface ICommandLineProperty
{
    string Name { get; }

    ArgumentHandlerAcceptResponse Accept(string argument);
    ArgumentHandlerFinishResponse Finish();

    IEnumerable<string> GetHelpText();
}