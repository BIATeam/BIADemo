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

@Component({
  selector: 'bia-table-output',
  templateUrl: './bia-table-output.component.html',
  styleUrls: ['./bia-table-output.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class BiaTableOutputComponent
  extends BiaFieldBaseComponent
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
