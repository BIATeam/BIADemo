import { Injectable, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppSettings } from 'packages/bia-ng/models/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { Observable, Subscription } from 'rxjs';
import { filter, skip } from 'rxjs/operators';
import { getAppSettings } from '../../app-settings/store/app-settings.state';
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
    protected store: Store<BiaAppState>
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
