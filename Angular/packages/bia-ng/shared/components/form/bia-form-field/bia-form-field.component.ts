import { NgTemplateOutlet } from '@angular/common';
import { Component, Input, TemplateRef } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { BiaFieldConfig } from 'packages/bia-ng/models/public-api';
import { PrimeTemplate } from 'primeng/api';
import { DictOptionDto } from '../../table/bia-table/dict-option-dto';
import { BiaInputComponent } from '../bia-input/bia-input.component';
import { BiaOutputComponent } from '../bia-output/bia-output.component';

@Component({
  selector: 'bia-form-field',
  imports: [
    PrimeTemplate,
    BiaInputComponent,
    BiaOutputComponent,
    NgTemplateOutlet,
  ],
  templateUrl: './bia-form-field.component.html',
  styleUrl: './bia-form-field.component.scss',
})
export class BiaFormFieldComponent<TDto extends { id: number | string }> {
  @Input() element?: TDto;
  @Input() field: BiaFieldConfig<TDto>;
  @Input() isAdd?: boolean;
  @Input() form: UntypedFormGroup;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() readOnly: boolean;
  @Input() specificInputTemplate: TemplateRef<any>;
  @Input() specificOutputTemplate: TemplateRef<any>;

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
}
