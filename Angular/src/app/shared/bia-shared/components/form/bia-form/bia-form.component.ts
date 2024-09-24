import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  TemplateRef,
} from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import {
  BiaFieldConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';

@Component({
  selector: 'bia-form',
  templateUrl: './bia-form.component.html',
  styleUrls: ['./bia-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class BiaFormComponent
  implements OnInit, OnDestroy, OnChanges, AfterContentInit
{
  @Input() element: any = {};
  @Input() fields: BiaFieldConfig[];
  @Input() dictOptionDtos: DictOptionDto[];
  @Output() save = new EventEmitter<any>();
  @Output() cancel = new EventEmitter<void>();

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  specificInputTemplate: TemplateRef<any>;
  specificOutputTemplate: TemplateRef<any>;

  form?: UntypedFormGroup;
  protected sub = new Subscription();

  constructor(
    public formBuilder: UntypedFormBuilder
    // protected authService: AuthService
  ) {}

  ngOnInit() {
    this.element ??= {};
    this.initForm();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
  ngAfterContentInit() {
    this.templates.forEach(item => {
      switch (item.getType()) {
        case 'specificInput':
          this.specificInputTemplate = item.template;
          break;
        case 'specificOutput':
          this.specificOutputTemplate = item.template;
          break;
      }
    });
  }

  ngOnChanges() {
    if (this.element && this.form) {
      this.form.reset();
      if (this.element) {
        this.form.patchValue({ ...this.element });
      }
    }
  }

  public checkObject(obj: any): { element: any; errorMessages: string[] } {
    const errorMessages: string[] = [];
    if (this.form) {
      const form = this.form;
      form.reset();
      if (obj) {
        form.patchValue({ ...obj });
      }

      if (this.form.invalid) {
        Object.keys(form.controls).forEach(controlName => {
          const controlErrors = form.controls[controlName].errors;
          if (controlErrors != null) {
            errorMessages.push(
              `${controlName}: ${JSON.stringify(controlErrors)}`
            );
          }
        });
      }
    }
    const element = this.getElement();
    return { element, errorMessages };
  }

  protected initForm() {
    this.form = this.formBuilder.group(this.formFields());
  }
  protected formFields() {
    const fields: { [key: string]: any } = { id: [this.element?.id] };
    for (const col of this.fields) {
      if (col.validators && col.validators.length > 0) {
        fields[col.field] = [this.element[col.field], col.validators];
      } else if (col.isRequired) {
        fields[col.field] = [this.element[col.field], Validators.required];
      } else {
        fields[col.field] = [this.element[col.field]];
      }
    }
    return fields;
  }

  onCancel() {
    this.form?.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form?.valid) {
      const element: any = this.getElement();
      this.save.emit(element);
      this.form.reset();
    }
  }
  public getElement() {
    const element: any = this.form?.value;
    element.id = element.id > 0 ? element.id : 0;
    for (const col of this.fields) {
      switch (col.type) {
        case PropType.Boolean:
          Reflect.set(
            element,
            col.field,
            element[col.field] ? element[col.field] : false
          );
          break;
        case PropType.ManyToMany:
          Reflect.set(
            element,
            col.field,
            BiaOptionService.differential(
              Reflect.get(element, col.field),
              this.element ? Reflect.get(this.element, col.field) : undefined
            )
          );
          break;
        case PropType.OneToMany:
          Reflect.set(
            element,
            col.field,
            BiaOptionService.clone(element[col.field])
          );
          break;
      }
    }
    return element;
  }

  getCellData(field: any): any {
    const nestedProperties: string[] = field.field.split('.');
    let value: any = this.element;
    for (const prop of nestedProperties) {
      if (value == null) {
        return null;
      }

      value = value[prop];
    }

    return value;
  }
}
