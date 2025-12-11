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
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  TemplateRef,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormGroup,
} from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { AutoFocusModule } from 'primeng/autofocus';
import { BaseComponent } from 'primeng/basecomponent';
import { Checkbox } from 'primeng/checkbox';
import { DatePicker } from 'primeng/datepicker';
import { InputNumber } from 'primeng/inputnumber';
import { InputText } from 'primeng/inputtext';
import { MultiSelect } from 'primeng/multiselect';
import { Select } from 'primeng/select';
import { Subscription } from 'rxjs';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { CrudHelperService } from '../../../services/crud-helper.service';
import { BiaFieldBaseComponent } from '../../form/bia-field-base/bia-field-base.component';

@Component({
  selector: 'bia-table-input',
  templateUrl: './bia-table-input.component.html',
  styleUrls: ['./bia-table-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    NgTemplateOutlet,
    NgSwitch,
    NgSwitchCase,
    Select,
    MultiSelect,
    Checkbox,
    InputNumber,
    DatePicker,
    NgSwitchDefault,
    InputText,
    AutoFocusModule,
  ],
})
export class BiaTableInputComponent<CrudItem>
  extends BiaFieldBaseComponent<CrudItem>
  implements OnInit, OnDestroy, AfterContentInit
{
  @Input() field: BiaFieldConfig<CrudItem>;
  @Input() form: UntypedFormGroup;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() focusByDefault = false;

  @Output() valueChange = new EventEmitter<void>();
  @Output() complexInput = new EventEmitter<boolean>();

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
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

  onPanelHide(multiselect: MultiSelect) {
    this.onComplexInput(false);
    multiselect.el.nativeElement.querySelector('input')?.focus();
  }

  onMouseDown(element: any, event: MouseEvent) {
    if (event.button !== 0) {
      return;
    }

    if (element instanceof BaseComponent) {
      CrudHelperService.scrollHorizontalToElementInTable(
        element.el.nativeElement
      );
    } else if (element instanceof HTMLElement) {
      CrudHelperService.scrollHorizontalToElementInTable(element);
    }
  }
}
