using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Google.TagManager.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Utils.CancellationScopes;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Google.TagManager;

///<inheritdoc cref="IGoogleTagManagerInterop"/>
public sealed class GoogleTagManagerInterop : IGoogleTagManagerInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<GoogleTagManagerInterop> _logger;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncSingleton _scriptInitializer;

    private const string _modulePath = "Soenneker.Blazor.Google.TagManager/js/googletagmanagerinterop.js";
    private const string _moduleName = "GoogleTagManagerInterop";
    private readonly CancellationScope _cancellationScope = new();

    public GoogleTagManagerInterop(IJSRuntime jSRuntime, ILogger<GoogleTagManagerInterop> logger, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton(async (token, _) =>
        {
            await _resourceLoader.ImportModuleAndWaitUntilAvailable(_modulePath, _moduleName, 100, token);
            return new object();
        });
    }

    public async ValueTask Init(string gtmId, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing GoogleTagManager...");

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await _scriptInitializer.Init(linked);

            await _jsRuntime.InvokeVoidAsync($"{_moduleName}.init", linked, gtmId);
        }
    }

    public async ValueTask PushEvent(object eventData, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await _scriptInitializer.Init(linked);
            await _jsRuntime.InvokeVoidAsync($"{_moduleName}.pushEvent", linked, eventData);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_modulePath);

        await _scriptInitializer.DisposeAsync();

        await _cancellationScope.DisposeAsync();
    }
}
