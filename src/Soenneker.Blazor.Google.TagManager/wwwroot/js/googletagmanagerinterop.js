function getDataLayer() {
    window.dataLayer = window.dataLayer || [];
    return window.dataLayer;
}

function toConsentState(settings, includeWaitForUpdate) {
    const state = {
        ad_storage: settings.adStorage ? "granted" : "denied",
        analytics_storage: settings.analyticsStorage ? "granted" : "denied",
        ad_user_data: settings.adUserData ? "granted" : "denied",
        ad_personalization: settings.adPersonalization ? "granted" : "denied"
    };

    if (includeWaitForUpdate && Number.isInteger(settings.waitForUpdateMilliseconds) && settings.waitForUpdateMilliseconds > 0) {
        state.wait_for_update = settings.waitForUpdateMilliseconds;
    }

    return state;
}

function pushConsent(command, settings, includeWaitForUpdate) {
    function gtag() {
        getDataLayer().push(arguments);
    }

    gtag("consent", command, toConsentState(settings, includeWaitForUpdate));
}

export function init(gtmId) {
    const alreadyLoaded = Array.from(document.scripts).some(script => {
        if (!script.src) {
            return false;
        }

        const url = new URL(script.src, document.baseURI);
        return url.hostname === "www.googletagmanager.com" && url.pathname === "/gtm.js" && url.searchParams.get("id") === gtmId;
    });

    if (alreadyLoaded) {
        return;
    }

    getDataLayer().push({
        "gtm.start": new Date().getTime(),
        event: "gtm.js"
    });

    const script = document.createElement("script");
    script.async = true;
    script.src = "https://www.googletagmanager.com/gtm.js?id=" + encodeURIComponent(gtmId);

    const firstScript = document.getElementsByTagName("script")[0];
    if (firstScript?.parentNode) {
        firstScript.parentNode.insertBefore(script, firstScript);
    } else {
        document.head.appendChild(script);
    }
}

export function setDefaultConsent(settings) {
    pushConsent("default", settings, true);
}

export function updateConsent(settings) {
    pushConsent("update", settings, false);
}

export function pushEvent(eventData) {
    getDataLayer().push(eventData);
}
