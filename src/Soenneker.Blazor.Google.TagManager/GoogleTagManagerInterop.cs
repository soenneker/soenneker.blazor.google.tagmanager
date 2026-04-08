using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Google.TagManager.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Google.TagManager;

///<inheritdoc cref="IGoogleTagManagerInterop"/>
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
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("pushEvent", linked, eventData);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_modulePath);

        await _cancellationScope.DisposeAsync();
    }
}
