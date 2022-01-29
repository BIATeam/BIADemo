import { Injectable, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Subscription, Observable } from 'rxjs';
import { filter } from 'rxjs/operators';
import { AppSettings } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import { getAppSettings } from 'src/app/domains/bia-domains/app-settings/store/app-settings.state';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root'
})
export class BiaExternalJsService implements OnDestroy {
  protected sub = new Subscription();

  constructor(
    private store: Store<AppState>
  ) {}

  public init() {
    this.initInjector();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  private initInjector() {
    const appSettings$ = this.getAppSettings();

    this.sub.add(
      appSettings$.subscribe((appSettings) => {
          //       
        if (appSettings && appSettings.environment.urlsAdditionalJS && appSettings.environment.urlsAdditionalJS.length >0) {
          let externalJs = appSettings.environment.urlsAdditionalJS;
          externalJs.forEach(scriptPath => {
            const d2 = document,
            g2 = d2.createElement('script'),
            s2 = d2.getElementsByTagName('script')[1];
            g2.type = 'text/javascript';
            g2.async = true;
            g2.src = scriptPath;
  
            if (s2.parentNode) {
              s2.parentNode.insertBefore(g2, s2);
            }       
          });
        }
      }
    ));
  }
  private getAppSettings(): Observable<AppSettings | null> {
    return this.store.select(getAppSettings).pipe(filter((envConf) => !!envConf));
  }
}
