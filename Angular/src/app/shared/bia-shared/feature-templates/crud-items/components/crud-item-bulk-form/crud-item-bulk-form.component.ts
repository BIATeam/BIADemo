import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { FileUpload } from 'primeng/fileupload';
import { DateHelperService } from 'src/app/core/bia-core/services/date-helper.service';
import { AppSettings } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { clone } from 'src/app/shared/bia-shared/utils';
import { CrudConfig } from '../../model/crud-config';
import { BulkParam } from '../../services/crud-item-bulk.service';

@Component({
  selector: 'bia-crud-item-bulk-form',
  templateUrl: './crud-item-bulk-form.component.html',
  styleUrls: ['./crud-item-bulk-form.component.scss'],
})
export class CrudItemBulkFormComponent<TDto extends { id: number }> {
  @ViewChild('fileUpload') fileUpload: FileUpload;

  fillFormDone = false;
  deleteChecked = false;
  updateChecked = false;
  insertChecked = false;
  loading = false;
  hasDate = false;
  dateFormats: string[];
  timeFormats: string[];
  form: UntypedFormGroup;

  displayedColumns: KeyValuePair[];
  displayedColumnErrors: KeyValuePair[];
  fieldsConfigErrors: BiaFieldsConfig<TDto>;
  sortFieldValue = '';
  protected crudConfigurationError: CrudConfig<TDto>;

  protected _crudConfiguration: CrudConfig<TDto>;
  get crudConfiguration(): CrudConfig<TDto> {
    return this._crudConfiguration;
  }
  @Input() set crudConfiguration(value: CrudConfig<TDto>) {
    this._crudConfiguration = value;
    this.initHasDate();
    this.initTableParam();
  }

  protected _bulkData: any;
  get bulkData(): any {
    return this._bulkData;
  }
  @Input() set bulkData(value: any) {
    this._bulkData = value;
    if (this.fileUpload) {
      this.fileUpload.clear();
    }
    this.loading = false;
  }

  protected _appSettings: AppSettings | null;
  get appSettings(): AppSettings | null {
    return this._appSettings;
  }
  @Input() set appSettings(value: AppSettings | null) {
    this._appSettings = value;
    this.initDdlFormatDate();
  }

  protected _bulkParam: BulkParam = <BulkParam>{};
  get bulkParam(): BulkParam {
    return this._bulkParam;
  }
  @Input() set bulkParam(value: BulkParam) {
    this._bulkParam = value;
    this.fillForm();
  }

  @Input() canEdit = false;
  @Input() canDelete = false;
  @Input() canAdd = false;
  @Output() save = new EventEmitter<any[]>();
  @Output() cancel = new EventEmitter<void>();
  @Output() fileSelected = new EventEmitter<any>();
  @Output() changeBulkParam = new EventEmitter<BulkParam>();

  constructor(public formBuilder: UntypedFormBuilder) {
    this.initForm();
  }

  protected initForm() {
    this.form = this.formBuilder.group({
      useCurrentView: [this.bulkParam.useCurrentView, Validators.required],
      dateFormat: [this.bulkParam.dateFormat, Validators.required],
      timeFormat: [this.bulkParam.timeFormat, Validators.required],
    });
  }

  fillForm() {
    if (this.bulkParam && this.fillFormDone !== true) {
      this.form.reset();
      if (this.bulkParam) {
        this.form.patchValue({ ...this.bulkParam });
      }
      this.fillFormDone = true;
    }
  }

  initDdlFormatDate() {
    if (this.appSettings && this.appSettings.cultures.length > 0) {
      this.appSettings.cultures.map(x => x.dateFormat);
      this.dateFormats = [
        DateHelperService.dateFormatIso8601,
        ...new Set(this.appSettings.cultures.map(x => x.dateFormat)), // new Set => Distinct()
      ];
      this.timeFormats = [
        ...new Set(this.appSettings.cultures.map(x => x.timeFormat)),
      ];
    }
  }

  initHasDate() {
    if (this.crudConfiguration) {
      this.hasDate =
        this.crudConfiguration.fieldsConfig.columns.some(
          x => x.type === PropType.Date || x.type === PropType.DateTime
        ) === true;
    }
  }

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
      this.crudConfigurationError = clone(this.crudConfiguration, false);
      this.crudConfigurationError.fieldsConfig.columns.push(
        Object.assign(
          new BiaFieldConfig<TDto>(
            <keyof TDto & string>'sErrors',
            'bia.errors'
          ),
          {
            isEditable: false,
            type: PropType.String,
          }
        )
      );

      this.displayedColumnErrors =
        this.crudConfigurationError.fieldsConfig.columns.map(
          col => <KeyValuePair>{ key: col.field, value: col.header }
        );
    }
  }

  onFileSelected(event: any) {
    this.loading = true;
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

  canUseDelete(): boolean {
    return (
      this.canDelete === true &&
      this.crudConfiguration.bulkMode?.useDelete === true
    );
  }

  canUseInsert(): boolean {
    return (
      this.canAdd === true &&
      this.crudConfiguration.bulkMode?.useInsert === true
    );
  }

  canUseUpdate(): boolean {
    return (
      this.canEdit === true &&
      this.crudConfiguration.bulkMode?.useUpdate === true
    );
  }

  onSubmit() {
    if (this.form.valid) {
      this._bulkParam = <BulkParam>this.form.value;
      this.changeBulkParam.next(this._bulkParam);
    }
  }
}
