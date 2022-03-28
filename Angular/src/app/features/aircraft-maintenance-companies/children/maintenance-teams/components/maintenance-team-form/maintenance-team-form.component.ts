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
import { MaintenanceTeam } from '../../model/maintenance-team';

@Component({
  selector: 'app-maintenance-team-form',
  templateUrl: './maintenance-team-form.component.html',
  styleUrls: ['./maintenance-team-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class MaintenanceTeamFormComponent implements OnChanges {
  @Input() maintenanceTeam: MaintenanceTeam = <MaintenanceTeam>{};

  @Output() save = new EventEmitter<MaintenanceTeam>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.maintenanceTeam) {
      this.form.reset();
      if (this.maintenanceTeam) {
        this.form.patchValue({ ...this.maintenanceTeam });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.maintenanceTeam.id],
      title: [this.maintenanceTeam.title, Validators.required],
    });

  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const maintenanceTeam: MaintenanceTeam = <MaintenanceTeam>this.form.value;
      maintenanceTeam.id = maintenanceTeam.id > 0 ? maintenanceTeam.id : 0;

      // force the parent key => siteId from authService or other Id from 'parent'Service
      this.save.emit(maintenanceTeam);
      this.form.reset();
    }
  }
}

