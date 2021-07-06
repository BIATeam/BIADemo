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
import { PlaneType } from '../../model/plane-type';

@Component({
  selector: 'app-plane-type-form',
  templateUrl: './plane-type-form.component.html',
  styleUrls: ['./plane-type-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PlaneTypeFormComponent implements OnInit, OnChanges {
  @Input() planeType: PlaneType = <PlaneType>{};

  @Output() save = new EventEmitter<PlaneType>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes.planeType) {
      this.form.reset();
      if (this.planeType) {
        this.form.patchValue({ ...this.planeType });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.planeType.id],
      title: [this.planeType.title, Validators.required],
      certificationDate: [this.planeType.certificationDate, Validators.required],
    });
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const planeType: PlaneType = <PlaneType>this.form.value;
      planeType.id = planeType.id > 0 ? planeType.id : 0;
      this.save.emit(planeType);
      this.form.reset();
    }
  }
}
