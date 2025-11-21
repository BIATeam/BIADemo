import { NgTemplateOutlet } from '@angular/common';
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
import { TranslateModule } from '@ngx-translate/core';
import { BiaFieldConfig } from 'packages/bia-ng/models/public-api';
import { PrimeTemplate } from 'primeng/api';
import { DictOptionDto } from '../../table/bia-table/dict-option-dto';
import { BiaFieldBaseComponent } from '../bia-field-base/bia-field-base.component';
import { BiaInputComponent } from '../public-api';

@Component({
  selector: 'bia-output',
  templateUrl: './bia-output.component.html',
  styleUrls: ['./bia-output.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [NgTemplateOutlet, TranslateModule, BiaInputComponent],
})
export class BiaOutputComponent<CrudItem>
  extends BiaFieldBaseComponent<CrudItem>
  implements OnInit, OnDestroy, AfterContentInit
{
  @Input() field: BiaFieldConfig<CrudItem>;
  @Input() data: any;
  @Input() form: UntypedFormGroup;
  @Input() dictOptionDtos: DictOptionDto[];

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  specificOutputTemplate: TemplateRef<any>;

  ngAfterContentInit() {
    this.templates.forEach(item => {
      switch (item.getType()) {
        case 'specificOutput':
          this.specificOutputTemplate = item.template;
          break;
      }
    });
  }
}
