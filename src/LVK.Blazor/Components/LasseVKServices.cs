using System.Globalization;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

// ReSharper disable InconsistentNaming

namespace LVK.Blazor.Components;

public class LasseVKServices : ComponentBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LasseVKServices(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    protected override void OnInitialized()
    {
        string? culture = _httpContextAccessor.HttpContext?.Request.Cookies["blazor-culture"];

        if (string.IsNullOrWhiteSpace(culture))
        {
            string? userLangs = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();
            culture = userLangs?.Split(',').FirstOrDefault();
        }

        if (string.IsNullOrWhiteSpace(culture))
        {
            return;
        }

        var ci = new CultureInfo(culture);
        CultureInfo.DefaultThreadCurrentCulture = ci;
        CultureInfo.DefaultThreadCurrentUICulture = ci;
    }
}