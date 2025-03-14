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
import { DtoState } from '../../../model/dto-state.enum';
import { BiaFieldBaseComponent } from '../../form/bia-field-base/bia-field-base.component';
import {
  NgIf,
  NgTemplateOutlet,
  NgSwitch,
  NgSwitchCase,
  NgSwitchDefault,
} from '@angular/common';
import { PluckPipe } from '../../../pipes/pluck.pipe';
import { JoinPipe } from '../../../pipes/join.pipe';
import { FormatValuePipe } from '../../../pipes/format-value.pipe';

@Component({
  selector: 'bia-table-output',
  templateUrl: './bia-table-output.component.html',
  styleUrls: ['./bia-table-output.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    NgIf,
    NgTemplateOutlet,
    NgSwitch,
    NgSwitchCase,
    NgSwitchDefault,
    PluckPipe,
    JoinPipe,
    FormatValuePipe,
  ],
})
export class BiaTableOutputComponent<CrudItem>
  extends BiaFieldBaseComponent<CrudItem>
  implements OnInit, OnDestroy, AfterContentInit
{
  @Input() data: any;

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

  protected filterDtoState(data: any) {
    if (!Array.isArray(data)) {
      return data;
    }

    return data.filter(item => item.dtoState !== DtoState.Deleted);
  }
}
