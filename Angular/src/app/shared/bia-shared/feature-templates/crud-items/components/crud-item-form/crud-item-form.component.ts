import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { BiaFieldConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

@Component({
  selector: 'bia-crud-item-form',
  templateUrl: './crud-item-form.component.html',
  styleUrls: ['./crud-item-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class CrudItemFormComponent<CrudItem extends BaseDto>  {
  @Input() crudItem: CrudItem = <CrudItem>{};
  @Input() fields: BiaFieldConfig[];
  @Input() dictOptionDtos: DictOptionDto[];

  @Output() save = new EventEmitter<CrudItem>();
  @Output() cancel = new EventEmitter<void>();

  constructor() {
  }

  onCancel() {
    this.cancel.next();
  }

  onSave(crudItem: any) {
    this.save.emit(crudItem);
  }
}

