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
import { Site } from '../../model/site/site';

@Component({
  selector: 'app-site-form',
  templateUrl: './site-form.component.html',
  styleUrls: ['./site-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SiteFormComponent implements OnInit, OnChanges {
  @Input() site: Site = <Site>{};

  @Output() save = new EventEmitter<Site>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes.site) {
      this.form.reset();
      if (this.site) {
        this.form.patchValue({ ...this.site });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.site.id],
      title: [this.site.title, Validators.required]
    });
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const site: Site = <Site>this.form.value;
      site.id = site.id > 0 ? site.id : 0;
      this.save.emit(site);
      this.form.reset();
    }
  }
}
