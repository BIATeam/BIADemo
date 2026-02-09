import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import {
  BiaLayoutService,
  CrudItemItemComponent,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { first } from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { MaintenanceContract } from '../../model/maintenance-contract';
import { MaintenanceContractService } from '../../services/maintenance-contract.service';

@Component({
  selector: 'app-maintenance-contracts-item',
  templateUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
})
export class MaintenanceContractItemComponent
  extends CrudItemItemComponent<MaintenanceContract>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public maintenanceContractService: MaintenanceContractService,
    protected layoutService: BiaLayoutService
  ) {
    super(injector, maintenanceContractService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add(
      this.maintenanceContractService.displayItemName$.subscribe(
        displayItemName => {
          if (displayItemName) {
            this.route.data.pipe(first()).subscribe(routeData => {
              (routeData as any)['breadcrumb'] = displayItemName;
            });
            this.layoutService.refreshBreadcrumb();
          }
        }
      )
    );
  }
}
