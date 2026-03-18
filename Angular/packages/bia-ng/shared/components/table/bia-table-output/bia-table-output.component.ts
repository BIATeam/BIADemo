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
import { DtoState } from 'packages/bia-ng/models/enum/public-api';
import { PrimeTemplate } from 'primeng/api';
import { FormatValuePipe } from '../../../pipes/format-value.pipe';
import { JoinPipe } from '../../../pipes/join.pipe';
import { PluckPipe } from '../../../pipes/pluck.pipe';
import { BiaFieldBaseComponent } from '../../form/bia-field-base/bia-field-base.component';

@Component({
  selector: 'bia-table-output',
  templateUrl: './bia-table-output.component.html',
  styleUrls: ['./bia-table-output.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [NgTemplateOutlet, PluckPipe, JoinPipe, FormatValuePipe],
})
export class BiaTableOutputComponent<CrudItem>
  extends BiaFieldBaseComponent<CrudItem>
  implements OnInit, OnDestroy, AfterContentInit
{
  @Input() data: any;
  @Input() ignoreSpecificOutput = false;

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

  isArray(data: any): boolean {
    return Array.isArray(data);
  }

  protected filterDtoState(data: any) {
    if (!Array.isArray(data)) {
      return data;
    }

    return data.filter(item => item.dtoState !== DtoState.Deleted);
  }
}
