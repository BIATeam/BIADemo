import { DatePipe, NgClass, NgIf } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import {
  Accordion,
  AccordionContent,
  AccordionHeader,
  AccordionPanel,
} from 'primeng/accordion';
import { ButtonDirective } from 'primeng/button';
import { Checkbox } from 'primeng/checkbox';
import { FileUpload } from 'primeng/fileupload';
import { FloatLabel } from 'primeng/floatlabel';
import { Ripple } from 'primeng/ripple';
import { Select } from 'primeng/select';
import { TablePageEvent } from 'primeng/table';
import { AppSettings } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { clone } from 'src/app/shared/bia-shared/utils';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { BiaTableComponent } from '../../../../components/table/bia-table/bia-table.component';
import { CrudConfig } from '../../model/crud-config';
import { ImportParam } from '../../services/crud-item-import.service';

interface FormatExample {
  format: string;
  example: string;
}

@Component({
  selector: 'bia-crud-item-import-form',
  templateUrl: './crud-item-import-form.component.html',
  styleUrls: ['./crud-item-import-form.component.scss'],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    FileUpload,
    ButtonDirective,
    NgIf,
    Select,
    Checkbox,
    Accordion,
    AccordionPanel,
    Ripple,
    AccordionHeader,
    AccordionContent,
    BiaTableComponent,
    NgClass,
    SpinnerComponent,
    TranslateModule,
    FloatLabel,
  ],
})
export class CrudItemImportFormComponent<TDto extends { id: number }> {
  @ViewChild('fileUpload') fileUpload: FileUpload | undefined;

  fillFormDone = false;
  deleteChecked = false;
  updateChecked = false;
  insertChecked = false;
  loading = false;
  hasDate = false;
  dateFormats: FormatExample[];
  timeFormats: FormatExample[];
  form: UntypedFormGroup;
  importDataTablesPageSize = 50;
  pageSizeOptions = [50];

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

  protected _importData: any;
  get importData(): any {
    return this._importData;
  }
  @Input() set importData(value: any) {
    this._importData = value;
    if (value) {
      this.importDataErrorsToDisplayInTable = [...value.errorToSaves];
      this.importDataToInsertToDisplayInTable = [...value.toInserts];
      this.importDataToUpdateToDisplayInTable = [...value.toUpdates];
      this.importDataToDeleteToDisplayInTable = [...value.toDeletes];
    }
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

  protected _importParam: ImportParam = <ImportParam>{};
  get importParam(): ImportParam {
    return this._importParam;
  }
  @Input() set importParam(value: ImportParam) {
    this._importParam = value;
    this.fillForm();
  }

  @Input() canEdit = false;
  @Input() canDelete = false;
  @Input() canAdd = false;
  @Output() save = new EventEmitter<any[]>();
  @Output() cancelled = new EventEmitter<void>();
  @Output() fileSelected = new EventEmitter<File>();
  @Output() changeImportParam = new EventEmitter<ImportParam>();

  importDataErrorsToDisplayInTable: any[] = [];
  importDataToInsertToDisplayInTable: any[] = [];
  importDataToUpdateToDisplayInTable: any[] = [];
  importDataToDeleteToDisplayInTable: any[] = [];

  constructor(
    public formBuilder: UntypedFormBuilder,
    public datepipe: DatePipe
  ) {
    this.initForm();
  }

  protected initForm() {
    this.form = this.formBuilder.group({
      useCurrentView: [this.importParam.useCurrentView, Validators.required],
      dateFormat: [this.importParam.dateFormat, Validators.required],
      timeFormat: [this.importParam.timeFormat, Validators.required],
    });
  }

  fillForm() {
    if (this.importParam && this.fillFormDone !== true) {
      this.form.reset();
      if (this.importParam) {
        this.form.patchValue({ ...this.importParam });
      }
      this.fillFormDone = true;
    }
  }

  initDdlFormatDate() {
    if (this.appSettings && this.appSettings.cultures.length > 0) {
      const dateFormatSets = [
        ...new Set(this.appSettings.cultures.map(x => x.dateFormat)),
      ];

      const timeFormatSets = [
        ...new Set(this.appSettings.cultures.map(x => x.timeFormat)),
      ];

      const exampleDate = new Date(2025, 11, 31, 9, 45);

      this.dateFormats = dateFormatSets.map(dateFormatSet => {
        return <FormatExample>{
          format: dateFormatSet,
          example: this.datepipe.transform(exampleDate, dateFormatSet),
        };
      });

      this.timeFormats = timeFormatSets.map(timeFormatSet => {
        return <FormatExample>{
          format: timeFormatSet,
          example: this.datepipe.transform(exampleDate, timeFormatSet),
        };
      });
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
      this.displayedColumns = this.getVisibleColumnsFromFieldsConfig(
        this.crudConfiguration.fieldsConfig
      );
      this.sortFieldValue = this.displayedColumns[0].key;

      this.initTableErrorParam();
    }
  }

  initTableErrorParam() {
    if (this.crudConfiguration) {
      this.crudConfigurationError = clone(this.crudConfiguration, false);
      this.crudConfigurationError.fieldsConfig.columns = [
        ...this.crudConfiguration.fieldsConfig.columns,
        Object.assign(
          new BiaFieldConfig<TDto>(
            <keyof TDto & string>'sErrors',
            'bia.errors'
          ),
          {
            isEditable: false,
            type: PropType.String,
          }
        ),
      ];

      this.displayedColumnErrors = this.getVisibleColumnsFromFieldsConfig(
        this.crudConfigurationError.fieldsConfig
      );
    }
  }

  protected getVisibleColumnsFromFieldsConfig(
    fields: BiaFieldsConfig<TDto>
  ): KeyValuePair[] {
    return fields.columns
      .filter(col => col.isVisibleInTable)
      .map(col => <KeyValuePair>{ key: col.field, value: col.header });
  }

  onFileSelected() {
    this._importData = null;
  }

  onCancel() {
    this.cancelled.next();
  }

  onSave() {
    let toSaves: any[] = [];

    if (this.deleteChecked === true) {
      toSaves = toSaves.concat(this.importData.toDeletes);
    }

    if (this.insertChecked === true) {
      toSaves = toSaves.concat(this.importData.toInserts);
    }

    if (this.updateChecked === true) {
      toSaves = toSaves.concat(this.importData.toUpdates);
    }

    if (toSaves.length > 0) {
      this.save.emit(toSaves);
    }
  }

  onApply() {
    const file = this.fileUpload?.files[0];
    if (file) {
      this.loading = true;
      this.fileSelected.next(file);
    }
  }

  canUseDelete(): boolean {
    return (
      this.canDelete === true &&
      this.crudConfiguration.importMode?.useDelete === true
    );
  }

  canUseInsert(): boolean {
    return (
      this.canAdd === true &&
      this.crudConfiguration.importMode?.useInsert === true
    );
  }

  canUseUpdate(): boolean {
    return (
      this.canEdit === true &&
      this.crudConfiguration.importMode?.useUpdate === true
    );
  }

  onSubmit() {
    if (this.form.valid) {
      this._importParam = <ImportParam>this.form.value;
      this.changeImportParam.next(this._importParam);
    }
  }

  onTablePageChanges(
    tablePageEvent: TablePageEvent,
    importDataElementsToDisplayInTable: any[],
    importDataElementsReference: any[]
  ) {
    importDataElementsToDisplayInTable.splice(
      0,
      importDataElementsToDisplayInTable.length
    );
    importDataElementsToDisplayInTable.push(
      ...importDataElementsReference.filter(
        (_: any, index: number) =>
          index >= tablePageEvent.first &&
          index < tablePageEvent.first + tablePageEvent.rows
      )
    );
  }
}
