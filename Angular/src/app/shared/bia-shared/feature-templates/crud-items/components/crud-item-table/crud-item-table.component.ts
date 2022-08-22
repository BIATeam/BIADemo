import { Component, OnChanges } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

@Component({
  selector: 'app-crud-item-table',
  templateUrl: '../../../../components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: ['../../../../components/table/bia-calc-table/bia-calc-table.component.scss']
})
export class CrudItemTableComponent<CrudItem extends BaseDto> extends BiaCalcTableComponent implements OnChanges {

  constructor(
    public formBuilder: FormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }

  public initForm() {
    // TODO redefine in plane 
    /*
    this.form = this.formBuilder.group({
      id: [this.element.id], // This field is mandatory. Do not remove it.
      msn: [this.element.msn, Validators.required],
      isActive: [this.element.isActive],
      lastFlightDate: [this.element.lastFlightDate],
      deliveryDate: [this.element.deliveryDate],
      syncTime: [this.element.syncTime],
      capacity: [this.element.capacity, Validators.required],
      connectingAirports: [this.element.connectingAirports],
      crudItemType: [this.element.crudItemType?.id],
    });*/
  }

    onSubmit() {
    if (this.form.valid) {
      const crudItem: CrudItem = <CrudItem>this.form.value;
      crudItem.id = crudItem.id > 0 ? crudItem.id : 0;
      // TODO redefine in plane 
      /*
      crudItem.isActive = crudItem.isActive ? crudItem.isActive : false;
      crudItem.connectingAirports = BiaOptionService.Differential(crudItem.connectingAirports, this.element?.connectingAirports);
      crudItem.crudItemType = BiaOptionService.Clone(crudItem.crudItemType);

      // force the parent key => siteId from authService or other Id from 'parent'Service
      crudItem.siteId = this.authService.getCurrentTeamId(TeamTypeId.Site),*/
      this.save.emit(crudItem);
      this.form.reset();
    }
  }
}
