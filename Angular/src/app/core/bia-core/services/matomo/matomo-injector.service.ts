import { Injectable } from '@angular/core';

/**
 * Access to the global window variable.
 */
declare var window: {
  [key: string]: any;
  prototype: Window;
  new (): Window;
};

/**
 * Service for injecting the Matomo tracker in the application.
 *
 * @export
 */
@Injectable({
  providedIn: 'root'
})
export class MatomoInjector {
  /**
   * Creates an instance of MatomoInjector.
   */
  constructor() {
    window._paq = window._paq || [];
  }

  /**
   * Injects the Matomo tracker in the DOM.
   */
  init(url: string, siteId: string, siteName?: string) {
    console.log('MatomoInjector siteName:' + siteName);
    window._paq.push(["setRequestMethod", "POST"]);
    window._paq.push(['trackPageView']);
    window._paq.push(['enableLinkTracking']);
    (() => {
      window._paq.push(['setTrackerUrl', url + '.php']);
      window._paq.push(['setSiteId', siteId]);
      window._paq.push(['setCustomDimension', 1, siteName]);
      const d = document,
        g = d.createElement('script'),
        s = d.getElementsByTagName('script')[0];
      g.type = 'text/javascript';
      g.async = true;
      g.src = url + '.js';
      //g.src = './assets/bia/matomo/matomo.js'; blocked by uBlock
      if (s.parentNode) {
        s.parentNode.insertBefore(g, s);
      }
    })();
  }
}
