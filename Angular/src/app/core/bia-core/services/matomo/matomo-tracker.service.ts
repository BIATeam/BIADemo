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
 * Wrapper for functions available in the Matomo Javascript tracker.
 *
 * @export
 */
@Injectable({
  providedIn: 'root'
})
export class MatomoTracker {
  /**
   * Creates an instance of MatomoTracker.
   */
  constructor() {
    if (typeof window._paq === 'undefined') {
      console.warn('Matomo has not yet been initialized! (Did you forget to inject it?)');
    }
  }

  /**
   * Logs a visit to this page.
   *
   */
  trackPageView(): void {
    window._paq.push(['trackPageView']);
  }

  /**
   * Overrides document.title
   *
   * @param title Title of the document.
   */
  setDocumentTitle(title: string): void {
    console.log('setDocumentTitle ' + title);
    window._paq.push(['setDocumentTitle', title]);
  }

  /**
   * Override the page's reported URL.
   *
   * @param url URL to be reported for the page.
   */
  setCustomUrl(url: string): void {
    console.log('setCustomUrl ' + url);
    window._paq.push(['setCustomUrl', url]);
  }
}
