import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { Plane } from '../../model/plane';

@Component({
  selector: 'app-plane-form',
  templateUrl: './plane-form.component.html',
  styleUrls: ['./plane-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class PlaneFormComponent implements OnInit, OnChanges {
  @Input() plane: Plane = <Plane>{};
  @Input() airportOptions: OptionDto[];
  @Input() planeTypeOptions: OptionDto[];

  @Output() save = new EventEmitter<Plane>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.plane) {
      this.form.reset();
      if (this.plane) {
        this.form.patchValue({ ...this.plane });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.plane.id],
      msn: [this.plane.msn, Validators.required],
      isActive: [this.plane.isActive],
      firstFlightDate: [this.plane.firstFlightDate, Validators.required],
      firstFlightTime: [this.plane.firstFlightTime, Validators.required],
      lastFlightDate: [this.plane.lastFlightDate],
      capacity: [this.plane.capacity, Validators.required],
      connectingAirports: [this.plane.connectingAirports],
      planeType: [this.plane.planeType?.id],
    });
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const plane: Plane = <Plane>this.form.value;
      plane.id = plane.id > 0 ? plane.id : 0;
      plane.isActive = plane.isActive ? plane.isActive : false;
      plane.connectingAirports = BiaOptionService.Differential(plane.connectingAirports, this.plane?.connectingAirports);
      plane.planeType = BiaOptionService.Clone(plane.planeType);
      this.save.emit(plane);
      this.form.reset();
    }
  }
}

