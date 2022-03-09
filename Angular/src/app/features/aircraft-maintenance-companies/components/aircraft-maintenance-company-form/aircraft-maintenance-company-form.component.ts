import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';

@Component({
  selector: 'app-aircraft-maintenance-company-form',
  templateUrl: './aircraft-maintenance-company-form.component.html',
  styleUrls: ['./aircraft-maintenance-company-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class AircraftMaintenanceCompanyFormComponent implements OnChanges {
  @Input() aircraftMaintenanceCompany: AircraftMaintenanceCompany = <AircraftMaintenanceCompany>{};

  @Output() save = new EventEmitter<AircraftMaintenanceCompany>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.aircraftMaintenanceCompany) {
      this.form.reset();
      if (this.aircraftMaintenanceCompany) {
        this.form.patchValue({ ...this.aircraftMaintenanceCompany });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.aircraftMaintenanceCompany.id],
      title: [this.aircraftMaintenanceCompany.title, Validators.required],
    });

  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const aircraftMaintenanceCompany: AircraftMaintenanceCompany = <AircraftMaintenanceCompany>this.form.value;
      aircraftMaintenanceCompany.id = aircraftMaintenanceCompany.id > 0 ? aircraftMaintenanceCompany.id : 0;

      // force the parent key => siteId from authService or other Id from 'parent'Service
      this.save.emit(aircraftMaintenanceCompany);
      this.form.reset();
    }
  }
}

