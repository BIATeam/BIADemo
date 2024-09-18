import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { View } from '../../model/view';

@Component({
  selector: 'bia-view-form',
  templateUrl: './view-form.component.html',
  styleUrls: ['./view-form.component.scss'],
})
export class ViewFormComponent<T extends View> implements OnChanges {
  @Input() view: T | undefined;

  @Output() save = new EventEmitter<T>();

  form: UntypedFormGroup;
  isAdd = true;

  constructor(public formBuilder: UntypedFormBuilder) {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.view) {
      this.onViewChange(this.view);
    }
  }

  onViewChange(view: T | undefined) {
    this.form.reset();
    this.isAdd = !(view && view.id > 0);
    if (view) {
      this.form.patchValue({ ...view });
    }
  }

  protected initForm() {
    this.form = this.formBuilder.group({
      id: [this.view?.id],
      name: [this.view?.name, Validators.required],
      description: [this.view?.description],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const view: T = <T>this.form.value;
      view.id = view.id > 0 ? view.id : 0;
      if (view.id === 0) {
        this.form.reset();
      }
      this.save.emit(view);
    }
  }
}
