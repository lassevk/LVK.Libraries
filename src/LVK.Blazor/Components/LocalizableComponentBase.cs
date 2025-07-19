using Microsoft.AspNetCore.Components;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace LVK.Blazor.Components;

public abstract class LocalizableComponentBase : ComponentBase
{
    [Inject] private ILocalizationServiceProvider LocalizationServiceProvider { get; set; }

    protected ILocalizationService L { get; private set; } = null!;

    protected override void OnInitialized()
    {
        L = LocalizationServiceProvider.GetService(GetType());
    }
}