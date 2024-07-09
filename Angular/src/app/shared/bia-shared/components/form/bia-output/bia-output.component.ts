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
import { PrimeTemplate } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BiaFieldConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaFieldBaseComponent } from '../bia-field-base/bia-field-base.component';

@Component({
  selector: 'bia-output',
  templateUrl: './bia-output.component.html',
  styleUrls: ['./bia-output.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class BiaOutputComponent
  extends BiaFieldBaseComponent
  implements OnInit, OnDestroy, AfterContentInit
{
  @Input() field: BiaFieldConfig;
  @Input() data: any;

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  specificOutputTemplate: TemplateRef<any>;
  protected sub = new Subscription();

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
