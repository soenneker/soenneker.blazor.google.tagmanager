[![](https://img.shields.io/nuget/v/soenneker.blazor.google.tagmanager.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.google.tagmanager/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.google.tagmanager/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.google.tagmanager/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.google.tagmanager.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.google.tagmanager/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.google.tagmanager/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.google.tagmanager/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Google.TagManager
### A Blazor interop library for Google Tag Manager

## Installation

```
dotnet add package Soenneker.Blazor.Google.TagManager
```

## Usage

Register the interop:

```csharp
services.AddGoogleTagManagerInteropAsScoped();
```

Then initialize the GTM container after the page becomes interactive:

```csharp
await GoogleTagManagerInterop.Init("GTM-XXXXXX");
```

Push data-layer events with `PushEvent`:

```csharp
await GoogleTagManagerInterop.PushEvent(new
{
    event = "purchase",
    value = 42.00
});
```

## Consent Mode V2

Queue default consent before initializing GTM:

```csharp
var consent = new GoogleTagManagerConsentSettings
{
    AdStorage = false,
    AnalyticsStorage = false,
    AdUserData = false,
    AdPersonalization = false,
    WaitForUpdateMilliseconds = 500
};

await GoogleTagManagerInterop.SetDefaultConsent(consent);
await GoogleTagManagerInterop.Init("GTM-XXXXXX");
```

After the visitor makes or changes their choice, call `UpdateConsent` with the current values.

The JavaScript-driven initializer cannot provide a no-JavaScript fallback. If your site requires GTM's optional `<noscript>` iframe, include the official iframe markup in the page's static HTML.
