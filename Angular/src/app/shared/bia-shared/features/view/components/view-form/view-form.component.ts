import { Component, OnInit, EventEmitter, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { View } from '../../model/view';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-view-form',
  templateUrl: './view-form.component.html',
  styleUrls: ['./view-form.component.scss']
})
export class ViewFormComponent implements OnInit, OnChanges {
  @Input() view: View = <View>{};

  @Output() save = new EventEmitter<View>();

  form: FormGroup;
  isAdd = true;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes.view) {
      this.onViewChange(this.view);
    }
  }

  onViewChange(view: View) {
    this.form.reset();
    this.isAdd = !(view && view.id > 0);
    if (view) {
      this.form.patchValue({ ...view });
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.view.id],
      name: [this.view.name, Validators.required],
      description: [this.view.description]
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const view: View = <View>this.form.value;
      view.id = view.id > 0 ? view.id : 0;
      if (view.id === 0) {
        this.form.reset();
      }
      this.save.emit(view);
    }
  }
}
