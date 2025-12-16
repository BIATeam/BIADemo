import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  BiaLayoutService,
  CrudItemItemComponent,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { MaintenanceContract } from '../../model/maintenance-contract';
import { MaintenanceContractService } from '../../services/maintenance-contract.service';

@Component({
  selector: 'app-maintenance-contracts-item',
  templateUrl:
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
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
