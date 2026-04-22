using Soenneker.Blazor.Google.TagManager.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Google.TagManager.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class GoogleTagManagerInteropTests : HostedUnitTest
{
    private readonly IGoogleTagManagerInterop _blazorlibrary;

    public GoogleTagManagerInteropTests(Host host) : base(host)
    {
        _blazorlibrary = Resolve<IGoogleTagManagerInterop>(true);
    }

    [Test]
    public void Default()
    {

    }
}
