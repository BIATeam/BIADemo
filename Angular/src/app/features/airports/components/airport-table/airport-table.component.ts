import { Component, OnChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { Airport } from '../../model/airport';

@Component({
  selector: 'app-airport-table',
  templateUrl: '../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: ['../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss']
})
export class AirportTableComponent extends BiaCalcTableComponent implements OnChanges {

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
      name: [this.element.name, Validators.required],
      city: [this.element.city, Validators.required],
    });
  }

    onSubmit() {
    if (this.form.valid) {
      const airport: Airport = <Airport>this.form.value;
      airport.id = airport.id > 0 ? airport.id : 0;
      this.save.emit(airport);
      this.form.reset();
    }
  }
}
