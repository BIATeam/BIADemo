import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  Input,
  OnDestroy,
  OnInit,
  QueryList,
  TemplateRef,
} from '@angular/core';
import { UntypedFormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaFieldBaseComponent } from '../bia-field-base/bia-field-base.component';
import { NgIf, NgTemplateOutlet, NgSwitch, NgSwitchCase, NgSwitchDefault } from '@angular/common';
import { Select } from 'primeng/select';
import { MultiSelect } from 'primeng/multiselect';
import { Checkbox } from 'primeng/checkbox';
import { InputNumber } from 'primeng/inputnumber';
import { DatePicker } from 'primeng/datepicker';
import { InputText } from 'primeng/inputtext';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'bia-input',
    templateUrl: './bia-input.component.html',
    styleUrls: ['./bia-input.component.scss'],
    changeDetection: ChangeDetectionStrategy.Default,
    imports: [FormsModule, ReactiveFormsModule, NgIf, NgTemplateOutlet, NgSwitch, NgSwitchCase, Select, MultiSelect, Checkbox, InputNumber, DatePicker, NgSwitchDefault, InputText, TranslateModule]
})
export class BiaInputComponent<CrudItem>
  extends BiaFieldBaseComponent<CrudItem>
  implements OnInit, OnDestroy, AfterContentInit
{
  @Input() field: BiaFieldConfig<CrudItem>;
  @Input() readOnly: boolean;
  @Input() form: UntypedFormGroup;
  @Input() dictOptionDtos: DictOptionDto[];

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  // specificInputTemplate: TemplateRef<any>;
  specificInputTemplate: TemplateRef<any>;

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

  ngAfterContentInit() {
    this.templates.forEach(item => {
      switch (item.getType()) {
        /*case 'specificInput':
            this.specificInputTemplate = item.template;
          break;*/
        case 'specificInput':
          this.specificInputTemplate = item.template;
          break;
      }
    });
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos?.filter(x => x.key === key)[0]?.value;
  }
}
