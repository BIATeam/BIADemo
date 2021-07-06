import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { getCurrentSite} from '../../store/site.state';
import { Site } from '../../model/site/site';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute } from '@angular/router';
import { SiteService } from '../../services/site.service';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';

@Component({
  templateUrl: './site-item.component.html',
  styleUrls: ['./site-item.component.scss']
})
export class SiteItemComponent implements OnInit, OnDestroy {
  site$: Observable<Site>;
  private sub = new Subscription();

  constructor(private store: Store<AppState>,
    private route: ActivatedRoute,
    public siteService: SiteService,
    private layoutService: BiaClassicLayoutService) { }

  ngOnInit() {
    this.siteService.currentSiteId = this.route.snapshot.params.siteId;
    this.sub.add
      (
        this.store.select(getCurrentSite).subscribe((site) => {
          if (site?.title) {
            this.route.data.pipe(first()).subscribe(routeData => {
              routeData['breadcrumb'] = site.title;
            });
            this.layoutService.refreshBreadcrumb();
          }
        })
      );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}
