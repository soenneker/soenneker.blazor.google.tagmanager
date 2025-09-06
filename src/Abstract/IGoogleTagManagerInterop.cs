using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Google.TagManager.Abstract;

/// <summary>
/// A Blazor interop library for Google Tag Manager
/// </summary>
public interface IGoogleTagManagerInterop : IAsyncDisposable
{
    ValueTask Init(string gtmId, CancellationToken cancellationToken = default);

    ValueTask PushEvent(object eventData, CancellationToken cancellationToken = default);
}
