import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { ButtonDirective } from 'primeng/button';
import { FloatLabel } from 'primeng/floatlabel';
import { InputText } from 'primeng/inputtext';
import { View } from '../../model/view';

@Component({
  selector: 'bia-view-form',
  templateUrl: './view-form.component.html',
  styleUrls: ['./view-form.component.scss'],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    InputText,
    ButtonDirective,
    TranslateModule,
    FloatLabel,
  ],
})
export class ViewFormComponent<T extends View> implements OnChanges {
  @Input() view: T | undefined;

  @Output() save = new EventEmitter<T>();

  form: UntypedFormGroup;
  isAdd = true;
  submittingForm = false;

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
      this.submittingForm = true;
      const view: T = <T>this.form.value;
      view.id = view.id > 0 ? view.id : 0;
      if (view.id === 0) {
        this.form.reset();
      }
      this.save.emit(view);
      setTimeout(() => {
        this.submittingForm = false;
      }, 2000);
    }
  }
}
