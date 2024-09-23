import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  TemplateRef,
} from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { Subscription } from 'rxjs';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaFieldBaseComponent } from '../../form/bia-field-base/bia-field-base.component';

@Component({
  selector: 'bia-table-input',
  templateUrl: './bia-table-input.component.html',
  styleUrls: ['./bia-table-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class BiaTableInputComponent
  extends BiaFieldBaseComponent
  implements OnInit, OnDestroy, AfterContentInit
{
  @Input() field: BiaFieldConfig;
  @Input() form: UntypedFormGroup;
  @Input() dictOptionDtos: DictOptionDto[];

  @Output() valueChange = new EventEmitter<void>();
  @Output() complexInput = new EventEmitter<boolean>();

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  // specificInputTemplate: TemplateRef<any>;
  specificInputTemplate: TemplateRef<any>;
  protected sub = new Subscription();

  getDisplayDateFormat(
    displayFormat: BiaFieldNumberFormat | BiaFieldDateFormat | null
  ): BiaFieldDateFormat | null {
    return displayFormat && displayFormat instanceof BiaFieldDateFormat
      ? displayFormat
      : null;
  }

  getDisplayNumberFormat(
    displayFormat: BiaFieldNumberFormat | BiaFieldDateFormat | null
  ): BiaFieldNumberFormat | null {
    return displayFormat && displayFormat instanceof BiaFieldNumberFormat
      ? displayFormat
      : null;
  }

  ngAfterContentInit() {
    this.templates.forEach(item => {
      switch (item.getType()) {
        /*case 'specificInput':
            this.specificInputTemplate = item.template;
          break;*/
        case 'specificInput':
          this.specificInputTemplate = item.template;
          break;
      }
    });
  }

  public onChange() {
    this.valueChange.emit();
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos.filter(x => x.key === key)[0]?.value;
  }

  public onComplexInput(isIn: boolean) {
    this.complexInput.emit(isIn);
  }
}
