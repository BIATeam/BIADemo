import { Injectable, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { filter, skip } from 'rxjs/operators';
import { AppSettings } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import { getAppSettings } from 'src/app/domains/bia-domains/app-settings/store/app-settings.state';
import { AppState } from 'src/app/store/state';
import { AuthService } from '../auth.service';
import { MatomoInjector } from './matomo-injector.service';
import { MatomoTracker } from './matomo-tracker.service';

@Injectable({
  providedIn: 'root',
})
export class BiaMatomoService implements OnDestroy {
  protected sub = new Subscription();

  constructor(
    protected router: Router,
    protected authService: AuthService,
    protected matomoInjector: MatomoInjector,
    protected matomoTracker: MatomoTracker,
    protected store: Store<AppState>
  ) {}

  public init() {
    this.initMatomoInjector();
    this.initMatomoTracker();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  protected initMatomoInjector() {
    const appSettings$ = this.getAppSettings();

    this.sub.add(
      appSettings$.subscribe(appSettings => {
        if (
          appSettings &&
          appSettings.environment.urlMatomo &&
          appSettings.environment.urlMatomo !== undefined &&
          appSettings.environment.urlMatomo !== ''
        ) {
          this.matomoInjector.init(
            appSettings.environment.urlMatomo,
            appSettings.environment.siteIdMatomo,
            this.authService
              .getDecryptedToken()
              .userData.currentTeams.map(a => a.teamTitle)
              .join()
          );
        }
      })
    );
  }

  protected initMatomoTracker() {
    this.sub.add(
      this.router.events
        .pipe(
          filter(event => event instanceof NavigationEnd),
          skip(1)
        )
        .subscribe(() => {
          this.matomoTracker.setCustomUrl(window.location.href);
          this.matomoTracker.trackPageView();
        })
    );
  }

  protected getAppSettings(): Observable<AppSettings | null> {
    return this.store.select(getAppSettings).pipe(filter(envConf => !!envConf));
  }
}
