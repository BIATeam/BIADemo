import { NgTemplateOutlet } from '@angular/common';
import { Component, Input, TemplateRef } from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormGroup,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import {
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
} from 'packages/bia-ng/models/public-api';
import { Checkbox } from 'primeng/checkbox';
import { DatePicker } from 'primeng/datepicker';
import { FloatLabel } from 'primeng/floatlabel';
import { InputNumber } from 'primeng/inputnumber';
import { InputText } from 'primeng/inputtext';
import { MultiSelect } from 'primeng/multiselect';
import { Select } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { DictOptionDto } from '../../table/bia-table/dict-option-dto';
import { BiaFieldBaseComponent } from '../bia-field-base/bia-field-base.component';

@Component({
  selector: 'bia-form-field',
  imports: [
    NgTemplateOutlet,
    FormsModule,
    ReactiveFormsModule,
    NgTemplateOutlet,
    Select,
    MultiSelect,
    Checkbox,
    InputNumber,
    DatePicker,
    InputText,
    TranslateModule,
    FloatLabel,
    TextareaModule,
  ],
  templateUrl: './bia-form-field.component.html',
  styleUrls: ['./bia-form-field.component.scss'],
})
export class BiaFormFieldComponent<
  CrudItem,
> extends BiaFieldBaseComponent<CrudItem> {
  @Input() element?: CrudItem;
  @Input() isAdd?: boolean;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() readOnly: boolean;
  @Input() specificInputTemplate: TemplateRef<any>;
  @Input() specificOutputTemplate: TemplateRef<any>;
  @Input() form: UntypedFormGroup;

  get fieldDisabled(): boolean {
    return this.form?.get(this.field.field)?.disabled === true;
  }

  get formDisabled(): boolean {
    return this.form?.disabled === true;
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

  getDisplayDateFormat(
    displayFormat: BiaFieldNumberFormat | BiaFieldDateFormat | null
  ): BiaFieldDateFormat | null {
    return displayFormat && displayFormat instanceof BiaFieldDateFormat
      ? displayFormat
      : null;
  }

  getDisplayNumberFormat(
    displayFormat: BiaFieldNumberFormat | BiaFieldDateFormat | null
  ): BiaFieldNumberFormat | null {
    return displayFormat && displayFormat instanceof BiaFieldNumberFormat
      ? displayFormat
      : null;
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos?.filter(x => x.key === key)[0]?.value;
  }
}
