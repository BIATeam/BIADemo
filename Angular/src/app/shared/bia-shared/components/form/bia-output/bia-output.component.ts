import {
  NgIf,
  NgSwitch,
  NgSwitchCase,
  NgSwitchDefault,
  NgTemplateOutlet,
} from '@angular/common';
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
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { FloatLabel } from 'primeng/floatlabel';
import { InputText } from 'primeng/inputtext';
import { Subscription } from 'rxjs';
import { BiaFieldConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { FormatValuePipe } from '../../../pipes/format-value.pipe';
import { JoinPipe } from '../../../pipes/join.pipe';
import { PluckPipe } from '../../../pipes/pluck.pipe';
import { BiaFieldBaseComponent } from '../bia-field-base/bia-field-base.component';

@Component({
  selector: 'bia-output',
  templateUrl: './bia-output.component.html',
  styleUrls: ['./bia-output.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    NgIf,
    NgTemplateOutlet,
    NgSwitch,
    InputText,
    NgSwitchCase,
    NgSwitchDefault,
    TranslateModule,
    PluckPipe,
    JoinPipe,
    FormatValuePipe,
    FloatLabel,
  ],
})
export class BiaOutputComponent<CrudItem>
  extends BiaFieldBaseComponent<CrudItem>
  implements OnInit, OnDestroy, AfterContentInit
{
  @Input() field: BiaFieldConfig<CrudItem>;
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
