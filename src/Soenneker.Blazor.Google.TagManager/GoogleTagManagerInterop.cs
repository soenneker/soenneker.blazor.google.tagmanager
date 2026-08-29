using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Google.TagManager.Abstract;
using Soenneker.Blazor.Google.TagManager.Models;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Google.TagManager;

/// <inheritdoc cref="IGoogleTagManagerInterop"/>
public sealed class GoogleTagManagerInterop : IGoogleTagManagerInterop
{
    private readonly ILogger<GoogleTagManagerInterop> _logger;
    private readonly IModuleImportUtil _moduleImportUtil;

    private const string _modulePath = "_content/Soenneker.Blazor.Google.TagManager/js/googletagmanagerinterop.js";
    private readonly CancellationScope _cancellationScope = new();

    public GoogleTagManagerInterop(ILogger<GoogleTagManagerInterop> logger, IModuleImportUtil moduleImportUtil)
    {
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;
    }

    public async ValueTask Init(string gtmId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(gtmId))
            throw new ArgumentException("The GTM container ID cannot be null, empty, or whitespace.", nameof(gtmId));

        gtmId = gtmId.Trim();
        _logger.LogDebug("Initializing GoogleTagManager...");

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("init", linked, gtmId);
        }
    }

    public async ValueTask PushEvent(object eventData, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("pushEvent", linked, eventData);
        }
    }

    public async ValueTask SetDefaultConsent(GoogleTagManagerConsentSettings settings, CancellationToken cancellationToken = default)
    {
        ValidateConsent(settings);
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("setDefaultConsent", linked, settings);
        }
    }

    public async ValueTask UpdateConsent(GoogleTagManagerConsentSettings settings, CancellationToken cancellationToken = default)
    {
        ValidateConsent(settings);
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("updateConsent", linked, settings);
        }
    }

    private static void ValidateConsent(GoogleTagManagerConsentSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        if (settings.WaitForUpdateMilliseconds < 0)
            throw new ArgumentOutOfRangeException(nameof(settings), "WaitForUpdateMilliseconds cannot be negative.");
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_modulePath);

        await _cancellationScope.DisposeAsync();
    }
}
