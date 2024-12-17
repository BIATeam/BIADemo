import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudItemService } from '../../services/crud-item.service';

@Component({
  selector: 'bia-cruditem-item',
  templateUrl: './crud-item-item.component.html',
  styleUrls: ['./crud-item-item.component.scss'],
})
export class CrudItemItemComponent<CrudItem extends BaseDto>
  implements OnInit, OnDestroy
{
  protected sub = new Subscription();
  protected route: ActivatedRoute;
  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemService<CrudItem>
  ) {
    this.route = this.injector.get<ActivatedRoute>(ActivatedRoute);
  }

  ngOnInit() {
    this.crudItemService.currentCrudItemId =
      this.route.snapshot.params.crudItemId;
    // TODO redefine in plane
    /*
    this.sub.add
      (
        this.store.select(getCurrentCrudItem).subscribe((crudItem) => {
          if (crudItem?.msn) {
            this.route.data.pipe(first()).subscribe(routeData => {
              (routeData as any)['breadcrumb'] = crudItem.msn;
            });
            this.layoutService.refreshBreadcrumb();
          }
        })
      );*/
  }

  ngOnDestroy() {
    this.crudItemService.clearCurrent();
  }
}
