import {
  AsyncPipe,
  NgClass,
  NgFor,
  NgIf,
  NgStyle,
  NgSwitch,
  NgTemplateOutlet,
} from '@angular/common';
import { Component, OnChanges } from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  Validators,
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { Skeleton } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { BiaFrozenColumnDirective } from 'src/app/shared/bia-shared/components/table/bia-frozen-column/bia-frozen-column.directive';
import { BiaTableFilterComponent } from 'src/app/shared/bia-shared/components/table/bia-table-filter/bia-table-filter.component';
import { BiaTableFooterControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-footer-controller/bia-table-footer-controller.component';
import { BiaTableInputComponent } from 'src/app/shared/bia-shared/components/table/bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from 'src/app/shared/bia-shared/components/table/bia-table-output/bia-table-output.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { Plane } from '../../model/plane';

@Component({
  selector: 'app-plane-table',
  templateUrl:
    '../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss',
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    TableModule,
    PrimeTemplate,
    NgFor,
    Tooltip,
    NgSwitch,

    NgClass,
    NgTemplateOutlet,
    Skeleton,
    NgStyle,
    AsyncPipe,
    TranslateModule,
    BiaTableFilterComponent,
    BiaTableInputComponent,
    BiaTableOutputComponent,
    BiaTableFooterControllerComponent,
    BiaFrozenColumnDirective,
  ],
})
export class PlaneTableComponent
  extends BiaCalcTableComponent<Plane>
  implements OnChanges
{
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }

  public initForm() {
    this.form = this.formBuilder.group({
      id: [this.element.id], // This field is mandatory. Do not remove it.
      msn: [this.element.msn, Validators.required],
      isActive: [this.element.isActive],
      lastFlightDate: [this.element.lastFlightDate],
      deliveryDate: [this.element.deliveryDate],
      syncTime: [this.element.syncTime],
      capacity: [
        this.element.capacity,
        [Validators.required, Validators.min(1)],
      ],
      connectingAirports: [this.element.connectingAirports],
      planeType: [this.element.planeType?.id],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const plane: Plane = <Plane>this.form.value;
      plane.id = plane.id > 0 ? plane.id : 0;
      plane.isActive = plane.isActive ? plane.isActive : false;
      plane.connectingAirports = BiaOptionService.differential(
        plane.connectingAirports,
        this.element?.connectingAirports
      );
      plane.planeType = BiaOptionService.clone(plane.planeType);

      // force the parent key => siteId from authService or other Id from 'parent'Service
      plane.siteId = this.authService.getCurrentTeamId(TeamTypeId.Site);
      this.save.emit(plane);
      this.form.reset();
    }
  }
}
