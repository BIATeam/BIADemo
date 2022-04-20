import { Injectable, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription, Observable } from 'rxjs';
import { filter, skip } from 'rxjs/operators';
import { AppSettings } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import { getAppSettings } from 'src/app/domains/bia-domains/app-settings/store/app-settings.state';
import { AppState } from 'src/app/store/state';
import { AuthService } from '../auth.service';
import { MatomoInjector } from './matomo-injector.service';
import { MatomoTracker } from './matomo-tracker.service';

@Injectable({
  providedIn: 'root'
})
export class BiaMatomoService implements OnDestroy {
  protected sub = new Subscription();

  constructor(
    private router: Router,
    private authService: AuthService,
    private matomoInjector: MatomoInjector,
    private matomoTracker: MatomoTracker,
    private store: Store<AppState>
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

  private initMatomoInjector() {
    const appSettings$ = this.getAppSettings();

    this.sub.add(
      appSettings$.subscribe((appSettings) => {
        if (appSettings && appSettings.environment.urlMatomo &&
          appSettings.environment.urlMatomo !== undefined &&
          appSettings.environment.urlMatomo !== '') {
          this.matomoInjector.init(
            appSettings.environment.urlMatomo,
            this.authService.getUncryptedToken().userData.currentTeams.map(a => a.teamId).join(),
            this.authService.getUncryptedToken().userData.currentTeams.map(a => a.teamTitle).join());
        }
      })
    );
  }

  private initMatomoTracker() {
    this.sub.add(
      this.router.events
        .pipe(
          filter((event) => event instanceof NavigationEnd),
          skip(1)
        )
        .subscribe(() => {
          this.matomoTracker.setCustomUrl(window.location.href);
          this.matomoTracker.trackPageView();
        })
    );
  }

  private getAppSettings(): Observable<AppSettings | null> {
    return this.store.select(getAppSettings).pipe(filter((envConf) => !!envConf));
  }
}
