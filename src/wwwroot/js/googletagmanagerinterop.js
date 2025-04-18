export class GoogleTagManagerInterop {
        init(gtmId) {
        if (window.dataLayer && window.dataLayer.find(e => e.event === "gtm.js")) {
            return;
        }

        const scriptTag = document.createElement('script');
        scriptTag.textContent = `
            (function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
            new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
            j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;
            j.src='https://www.googletagmanager.com/gtm.js?id='+i+dl;
            f.parentNode.insertBefore(j,f);
            })(window,document,'script','dataLayer','${gtmId}');
            `;
        document.head.appendChild(scriptTag);

        const iframe = document.createElement('iframe');
        iframe.src = `https://www.googletagmanager.com/ns.html?id=${gtmId}`;
        iframe.style.display = "none";
        iframe.style.visibility = "hidden";
        iframe.width = "0";
        iframe.height = "0";

        document.body.prepend(iframe);
    }

    pushEvent(eventData) {
        window.dataLayer = window.dataLayer || [];
        window.dataLayer.push(eventData);
    }
}

window.GoogleTagManagerInterop = new GoogleTagManagerInterop();