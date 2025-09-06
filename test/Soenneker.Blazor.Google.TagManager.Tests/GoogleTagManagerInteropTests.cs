using Soenneker.Blazor.Google.TagManager.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Blazor.Google.TagManager.Tests;

[Collection("Collection")]
public class GoogleTagManagerInteropTests : FixturedUnitTest
{
    private readonly IGoogleTagManagerInterop _blazorlibrary;

    public GoogleTagManagerInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _blazorlibrary = Resolve<IGoogleTagManagerInterop>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
