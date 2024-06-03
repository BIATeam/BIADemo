import { Component, EventEmitter, Input, Output } from '@angular/core';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { CrudConfig } from '../../model/crud-config';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { clone } from 'src/app/shared/bia-shared/utils';

@Component({
  selector: 'bia-crud-item-bulk-form',
  templateUrl: './crud-item-bulk-form.component.html',
  styleUrls: ['./crud-item-bulk-form.component.scss'],
})
export class CrudItemBulkFormComponent {
  deleteChecked = false;
  updateChecked = false;
  insertChecked = false;

  displayedColumns: KeyValuePair[];
  displayedColumnErrors: KeyValuePair[];
  fieldsConfigErrors: BiaFieldsConfig;
  sortFieldValue = '';
  protected crudConfigurationError: CrudConfig;

  protected _CrudConfiguration: CrudConfig;
  get crudConfiguration(): CrudConfig {
    return this._CrudConfiguration;
  }
  @Input() set crudConfiguration(value: CrudConfig) {
    this._CrudConfiguration = value;
    this.initTableParam();
  }

  @Input() bulkData: any;
  @Output() save = new EventEmitter<any[]>();
  @Output() cancel = new EventEmitter<void>();
  @Output() fileSelected = new EventEmitter<any>();

  initTableParam() {
    if (this.crudConfiguration) {
      this.displayedColumns = this.crudConfiguration.fieldsConfig.columns.map(
        col => <KeyValuePair>{ key: col.field, value: col.header }
      );
      this.sortFieldValue = this.displayedColumns[0].key;

      this.initTableErrorParam();
    }
  }

  initTableErrorParam() {
    if (this.crudConfiguration) {
      this.crudConfigurationError = clone(this.crudConfiguration);
      this.crudConfigurationError.fieldsConfig.columns.push(
        Object.assign(new BiaFieldConfig('sErrors', 'sErrors'), {
          isEditable: false,
          type: PropType.String,
        })
      );

      this.displayedColumnErrors =
        this.crudConfigurationError.fieldsConfig.columns.map(
          col => <KeyValuePair>{ key: col.field, value: col.header }
        );
    }
  }

  onFileSelected(event: any) {
    this.fileSelected.next(event);
  }

  onCancel() {
    this.cancel.next();
  }

  onSave() {
    let toSaves: any[] = [];

    if (this.deleteChecked === true) {
      toSaves = toSaves.concat(this.bulkData.toDeletes);
    }

    if (this.insertChecked === true) {
      toSaves = toSaves.concat(this.bulkData.toInserts);
    }

    if (this.updateChecked === true) {
      toSaves = toSaves.concat(this.bulkData.toUpdates);
    }

    if (toSaves.length > 0) {
      this.save.emit(toSaves);
    }
  }
}
