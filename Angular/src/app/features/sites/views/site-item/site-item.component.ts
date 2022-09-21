import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Site } from '../../model/site';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { SiteService } from '../../services/site.service';

@Component({
  templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: ['../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss']
})
export class SiteItemComponent extends CrudItemItemComponent<Site> implements OnInit {
  constructor(protected store: Store<AppState>,
    protected injector: Injector,
    public siteService: SiteService,
    protected layoutService: BiaClassicLayoutService,
  ) {
    super(injector, siteService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add
      (
        this.siteService.crudItem$.subscribe((site) => {
          if (site?.title) {
            this.route.data.pipe(first()).subscribe(routeData => {
              (routeData as any)['breadcrumb'] = site.title;
            });
            this.layoutService.refreshBreadcrumb();
          }
        })
      );
  }
}
