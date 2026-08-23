using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Google.TagManager.Models;

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
    /// Queues default Consent Mode V2 settings. Call before <see cref="Init"/>.
    /// </summary>
    /// <param name="settings">The default consent settings.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    ValueTask SetDefaultConsent(GoogleTagManagerConsentSettings settings, CancellationToken cancellationToken = default);

    /// <summary>
    /// Queues a Consent Mode V2 update after the visitor changes their consent choice.
    /// </summary>
    /// <param name="settings">The updated consent settings.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    ValueTask UpdateConsent(GoogleTagManagerConsentSettings settings, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the push event operation.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask PushEvent(object eventData, CancellationToken cancellationToken = default);
}
