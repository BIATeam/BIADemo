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
import { UntypedFormGroup } from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaFieldBaseComponent } from '../bia-field-base/bia-field-base.component';

@Component({
  selector: 'bia-input',
  templateUrl: './bia-input.component.html',
  styleUrls: ['./bia-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class BiaInputComponent
  extends BiaFieldBaseComponent
  implements OnInit, OnDestroy, AfterContentInit
{
  @Input() field: BiaFieldConfig;
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
