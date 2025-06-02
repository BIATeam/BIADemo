import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { first, Subscription } from 'rxjs';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { CrudItemService } from '../../services/crud-item.service';

@Component({
  selector: 'bia-cruditem-item',
  templateUrl: './crud-item-item.component.html',
  styleUrls: ['./crud-item-item.component.scss'],
  imports: [RouterOutlet, NgIf, SpinnerComponent, AsyncPipe],
})
export class CrudItemItemComponent<
    ListCrudItem extends BaseDto,
    CrudItem extends BaseDto = ListCrudItem,
  >
  implements OnInit, OnDestroy
{
  protected sub = new Subscription();
  protected route: ActivatedRoute;
  protected layoutService: BiaLayoutService;
  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemService<ListCrudItem, CrudItem>
  ) {
    this.route = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.layoutService = this.injector.get<BiaLayoutService>(BiaLayoutService);
  }

  ngOnInit() {
    this.crudItemService.currentCrudItemId =
      this.route.snapshot.params.crudItemId;

    this.sub.add(
      this.crudItemService.displayItemName$.subscribe(displayItemName => {
        if (displayItemName) {
          this.route.data.pipe(first()).subscribe(routeData => {
            (routeData as any)['breadcrumb'] = displayItemName;
          });
          this.layoutService.refreshBreadcrumb();
        }
      })
    );

    this.route.params.subscribe(routeParams => {
      this.crudItemService.currentCrudItemId = routeParams.crudItemId;
    });
  }

  ngOnDestroy() {
    this.crudItemService.clearCurrent();
  }
}
