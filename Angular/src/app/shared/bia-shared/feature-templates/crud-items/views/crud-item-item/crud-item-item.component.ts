import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudItemFacadeService } from '../../services/crud-item-facade.service';

@Component({
  templateUrl: './crud-item-item.component.html',
  styleUrls: ['./crud-item-item.component.scss']
})
export class CrudItemItemComponent<CrudItem extends BaseDto> implements OnInit, OnDestroy {
  protected sub = new Subscription();

  constructor(
    //protected store: Store<AppState>,
    protected route: ActivatedRoute,
    public facadeService: CrudItemFacadeService<CrudItem>,
  ) { }

  ngOnInit() {
    this.facadeService.currentCrudItemId = this.route.snapshot.params.crudItemId;
    // TODO redefine in plane
    /*
    this.sub.add
      (
        this.store.select(getCurrentCrudItem).subscribe((crudItem) => {
          if (crudItem?.msn) {
            this.route.data.pipe(first()).subscribe(routeData => {
              routeData['breadcrumb'] = crudItem.msn;
            });
            this.layoutService.refreshBreadcrumb();
          }
        })
      );*/
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}
