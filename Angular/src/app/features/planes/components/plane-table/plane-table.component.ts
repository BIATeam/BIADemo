import { Component, OnChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { Plane } from '../../model/plane';

@Component({
  selector: 'app-plane-table',
  templateUrl: '../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: ['../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss']
})
export class PlaneTableComponent extends BiaCalcTableComponent implements OnChanges {

  constructor(
    public formBuilder: FormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
    this.initForm();
  }

  public initForm() {
    this.form = this.formBuilder.group({
      id: [this.element.id], // This field is mandatory. Do not remove it.
      msn: [this.element.msn, Validators.required],
      isActive: [this.element.isActive],
      firstFlightDate: [this.element.firstFlightDate, Validators.required],
      firstFlightTime: [this.element.firstFlightTime, Validators.required],
      lastFlightDate: [this.element.lastFlightDate],
      capacity: [this.element.capacity, Validators.required],
      connectingAirports: [this.element.connectingAirports],
      planeType: [this.element.planeType?.id],
    });
  }

    onSubmit() {
    if (this.form.valid) {
      const plane: Plane = <Plane>this.form.value;
      plane.id = plane.id > 0 ? plane.id : 0;
      plane.isActive = plane.isActive ? plane.isActive : false;
      plane.connectingAirports = BiaOptionService.Differential(plane.connectingAirports, this.element?.connectingAirports);
      plane.planeType = BiaOptionService.Clone(plane.planeType);
      this.save.emit(plane);
      this.form.reset();
    }
  }
}
