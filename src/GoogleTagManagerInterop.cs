using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Google.TagManager.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Blazor.Google.TagManager;

///<inheritdoc cref="IGoogleTagManagerInterop"/>
public class GoogleTagManagerInterop : IGoogleTagManagerInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<GoogleTagManagerInterop> _logger;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncSingleton _scriptInitializer;

    private const string _modulePath = "Soenneker.Blazor.Google.TagManager/js/googletagmanagerinterop.js";
    private const string _moduleName = "GoogleTagManagerInterop";

    public GoogleTagManagerInterop(IJSRuntime jSRuntime, ILogger<GoogleTagManagerInterop> logger, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton(async (token, _) =>
        {
            await _resourceLoader.ImportModuleAndWaitUntilAvailable(_modulePath, _moduleName, 100, token).NoSync();
            return new object();
        });
    }

    public async ValueTask Init(string gtmId, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing GoogleTagManager...");

        await _scriptInitializer.Init(cancellationToken).NoSync();

        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.init", cancellationToken, gtmId).NoSync();
    }

    public async ValueTask PushEvent(object eventData, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken).NoSync();
        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.pushEvent", cancellationToken, eventData).NoSync();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _resourceLoader.DisposeModule(_modulePath).NoSync();

        await _scriptInitializer.DisposeAsync().NoSync();
    }
}