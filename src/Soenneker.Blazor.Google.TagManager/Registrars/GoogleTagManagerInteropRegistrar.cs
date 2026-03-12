using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Google.TagManager.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.Google.TagManager.Registrars;

/// <summary>
/// A Blazor interop library for Google Tag Manager
/// </summary>
public static class GoogleTagManagerInteropRegistrar
{
    /// <summary>
    /// Adds <see cref="IGoogleTagManagerInterop"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddGoogleTagManagerInteropAsScoped(this IServiceCollection services)
    {
        services.AddResourceLoaderAsScoped().TryAddScoped<IGoogleTagManagerInterop, GoogleTagManagerInterop>();

        return services;
    }
}
