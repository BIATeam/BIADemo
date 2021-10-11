import { Injectable, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription, Observable } from 'rxjs';
import { filter, skip } from 'rxjs/operators';
import { EnvironmentConfiguration } from 'src/app/domains/environment-configuration/model/environment-configuration';
import { getEnvironmentConfiguration } from 'src/app/domains/environment-configuration/store/environment-configuration.state';
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
    const environmentConfiguration$ = this.getEnvironmentConfiguration();

    this.sub.add(
      environmentConfiguration$.subscribe((environmentConfiguration) => {
        if (environmentConfiguration && environmentConfiguration.urlMatomo && environmentConfiguration.urlMatomo != undefined) {
          this.matomoInjector.init(
            environmentConfiguration.urlMatomo, 
            this.authService.getCurrentSiteId().toString(), 
            this.authService.getAdditionalInfos().userData.currentSiteTitle);
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

  private getEnvironmentConfiguration(): Observable<EnvironmentConfiguration | null> {
    return this.store.select(getEnvironmentConfiguration).pipe(filter((envConf) => !!envConf));
  }
}
