using LasseVK.Bootstrapping.ConsoleApplications;
using LasseVK.Pushover;
using LasseVK.RazorTemplates;

using Microsoft.Extensions.Configuration;

using Sandbox.Console.Components;

namespace Sandbox.Console;

public class Application : IConsoleApplication
{
    private readonly IRazorRenderer _razorRenderer;

    public Application(IRazorRenderer razorRenderer)
    {
        _razorRenderer = razorRenderer ?? throw new ArgumentNullException(nameof(razorRenderer));
    }

    public async Task<int> RunAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();

        string html = await _razorRenderer.RenderComponentAsync<RazorPage>(config => config.WithParameter("PageName", "This is a test page").WithParameter("PersonName", "Lasse"));
        System.Console.WriteLine(html);

        return 0;
    }
}