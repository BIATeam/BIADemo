import { NgTemplateOutlet } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Input,
  TemplateRef,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormGroup,
} from '@angular/forms';
import { BiaFormLayoutConfig } from '@bia-team/bia-ng/models';
import { TranslateModule } from '@ngx-translate/core';
import { Badge } from 'primeng/badge';
import { PanelModule } from 'primeng/panel';
import { Ripple } from 'primeng/ripple';
import { Tab, TabList, TabPanel, Tabs } from 'primeng/tabs';
import { DictOptionDto } from '../../table/bia-table/dict-option-dto';
import { BiaFormFieldComponent } from '../bia-form-field/bia-form-field.component';

@Component({
  selector: 'bia-form-layout',
  templateUrl: './bia-form-layout.component.html',
  styleUrls: ['./bia-form-layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgTemplateOutlet,
    TranslateModule,
    Badge,
    Tabs,
    TabList,
    Ripple,
    Tab,
    TabPanel,
    PanelModule,
    BiaFormFieldComponent,
  ],
})
export class BiaFormLayoutComponent<CrudItem> {
  @Input() element?: CrudItem;
  @Input() formLayoutConfig: BiaFormLayoutConfig<CrudItem>;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() readOnly: boolean;
  @Input() isAdd?: boolean;
  @Input() specificInputTemplate: TemplateRef<any>;
  @Input() specificOutputTemplate: TemplateRef<any>;
  @Input() form: UntypedFormGroup;
}
