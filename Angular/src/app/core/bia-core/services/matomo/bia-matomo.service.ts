import { Injectable, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription, combineLatest, Observable } from 'rxjs';
import { filter, first, skip } from 'rxjs/operators';
import { EnvironmentConfiguration } from 'src/app/domains/environment-configuration/model/environment-configuration';
import { getEnvironmentConfiguration } from 'src/app/domains/environment-configuration/store/environment-configuration.state';
import { Site } from 'src/app/domains/site/model/site';
import { getAllSites } from 'src/app/domains/site/store/site.state';
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
    const sites$ = this.getSites();
    const environmentConfiguration$ = this.getEnvironmentConfiguration();

    this.sub.add(
      combineLatest([sites$, environmentConfiguration$]).subscribe(([sites, environmentConfiguration]) => {
        const currentsite: Site = sites.filter((x) => x.id === this.authService.getCurrentSiteId())[0];
        if (environmentConfiguration) {
          this.matomoInjector.init(environmentConfiguration.urlMatomo, '1', currentsite ?  currentsite.title : '');
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

  private getSites(): Observable<Site[]> {
    return this.store.select(getAllSites).pipe(
      filter((sites: Site[]) => sites && sites.length > 0),
      first()
    );
  }
  private getEnvironmentConfiguration(): Observable<EnvironmentConfiguration | null> {
    return this.store.select(getEnvironmentConfiguration).pipe(filter((envConf) => !!envConf));
  }
}
