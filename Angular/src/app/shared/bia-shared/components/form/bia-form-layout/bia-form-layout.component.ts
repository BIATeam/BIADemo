import {
  NgFor,
  NgIf,
  NgSwitch,
  NgSwitchCase,
  NgTemplateOutlet,
} from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Input,
  TemplateRef,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { Badge } from 'primeng/badge';
import { PanelModule } from 'primeng/panel';
import { Ripple } from 'primeng/ripple';
import { Tab, TabList, TabPanel, Tabs } from 'primeng/tabs';
import { BiaFormLayoutConfig } from '../../../model/bia-form-layout-config';
import { DictOptionDto } from '../../table/bia-table/dict-option-dto';
import { BiaInputComponent } from '../bia-input/bia-input.component';
import { BiaOutputComponent } from '../bia-output/bia-output.component';

@Component({
  selector: 'bia-form-layout',
  templateUrl: './bia-form-layout.component.html',
  styleUrls: ['./bia-form-layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    NgIf,
    FormsModule,
    ReactiveFormsModule,
    NgFor,
    NgSwitch,
    NgSwitchCase,
    NgTemplateOutlet,
    BiaInputComponent,
    PrimeTemplate,
    BiaOutputComponent,
    TranslateModule,
    Badge,
    Tabs,
    TabList,
    Ripple,
    Tab,
    TabPanel,
    PanelModule,
  ],
})
export class BiaFormLayoutComponent<TDto extends { id: number }> {
  @Input() element?: TDto;
  @Input() formLayoutConfig: BiaFormLayoutConfig<TDto>;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() readOnly: boolean;
  @Input() isAdd?: boolean;
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
