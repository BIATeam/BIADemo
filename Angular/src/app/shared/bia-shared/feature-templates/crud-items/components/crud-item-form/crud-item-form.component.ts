import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { ValidatorFn } from '@angular/forms';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { LayoutMode } from 'src/app/shared/bia-shared/components/layout/dynamic-layout/dynamic-layout.component';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { BiaFieldConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaFormLayoutConfig } from 'src/app/shared/bia-shared/model/bia-form-layout-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { BiaFormComponent as BiaFormComponent_1 } from '../../../../components/form/bia-form/bia-form.component';
import { FormReadOnlyMode } from '../../model/crud-config';

@Component({
  selector: 'bia-crud-item-form',
  templateUrl: './crud-item-form.component.html',
  styleUrls: ['./crud-item-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [BiaFormComponent_1, BiaFormComponent],
})
export class CrudItemFormComponent<CrudItem extends BaseDto> {
  @Input() crudItem?: CrudItem;
  @Input() fields: BiaFieldConfig<CrudItem>[];
  @Input() formLayoutConfig?: BiaFormLayoutConfig<CrudItem>;
  @Input() formValidators?: ValidatorFn[];
  @Input() formReadOnlyMode: FormReadOnlyMode = FormReadOnlyMode.off;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() isAdd?: boolean;
  @Input() isCrudItemOutdated = false;
  @Input() showSubmitButton = true;
  @Input() showFixableState?: boolean;
  @Input() canFix = false;
  @Input() showSplitButton = false;
  @Input() showPopupButton = false;
  @Input() showFullPageButton = false;

  @Output() save = new EventEmitter<CrudItem>();
  @Output() cancelled = new EventEmitter<void>();
  @Output() fixedChanged = new EventEmitter<boolean>();
  @Output() layoutChanged = new EventEmitter<LayoutMode>();
  @Output() readOnlyChanged = new EventEmitter<boolean>();

  @ViewChild(BiaFormComponent) biaFormComponent: BiaFormComponent<CrudItem>;

  onCancel() {
    this.cancelled.next();
  }

  onSave(crudItem: CrudItem) {
    this.save.emit(crudItem);
  }

  onFixableStateChanged(fixed: boolean) {
    this.fixedChanged.emit(fixed);
  }

  getOptionDto(key: string) {
    return this.dictOptionDtos?.filter(x => x.key === key)[0]?.value;
  }
}
