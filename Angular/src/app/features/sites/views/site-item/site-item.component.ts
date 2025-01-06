import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { Site } from '../../model/site';
import { SiteService } from '../../services/site.service';

@Component({
  selector: 'app-sites-item',
  templateUrl:
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
})
export class SiteItemComponent
  extends CrudItemItemComponent<Site>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public siteService: SiteService,
    protected layoutService: BiaLayoutService
  ) {
    super(injector, siteService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add(
      this.siteService.displayItemName$.subscribe(displayItemName => {
        if (displayItemName) {
          this.route.data.pipe(first()).subscribe(routeData => {
            (routeData as any)['breadcrumb'] = displayItemName;
          });
          this.layoutService.refreshBreadcrumb();
        }
      })
    );
  }
}
