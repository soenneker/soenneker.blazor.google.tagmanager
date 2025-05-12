export class GoogleTagManagerInterop {
    init(gtmId) {
        if (window.dataLayer && window.dataLayer.find(e => e.event === "gtm.js")) {
            return;
        }

        const inlineScript = document.createElement('script');
        inlineScript.innerHTML = `
            (function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
            new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
            j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;
            j.src='https://www.googletagmanager.com/gtm.js?id='+i+dl;
            f.parentNode.insertBefore(j,f);
            })(window,document,'script','dataLayer','${gtmId}');
        `;
        document.head.appendChild(inlineScript);

        const noscript = document.createElement('noscript');
        noscript.innerHTML = `
            <iframe src="https://www.googletagmanager.com/ns.html?id=${gtmId}"
            height="0" width="0" style="display:none;visibility:hidden"></iframe>
        `;
        document.body.prepend(noscript);
    }

    pushEvent(eventData) {
        window.dataLayer = window.dataLayer || [];
        window.dataLayer.push(eventData);
    }
}

window.GoogleTagManagerInterop = new GoogleTagManagerInterop();