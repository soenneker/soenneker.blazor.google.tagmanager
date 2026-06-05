using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Google.TagManager.Abstract;

/// <summary>
/// A Blazor interop library for Google Tag Manager
/// </summary>
public interface IGoogleTagManagerInterop : IAsyncDisposable
{
    /// <summary>
    /// Initializes the instance.
    /// </summary>
    /// <param name="gtmId">The gtm id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask Init(string gtmId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the push event operation.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask PushEvent(object eventData, CancellationToken cancellationToken = default);
}
