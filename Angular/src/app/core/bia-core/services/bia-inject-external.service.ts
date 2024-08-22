import { Injectable, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Subscription, Observable } from 'rxjs';
import { filter } from 'rxjs/operators';
import { AppSettings } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import { getAppSettings } from 'src/app/domains/bia-domains/app-settings/store/app-settings.state';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class BiaInjectExternalService implements OnDestroy {
  protected sub = new Subscription();

  constructor(protected store: Store<AppState>) {}

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
