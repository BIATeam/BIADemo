import {
  ChangeDetectionStrategy,
  Component,
  Input,
  TemplateRef,
} from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { BiaFormLayoutConfig } from '../../../model/bia-form-layout-config';
import { DictOptionDto } from '../../table/bia-table/dict-option-dto';

@Component({
  selector: 'bia-form-layout',
  templateUrl: './bia-form-layout.component.html',
  styleUrls: ['./bia-form-layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class BiaFormLayoutComponent<TDto extends { id: number }> {
  @Input() element?: TDto;
  @Input() formLayoutConfig: BiaFormLayoutConfig<TDto>;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() readOnly: boolean;
  @Input() form: UntypedFormGroup;
  @Input() specificInputTemplate: TemplateRef<any>;
  @Input() specificOutputTemplate: TemplateRef<any>;

  constructor(public formBuilder: UntypedFormBuilder) {}

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

  getFormGroup(id: string): UntypedFormGroup {
    return this.form?.controls[id] as UntypedFormGroup;
  }
}
