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
    /// <param name="gtmId">Identifier of the gtm to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the google tag manager is ready for use.</returns>
    ValueTask Init(string gtmId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Queues default Consent Mode V2 settings. Call before <see cref="Init"/>.
    /// </summary>
    /// <param name="settings">Settings to apply.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the default consent has been stored.</returns>
    ValueTask SetDefaultConsent(GoogleTagManagerConsentSettings settings, CancellationToken cancellationToken = default);

    /// <summary>
    /// Queues a Consent Mode V2 update after the visitor changes their consent choice.
    /// </summary>
    /// <param name="settings">Settings to apply.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the consent update is complete.</returns>
    ValueTask UpdateConsent(GoogleTagManagerConsentSettings settings, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pushes event.
    /// </summary>
    /// <param name="eventData">Event Data for the push event operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the push event operation is complete.</returns>
    ValueTask PushEvent(object eventData, CancellationToken cancellationToken = default);
}
