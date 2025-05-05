namespace Sandbox.Blazor.Components.Pages;

public partial class Home
{
    private async Task SetNorwegian()
    {
        await L.SetLanguageAsync("no");
    }

    private async Task SetEnglish()
    {
        await L.SetLanguageAsync("en");
    }
}