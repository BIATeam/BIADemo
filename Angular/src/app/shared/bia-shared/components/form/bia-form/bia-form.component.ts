import {
  AfterContentInit,
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  TemplateRef,
  ViewChildren,
} from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  ValidatorFn,
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
import { FormReadOnlyMode } from '../../../feature-templates/crud-items/model/crud-config';
import { BaseDto } from '../../../model/base-dto';
import {
  BiaFormLayoutConfig,
  BiaFormLayoutConfigField,
  BiaFormLayoutConfigGroup,
  BiaFormLayoutConfigRow,
} from '../../../model/bia-form-layout-config';

@Component({
  selector: 'bia-form',
  templateUrl: './bia-form.component.html',
  styleUrls: ['./bia-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class BiaFormComponent<TDto extends { id: number }>
  implements OnInit, OnDestroy, OnChanges, AfterContentInit, AfterViewInit
{
  @Input() element?: TDto;
  @Input() fields: BiaFieldConfig<TDto>[];
  @Input() formLayoutConfig?: BiaFormLayoutConfig<TDto>;
  @Input() formValidators?: ValidatorFn[];
  @Input() formReadOnlyMode: FormReadOnlyMode;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() isAdd?: boolean;
  @Input() isCrudItemOutdated = false;
  @Input() disableSubmitButton = false;
  @Input() showSubmitButton = true;
  @Input() showFixableState? = false;
  @Input() canFix = false;
  @Input() isFixed? = false;
  @Output() save = new EventEmitter<any>();
  @Output() cancelled = new EventEmitter<void>();
  @Output() readOnlyChanged = new EventEmitter<boolean>();
  @Output() fixableStateChanged = new EventEmitter<boolean>();

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  specificInputTemplate: TemplateRef<any>;
  specificOutputTemplate: TemplateRef<any>;

  form?: UntypedFormGroup;
  private _readOnly = false;
  protected sub = new Subscription();
  fieldsWithoutLayoutConfig: BiaFieldConfig<TDto>[] = [];

  @ViewChildren('refFormField', { read: ElementRef })
  formElements: QueryList<ElementRef>;

  constructor(public formBuilder: UntypedFormBuilder) {}

  get readOnly(): boolean {
    return this._readOnly;
  }

  set readOnly(value: boolean) {
    this._readOnly = value;
    this.readOnlyChanged.emit(value);
  }

  ngOnInit() {
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

    this.applyFormReadOnlyMode();
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.setFocus();
    });
  }

  get submitButtonLabel(): string {
    if (this.isAdd === true) {
      return 'bia.add';
    }
    switch (this.formReadOnlyMode) {
      case FormReadOnlyMode.off:
      case FormReadOnlyMode.on:
        return 'bia.save';
      case FormReadOnlyMode.clickToEdit:
        return this.readOnly === true ? 'bia.edit' : 'bia.save';
    }
  }

  get submitButtonIcon(): string {
    if (this.isAdd === true) {
      return 'pi-plus';
    }
    switch (this.formReadOnlyMode) {
      case FormReadOnlyMode.off:
      case FormReadOnlyMode.on:
        return 'pi-check';
      case FormReadOnlyMode.clickToEdit:
        return this.readOnly === true ? 'pi-pencil' : 'pi-check';
    }
  }

  get isSubmitButtonDisabled(): boolean {
    const readOnlyModeOn = this.formReadOnlyMode === FormReadOnlyMode.on;
    const clickToEdit =
      this.formReadOnlyMode === FormReadOnlyMode.clickToEdit &&
      this.readOnly === true;
    const invalidForm = clickToEdit === false && this.form?.valid === false;

    return (
      this.disableSubmitButton ||
      readOnlyModeOn ||
      invalidForm ||
      this.isCrudItemOutdated
    );
  }

  get isSubmitButtonVisible(): boolean {
    return (
      this.showSubmitButton === true &&
      this.formReadOnlyMode !== FormReadOnlyMode.on
    );
  }

  get cancelButtonLabel(): string {
    switch (this.formReadOnlyMode) {
      case FormReadOnlyMode.off:
        return 'bia.cancel';
      case FormReadOnlyMode.on:
        return 'bia.close';
      case FormReadOnlyMode.clickToEdit:
        return this.readOnly === true ? 'bia.close' : 'bia.cancel';
    }
  }

  /**
   * Find the first active form element and set the focus on it.
   */
  protected setFocus() {
    const formElement = 'input, textarea, select';
    const firstActiveField = this.formElements.find(field => {
      const element = field.nativeElement.querySelector(formElement);
      return this.readOnly ? element : element && !element.disabled;
    });
    if (firstActiveField) {
      const element = firstActiveField.nativeElement.querySelector(formElement);
      if (element) {
        element.focus();
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
          if (controlErrors) {
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
    this.fieldsWithoutLayoutConfig = [...this.fields];
    this.initFieldsWithLayoutConfig();

    this.form = this.formBuilder.group(this.formFields());
    if (this.formValidators) {
      this.form.addValidators(this.formValidators);
    }

    this.applyFormReadOnlyMode();
  }

  private applyFormReadOnlyMode() {
    if (!this.form) {
      return;
    }

    switch (this.formReadOnlyMode) {
      case FormReadOnlyMode.off:
        this.readOnly = false;
        this.form.enable();
        break;
      case FormReadOnlyMode.clickToEdit:
      case FormReadOnlyMode.on:
        this.readOnly = true;
        this.form.disable();
        break;
    }

    setTimeout(() => {
      this.setFocus();
    });
  }

  private initFieldsWithLayoutConfig() {
    if (!this.formLayoutConfig) {
      return;
    }

    const getFieldsFromRow = (
      row: BiaFormLayoutConfigRow<TDto>
    ): BiaFormLayoutConfigField<TDto>[] => {
      const fields = row.columns
        .filter(
          (c): c is BiaFormLayoutConfigField<TDto> =>
            c instanceof BiaFormLayoutConfigField
        )
        .flatMap(c => c as BiaFormLayoutConfigField<TDto>);

      const groups = row.columns
        .filter(
          (c): c is BiaFormLayoutConfigGroup<TDto> =>
            c instanceof BiaFormLayoutConfigGroup
        )
        .flatMap(c => c as BiaFormLayoutConfigGroup<TDto>);

      groups.forEach(g => {
        const groupFields = getFieldsFromRows(g.rows);
        groupFields.forEach(gf => fields.push(gf));
      });

      return fields;
    };

    const getFieldsFromRows = (
      rows: BiaFormLayoutConfigRow<TDto>[]
    ): BiaFormLayoutConfigField<TDto>[] =>
      rows.flatMap(row => getFieldsFromRow(row));

    const columnFields: BiaFormLayoutConfigField<TDto>[] =
      this.formLayoutConfig.items.flatMap(item => {
        switch (item.type) {
          case 'group':
            return getFieldsFromRows(item.rows);
          case 'row':
            return getFieldsFromRow(item);
          default:
            return [];
        }
      });

    columnFields.forEach(columnField => {
      const fieldIndex = this.fields.findIndex(
        x => x.field === columnField.field
      );
      if (fieldIndex !== -1) {
        columnField.fieldConfig = this.fields[fieldIndex];

        const fieldToRemoveIndex = this.fieldsWithoutLayoutConfig.findIndex(
          x => x.field === columnField.field
        );
        if (fieldToRemoveIndex !== -1) {
          this.fieldsWithoutLayoutConfig.splice(fieldToRemoveIndex, 1);
        }
      }
    });
  }

  protected formFields() {
    const fields: { [key: string]: any } = { id: [this.element?.id] };
    for (const col of this.fields) {
      if (col.validators && col.validators.length > 0) {
        fields[col.field as string] = [
          this.element ? this.element[col.field] : null,
          col.validators,
        ];
      } else if (col.isRequired) {
        fields[col.field as string] = [
          this.element ? this.element[col.field] : null,
          Validators.required,
        ];
      } else {
        fields[col.field as string] = [
          this.element ? this.element[col.field] : null,
        ];
      }
    }
    return fields;
  }

  onCancel() {
    this.cancelled.next();
  }

  onSubmit() {
    if (
      this.formReadOnlyMode === FormReadOnlyMode.clickToEdit &&
      this.readOnly === true
    ) {
      this.readOnly = false;
      return;
    }

    if (this.form?.valid) {
      const element: any = this.getElement();
      this.save.emit(element);
    }
  }

  public getElement() {
    const element: TDto = this.form?.value;
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
              Reflect.get(element, col.field) as BaseDto[],
              (this.element
                ? (Reflect.get(this.element, col.field) ?? [])
                : []) as BaseDto[]
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
      if (value === undefined) {
        return null;
      }

      value = value[prop];
    }

    return value;
  }

  get isFixableButtonVisible(): boolean {
    return this.showFixableState && this.canFix ? true : this.isFixed === true;
  }

  get fixableButtonLabel(): string {
    return this.isFixed ? 'bia.fixed' : 'bia.unfixed';
  }

  get fixableButtonIcon(): string {
    return this.isFixed ? 'pi pi-lock' : 'pi pi-lock-open';
  }

  onFixableButtonClicked(): void {
    console.log('Emit from BiaForm', !this.isFixed);
    this.fixableStateChanged.emit(!this.isFixed);
  }
}
