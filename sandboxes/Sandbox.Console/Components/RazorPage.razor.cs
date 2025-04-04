using Microsoft.AspNetCore.Components;

namespace Sandbox.Console.Components;

public partial class RazorPage : ComponentBase
{
    [Parameter]
    public string PageName { get; set; } = "";

    [Parameter]
    public string PersonName { get; set; } = "";
}