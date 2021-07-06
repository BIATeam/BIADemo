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
import { Airport } from '../../model/airport';

@Component({
  selector: 'app-airport-form',
  templateUrl: './airport-form.component.html',
  styleUrls: ['./airport-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AirportFormComponent implements OnInit, OnChanges {
  @Input() airport: Airport = <Airport>{};

  @Output() save = new EventEmitter<Airport>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes.airport) {
      this.form.reset();
      if (this.airport) {
        this.form.patchValue({ ...this.airport });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.airport.id],
      name: [this.airport.name, Validators.required],
      city: [this.airport.city, Validators.required]
    });
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
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
