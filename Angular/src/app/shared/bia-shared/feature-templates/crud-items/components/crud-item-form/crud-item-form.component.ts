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
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { BiaFieldConfig } from 'src/app/shared/bia-shared/model/bia-field-config';

@Component({
  selector: 'bia-crud-item-form',
  templateUrl: './crud-item-form.component.html',
  styleUrls: ['./crud-item-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class CrudItemFormComponent<CrudItem extends BaseDto> {
  @Input() crudItem?: CrudItem;
  @Input() fields: BiaFieldConfig<CrudItem>[];
  @Input() formValidators?: ValidatorFn[];
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() isAdd?: boolean;

  @Output() save = new EventEmitter<CrudItem>();
  @Output() cancel = new EventEmitter<void>();

  @ViewChild(BiaFormComponent) biaFormComponent: BiaFormComponent<CrudItem>;

  onCancel() {
    this.cancel.next();
  }

  onSave(crudItem: any) {
    this.save.emit(crudItem);
  }
}
