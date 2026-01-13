import { Injectable, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppSettings } from 'packages/bia-ng/models/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { Observable, Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { getAppSettings } from '../app-settings/store/app-settings.state';

@Injectable({
  providedIn: 'root',
})
export class BiaInjectExternalService implements OnDestroy {
  protected sub = new Subscription();

  constructor(protected store: Store<BiaAppState>) {}

  public init() {
    this.initInjector();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  protected initInjector() {
    const appSettings$ = this.getAppSettings();

    this.sub.add(
      appSettings$.subscribe(appSettings => {
        //
        if (appSettings) {
          if (
            appSettings.environment.urlsAdditionalJS &&
            appSettings.environment.urlsAdditionalJS.length > 0
          ) {
            const externalJs = appSettings.environment.urlsAdditionalJS;
            externalJs.forEach(scriptPath => {
              const d = document;
              const g = d.createElement('script');
              const s = d.getElementsByTagName('script')[0];
              g.type = 'text/javascript';
              g.async = true;
              g.src = scriptPath;

              if (s.parentNode) {
                s.parentNode.insertBefore(g, s);
              }
            });
          }

          if (
            appSettings.environment.urlsAdditionalCSS &&
            appSettings.environment.urlsAdditionalCSS.length > 0
          ) {
            const externalCss = appSettings.environment.urlsAdditionalCSS;
            externalCss.forEach(stylePath => {
              const d = document;
              const g = d.createElement('link');
              const s = d.getElementsByTagName('link')[0];
              g.rel = 'stylesheet';
              g.type = 'text/css';
              g.href = stylePath;

              if (s.parentNode) {
                s.parentNode.insertBefore(g, s);
              }
            });
          }
        }
      })
    );
  }
  protected getAppSettings(): Observable<AppSettings | null> {
    return this.store.select(getAppSettings).pipe(filter(envConf => !!envConf));
  }
}
