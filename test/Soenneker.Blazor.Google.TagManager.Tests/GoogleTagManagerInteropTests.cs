using System.Threading;
using Microsoft.JSInterop;
using Soenneker.Blazor.Google.TagManager.Abstract;
using Soenneker.Blazor.Google.TagManager.Models;
using Soenneker.Blazor.MockJsRuntime.Abstract;
using Soenneker.Tests.HostedUnit;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Google.TagManager.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class GoogleTagManagerInteropTests : HostedUnitTest
{
    private readonly IGoogleTagManagerInterop _blazorlibrary;

    public GoogleTagManagerInteropTests(Host host) : base(host)
    {
        var jsRuntime = (IMockJsRuntime) Resolve<IJSRuntime>(true);
        jsRuntime.SetupMockResult<IJSObjectReference>("import", new TestJsObjectReference());
        _blazorlibrary = Resolve<IGoogleTagManagerInterop>(true);
    }

    [Test]
    public async Task Consent_mode_v2_can_be_invoked(CancellationToken cancellationToken)
    {
        var settings = new GoogleTagManagerConsentSettings
        {
            AdStorage = false,
            AnalyticsStorage = false,
            AdUserData = false,
            AdPersonalization = false,
            WaitForUpdateMilliseconds = 500
        };

        await _blazorlibrary.SetDefaultConsent(settings, cancellationToken: cancellationToken);
        await _blazorlibrary.UpdateConsent(settings, cancellationToken: cancellationToken);
    }
}
