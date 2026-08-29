[![](https://img.shields.io/nuget/v/soenneker.blazor.google.tagmanager.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.google.tagmanager/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.google.tagmanager/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.google.tagmanager/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.google.tagmanager.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.google.tagmanager/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.google.tagmanager/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.google.tagmanager/actions/workflows/codeql.yml)

# Soenneker.Blazor.Google.TagManager

A Blazor interop service that loads a Google Tag Manager container and pushes .NET objects to its `dataLayer`.

## Installation

```bash
dotnet add package Soenneker.Blazor.Google.TagManager
```

```csharp
using Soenneker.Blazor.Google.TagManager.Registrars;

builder.Services.AddGoogleTagManagerInteropAsScoped();
```

## Initialize the container

Initialize after the page becomes interactive:

```razor
@using Soenneker.Blazor.Google.TagManager.Abstract
@inject IGoogleTagManagerInterop TagManager

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await TagManager.Init("GTM-XXXXXX");
    }
}
```

Repeated initialization with the same container ID does not add another matching GTM script.

## Push an event

```csharp
await TagManager.PushEvent(new
{
    event = "purchase",
    ecommerce = new
    {
        transaction_id = order.Id,
        currency = "USD",
        value = order.Total
    }
});
```

The object is appended as-is to `window.dataLayer`. Its `event` property must match a custom-event trigger configured in the GTM container; pushing data does not by itself send an analytics event.

For Blazor route tracking, push a page-view-style event from a `NavigationManager.LocationChanged` handler and unsubscribe when the owning component is disposed:

```csharp
await TagManager.PushEvent(new
{
    event = "virtual_page_view",
    page_location = Navigation.Uri
});
```

## Consent Mode V2

Queue denied defaults before loading the container, then update them when the visitor makes a choice:

```csharp
using Soenneker.Blazor.Google.TagManager.Models;

var consent = new GoogleTagManagerConsentSettings
{
    AdStorage = false,
    AnalyticsStorage = false,
    AdUserData = false,
    AdPersonalization = false,
    WaitForUpdateMilliseconds = 500
};

await TagManager.SetDefaultConsent(consent);
await TagManager.Init("GTM-XXXXXX");

// Later, after the visitor's explicit choice:
consent.AnalyticsStorage = true;
await TagManager.UpdateConsent(consent);
```

Queue the default before `Init`; otherwise the container is loaded first. Consent state controls tags only when the GTM container and its tags are configured to honor it.

## Security and privacy

Do not push names, email addresses, access tokens, raw URLs containing secrets, or other personally identifiable/sensitive values into the data layer. Treat container access and publishing permissions as production-code access: a published container can execute scripts in the application origin. Loading GTM also needs the corresponding privacy disclosure and content-security-policy allowances.

The JavaScript initializer cannot provide GTM's optional no-JavaScript fallback. If the site requires the `<noscript>` iframe, include Google's official iframe markup in the page's static HTML.

Use this package when GTM owns tag deployment. If Google Analytics is loaded through the container, do not also initialize a separate direct `gtag.js` integration unless duplicate loading and event behavior are intentional.
