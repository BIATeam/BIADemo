import { NgClass, NgFor, NgIf, NgTemplateOutlet } from '@angular/common';
import {
  AfterContentInit,
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
  SimpleChanges,
  TemplateRef,
  ViewChildren,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { Tooltip } from 'primeng/tooltip';
import { Subscription } from 'rxjs';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { BiaFieldConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { FormReadOnlyMode } from '../../../feature-templates/crud-items/model/crud-config';
import {
  BiaFormLayoutConfig,
  BiaFormLayoutConfigField,
  BiaFormLayoutConfigGroup,
  BiaFormLayoutConfigItem,
  BiaFormLayoutConfigRow,
  BiaFormLayoutConfigTabGroup,
} from '../../../model/bia-form-layout-config';
import { CrudHelperService } from '../../../services/crud-helper.service';
import { LayoutMode } from '../../layout/dynamic-layout/dynamic-layout.component';
import { BiaFormLayoutComponent } from '../bia-form-layout/bia-form-layout.component';
import { BiaInputComponent } from '../bia-input/bia-input.component';
import { BiaOutputComponent } from '../bia-output/bia-output.component';

@Component({
  selector: 'bia-form',
  templateUrl: './bia-form.component.html',
  styleUrls: ['./bia-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    NgIf,
    FormsModule,
    ReactiveFormsModule,
    ButtonDirective,
    NgClass,
    NgFor,
    NgTemplateOutlet,
    BiaInputComponent,
    PrimeTemplate,
    BiaOutputComponent,
    TranslateModule,
    BiaFormLayoutComponent,
    Tooltip,
  ],
})
export class BiaFormComponent<TDto extends { id: number }>
  implements OnInit, OnDestroy, OnChanges, AfterContentInit
{
  @Input() element?: TDto;
  @Input() fields: BiaFieldConfig<TDto>[];
  @Input() formLayoutConfig?: BiaFormLayoutConfig<TDto>;
  @Input() formValidators?: ValidatorFn[];
  @Input() formReadOnlyMode: FormReadOnlyMode = FormReadOnlyMode.off;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() isAdd?: boolean;
  @Input() isCrudItemOutdated = false;
  @Input() disableSubmitButton = false;
  @Input() showSubmitButton = true;
  @Input() showFixableState?: boolean;
  @Input() showSplitButton = false;
  @Input() showPopupButton = false;
  @Input() showFullPageButton = false;
  @Input() showFormHelp = true;
  @Input() canFix?: boolean;
  @Input() shown = true;
  @Output() save = new EventEmitter<any>();
  @Output() cancelled = new EventEmitter<void>();
  @Output() readOnlyChanged = new EventEmitter<boolean>();
  @Output() fixableStateChanged = new EventEmitter<boolean>();
  @Output() layoutChanged = new EventEmitter<LayoutMode>();

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  specificInputTemplate: TemplateRef<any>;
  specificOutputTemplate: TemplateRef<any>;

  form?: UntypedFormGroup;
  protected _readOnly = false;
  protected sub = new Subscription();
  fieldsWithoutLayoutConfig: BiaFieldConfig<TDto>[] = [];
  isFixed = false;

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
    this.applyFormReadOnlyMode();
    this.applyFixedState();
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

  ngOnChanges(changes: SimpleChanges) {
    if (changes.element) {
      if (this.element && this.form) {
        this.form.reset();
        if (this.element) {
          //this.form.patchValue({ ...this.element });
          //this.initForm();
          this.updateFormGroup(this.form, this.element);
        }
      }

      this.applyFormReadOnlyMode();
      this.applyFixedState();
    }
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

  updateFormGroup(
    formGroup: UntypedFormGroup,
    crudItem: { [key: string]: any }
  ) {
    for (const key in crudItem) {
      if (Object.prototype.hasOwnProperty.call(crudItem, key)) {
        const control = formGroup.get(key);

        if (control && !(control instanceof UntypedFormGroup)) {
          control.setValue(crudItem[key]);
        }
      }
    }

    Object.keys(formGroup.controls).forEach(controlKey => {
      const control = formGroup.get(controlKey);
      if (control instanceof UntypedFormGroup) {
        this.updateFormGroup(control, crudItem);
      }
    });
  }

  /**
   * Find the first active form element and set the focus on it.
   */
  protected setAutoFocus() {
    if (this.formLayoutConfig?.autoFocusFirstField === false) {
      return;
    }

    const formElement = 'input, textarea, select';
    const firstActiveField = this.formElements
      .map(element => element.nativeElement.querySelector(formElement))
      .find(field => {
        return this.readOnly ? field : field && !field.disabled;
      });
    if (firstActiveField) {
      firstActiveField.focus();
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
    const element = this.form ? this.getElement() : {};
    return { element, errorMessages };
  }

  protected initForm() {
    this.fieldsWithoutLayoutConfig = [...this.fields];
    this.initFieldsWithLayoutConfig(this.formLayoutConfig);

    if (this.form && this.formValidators) {
      this.form.addValidators(this.formValidators);
    }
  }

  protected applyFormReadOnlyMode() {
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
      this.setAutoFocus();
    });
  }

  protected initFieldsWithLayoutConfig(
    formLayoutConfig?: BiaFormLayoutConfig<TDto>
  ) {
    const formFields: { [key: string]: any } = { id: [this.element?.id] };

    if (formLayoutConfig) {
      const columnFields: BiaFormLayoutConfigField<TDto>[] =
        this.getFieldsFromItems(formLayoutConfig.items, formFields);

      columnFields.forEach(columnField => {
        const fieldIndex = this.fields.findIndex(
          x => x.field === columnField.field
        );
        if (fieldIndex !== -1) {
          const fieldToRemoveIndex = this.fieldsWithoutLayoutConfig.findIndex(
            x => x.field === columnField.field
          );
          if (fieldToRemoveIndex !== -1) {
            this.fieldsWithoutLayoutConfig.splice(fieldToRemoveIndex, 1);
          }
        }
      });
    }
    this.buildFormFields(formFields, this.fieldsWithoutLayoutConfig);

    this.form = this.formBuilder.group(formFields);
  }

  protected getFieldsFromRow(
    row: BiaFormLayoutConfigRow<TDto>,
    formFields: { [key: string]: any }
  ): BiaFormLayoutConfigField<TDto>[] {
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
      const groupFields = this.getFieldsFromRows(g.rows, formFields);
      groupFields.forEach(gf => fields.push(gf));
    });

    fields.forEach(columnField => {
      const fieldIndex = this.fields.findIndex(
        x => x.field === columnField.field
      );

      if (fieldIndex !== -1) {
        columnField.fieldConfig = this.fields[fieldIndex];
        this.buildFormField(formFields, columnField.fieldConfig);
      }
    });

    const tabGroups = row.columns
      .filter(
        (c): c is BiaFormLayoutConfigTabGroup<TDto> =>
          c instanceof BiaFormLayoutConfigTabGroup
      )
      .flatMap(c => c as BiaFormLayoutConfigTabGroup<TDto>);

    tabGroups.forEach(g => {
      const tabFields = this.getFieldsFromTab(g, formFields);
      tabFields.forEach(tf => fields.push(tf));
    });

    return fields;
  }

  protected getFieldsFromRows(
    rows: BiaFormLayoutConfigRow<TDto>[],
    formFields: { [key: string]: any }
  ): BiaFormLayoutConfigField<TDto>[] {
    return rows.flatMap(row => this.getFieldsFromRow(row, formFields));
  }

  protected getFieldsFromTab(
    tabGroup: BiaFormLayoutConfigTabGroup<TDto>,
    formFields: { [key: string]: any }
  ): BiaFormLayoutConfigField<TDto>[] {
    const tabConfigFields = tabGroup.tabs.flatMap(tab => {
      const tabFields: { [key: string]: any } = {};
      const f = this.getFieldsFromItems(tab.items, tabFields);
      formFields[tab.id] = this.formBuilder.group(tabFields);
      return f;
    });

    return tabConfigFields;
  }

  protected getFieldsFromItems(
    items: BiaFormLayoutConfigItem<TDto>[],
    formFields: { [key: string]: any }
  ): BiaFormLayoutConfigField<TDto>[] {
    return items.flatMap(item => {
      switch (item.type) {
        case 'tab':
          return this.getFieldsFromTab(item, formFields);
        case 'group':
          return this.getFieldsFromRows(item.rows, formFields);
        case 'row':
          return this.getFieldsFromRow(item, formFields);
        default:
          return [];
      }
    });
  }

  protected buildFormField(
    fields: { [key: string]: any },
    formField: BiaFieldConfig<TDto>
  ) {
    const validators: ValidatorFn[] = [];
    if (formField.validators && formField.validators.length > 0) {
      validators.push(...formField.validators);
    }
    if (formField.isRequired) {
      validators.push(Validators.required);
    }
    if (validators)
      fields[formField.field as string] = [
        this.element ? this.element[formField.field] : null,
        validators,
      ];
    else {
      fields[formField.field as string] = [
        this.element ? this.element[formField.field] : null,
      ];
    }
  }

  protected formFields() {
    const fields: { [key: string]: any } = {};
    this.buildFormFields(fields, this.fields);
    fields['id'] = [this.element?.id];
    return fields;
  }

  protected buildFormFields(
    fields: { [key: string]: any },
    formFields: BiaFieldConfig<TDto>[]
  ) {
    for (const col of formFields) {
      this.buildFormField(fields, col);
    }
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

  public getElement(): TDto {
    if (!this.form) {
      throw Error('Form must be initialized');
    }

    const element: TDto = this.flattenFormGroup(this.form) as TDto;
    element.id = element.id > 0 ? element.id : 0;
    CrudHelperService.applyDiff(this.element, element, this.fields);
    return element;
  }

  flattenFormGroup(
    formGroup: UntypedFormGroup,
    parentKey?: string,
    result: { [key: string]: any } = {}
  ): { [key: string]: any } {
    for (const key in formGroup.controls) {
      if (Object.prototype.hasOwnProperty.call(formGroup.controls, key)) {
        const control = formGroup.controls[key];
        const newKey = parentKey ? `${parentKey}.${key}` : key;

        if (control instanceof UntypedFormGroup) {
          this.flattenFormGroup(control, newKey, result);
        } else {
          result[key] = control.value;
        }
      }
    }
    return result;
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

  get showHeaderContainer(): boolean {
    return (
      this.isFixableButtonVisible ||
      this.showPopupButton ||
      this.showSplitButton ||
      this.showFullPageButton
    );
  }

  protected applyFixedState(): void {
    this.isFixed = (this.element as any)?.isFixed === true;
  }

  get isFixableButtonVisible(): boolean {
    return (
      this.showFixableState === true &&
      (this.canFix === true || this.isFixed === true)
    );
  }

  get fixableButtonLabel(): string {
    return this.isFixed === true ? 'bia.fixed' : 'bia.unfixed';
  }

  get fixableButtonIcon(): string {
    return this.isFixed === true ? 'pi pi-lock' : 'pi pi-lock-open';
  }

  onFixableButtonClicked(): void {
    this.fixableStateChanged.emit(!this.isFixed);
  }

  switchToSplit() {
    this.layoutChanged.emit(LayoutMode.splitPage);
  }

  switchToFullPage() {
    this.layoutChanged.emit(LayoutMode.fullPage);
  }

  switchToPopup() {
    this.layoutChanged.emit(LayoutMode.popup);
  }
}
