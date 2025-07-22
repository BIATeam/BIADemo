import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import {
  BiaLayoutService,
  CrudItemItemComponent,
  SpinnerComponent,
} from 'biang/shared';
import { first } from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { MaintenanceContract } from '../../model/maintenance-contract';
import { MaintenanceContractService } from '../../services/maintenance-contract.service';

@Component({
  selector: 'app-maintenance-contracts-item',
  templateUrl:
    '../../../../../../node_modules/biang/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../node_modules/biang/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, AsyncPipe, SpinnerComponent],
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
