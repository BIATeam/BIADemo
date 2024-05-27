import { Injectable } from '@angular/core';
import { Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import * as Papa from 'papaparse';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { CrudConfig } from '../model/crud-config';
import { TranslateService } from '@ngx-translate/core';
import { DictOptionDto } from '../../../components/table/bia-table/dict-option-dto';
import { BiaFieldConfig, PropType } from '../../../model/bia-field-config';
import { clone, isEmpty } from '../../../utils';
import { DateHelperService } from 'src/app/core/bia-core/services/date-helper.service';

export interface BulkSaveDataError<T extends BaseDto> {
  obj: T;
  errors: string[];
}

export interface BulkSaveData<T extends BaseDto> {
  toDeletes: T[];
  toInserts: T[];
  toUpdates: T[];
  errorToSaves: BulkSaveDataError<T>[];
}

@Injectable({
  providedIn: 'root',
})
export class CrudItemBulkSaveService<T extends BaseDto> {
  protected form: CrudItemFormComponent<T>;
  protected bulkSaveData: BulkSaveData<T>;

  constructor(
    protected crudItemService: CrudItemService<T>,
    protected translateService: TranslateService
  ) {}

  public uploadCsv(
    form: CrudItemFormComponent<T>,
    files: FileList,
    crudConfig: CrudConfig
  ): Observable<BulkSaveData<T>> {
    this.form = form;
    this.initBulkSaveData();
    const file = files.item(0);
    const reader = new FileReader();

    return new Observable(observer => {
      reader.onload = (e: any) => {
        const csv = e.target.result;
        this.parseCSV(csv, crudConfig).subscribe((data: BulkSaveData<T>) => {
          observer.next(data);
          observer.complete();
        });
      };

      if (file) {
        reader.readAsText(file);
      }
    });
  }

  protected parseCSV(
    csv: string,
    crudConfig: CrudConfig
  ): Observable<BulkSaveData<T>> {
    const cleanedCSVData = this.cleanCSVFormat(csv);
    const columnMapping = this.getColumnMapping(crudConfig);

    const result = Papa.parse<T>(cleanedCSVData, {
      skipEmptyLines: 'greedy',
      header: true,
      dynamicTyping: true,
      transformHeader: header => columnMapping[header],
    });

    const resultData$ = this.parseCSVBia(result.data, crudConfig);

    const allObjs$ = this.crudItemService.dasService.getList({
      endpoint: 'all',
    });

    const toSaves$: Observable<BulkSaveData<T>> = this.fillBulkSaveData(
      resultData$,
      allObjs$
    );

    return toSaves$;
  }

  protected getColumnMapping(crudConfig: CrudConfig) {
    return crudConfig.fieldsConfig.columns.reduce(
      (map: { [key: string]: string }, obj) => {
        map[this.translateService.instant(obj.header)] = obj.field;
        return map;
      },
      {}
    );
  }

  protected parseCSVBia(csvObjs: T[], crudConfig: CrudConfig): Observable<T[]> {
    return this.crudItemService.optionsService.dictOptionDtos$.pipe(
      map((dictOptionDtos: DictOptionDto[]) => {
        csvObjs.map(csvObj => {
          crudConfig.fieldsConfig.columns.map(column => {
            const csvValue: any = csvObj[<keyof typeof csvObj>column.field];
            if (column.type === PropType.String) {
              this.parseCSVString(csvObj, column);
            } else if (
              column.type === PropType.Date &&
              Object(csvValue) instanceof Date !== true
            ) {
              this.parseCSVDate(csvObj, column);
            } else if (
              column.type === PropType.DateTime &&
              Object(csvValue) instanceof Date !== true
            ) {
              this.parseCSVDateTime(csvObj, column);
            } else if (
              column.type === PropType.Boolean &&
              Object(csvValue) instanceof Boolean !== true
            ) {
              this.parseCSVBoolean(csvObj, column);
            } else if (
              column.type === PropType.Number &&
              Object(csvValue) instanceof Number !== true
            ) {
              this.parseCSVNumber(csvObj, column);
            } else if (column.type === PropType.OneToMany) {
              this.parseCSVOneToMany(csvObj, column, dictOptionDtos);
            } else if (column.type === PropType.ManyToMany) {
              this.parseCSVManyToMany(csvObj, column, dictOptionDtos);
            }
          });
        });

        return csvObjs;
      })
    );
  }

  protected cleanCSVFormat(csvData: string): string {
    // Check if the first line starts with "sep="
    const firstLine = csvData.substring(0, csvData.indexOf('\n'));
    if (firstLine.startsWith('sep=')) {
      // The CSV uses a custom delimiter, remove the first line
      csvData = csvData.substring(csvData.indexOf('\n') + 1);
    }

    // Use a regular expression to detect formatted strings
    const regex = /"=""(.+?)"""/g;

    // Replace each occurrence of formatted strings with the desired values
    const cleanedData = csvData.replace(regex, (match, p1) => p1);

    return cleanedData;
  }

  protected parseCSVString(csvObj: T, column: BiaFieldConfig) {
    const regex1 = /"=""(.+?)"""/g;
    const regex2 = /=""(.+?)""/g;

    csvObj[<keyof typeof csvObj>column.field] = <any>String(
      csvObj[<keyof typeof csvObj>column.field]
    )
      .replace(regex1, (match, p1) => p1)
      .replace(regex2, (match, p2) => p2)
      .trim();
  }

  protected parseCSVDateAndDateTime(csvObj: T, column: BiaFieldConfig) {
    const csvValue = csvObj[<keyof typeof csvObj>column.field]
      ?.toString()
      .trim();

    if (isEmpty(csvValue)) {
      csvObj[<keyof typeof csvObj>column.field] = <any>null;
    } else {
      let dateString = String(csvObj[<keyof typeof csvObj>column.field]);
      const timePattern = /\d{1,2}:\d{1,2}/;

      if (!timePattern.test(dateString)) {
        dateString += ' 00:00';
      }
      const date: Date | null = DateHelperService.parseDate(dateString);
      if (date) {
        csvObj[<keyof typeof csvObj>column.field] = <any>date;
      } else {
        this.AddErrorToSave(
          csvObj,
          column.field + ': unsupported date format: ' + csvValue
        );
      }
    }
  }

  protected parseCSVDate(csvObj: T, column: BiaFieldConfig) {
    return this.parseCSVDateAndDateTime(csvObj, column);
  }

  protected parseCSVDateTime(csvObj: T, column: BiaFieldConfig) {
    return this.parseCSVDateAndDateTime(csvObj, column);
  }

  protected parseCSVBoolean(csvObj: T, column: BiaFieldConfig) {
    csvObj[<keyof typeof csvObj>column.field] = <any>(
      (String(csvObj[<keyof typeof csvObj>column.field])?.toUpperCase() === 'X')
    );
  }

  protected parseCSVNumber(csvObj: T, column: BiaFieldConfig) {
    const csvValue = csvObj[<keyof typeof csvObj>column.field]
      ?.toString()
      .trim();

    const number = Number(csvValue);
    if (isNaN(number)) {
      this.AddErrorToSave(
        csvObj,
        column.field + ': unsupported number format: ' + csvValue
      );
    } else {
      csvObj[<keyof typeof csvObj>column.field] = <any>number;
    }
  }

  protected parseCSVOneToMany(
    csvObj: T,
    column: BiaFieldConfig,
    dictOptionDtos: DictOptionDto[]
  ) {
    const csvValue = csvObj[<keyof typeof csvObj>column.field]
      ?.toString()
      .trim();

    if (isEmpty(csvValue) === true) {
      csvObj[<keyof typeof csvObj>column.field] = <any>null;
    } else {
      const dictOptionDto = dictOptionDtos.find(x => x.key === column.field);
      const optionDto =
        dictOptionDto?.value.find(x => x.display === csvValue) ?? null;

      if (isEmpty(optionDto) !== true) {
        csvObj[<keyof typeof csvObj>column.field] = <any>optionDto;
      } else {
        this.AddErrorToSave(csvObj, column.field + ' not found: ' + csvValue);
      }
    }
  }

  protected parseCSVManyToMany(
    csvObj: T,
    column: BiaFieldConfig,
    dictOptionDtos: DictOptionDto[]
  ) {
    const csvValue = csvObj[<keyof typeof csvObj>column.field]
      ?.toString()
      .trim();

    if (isEmpty(csvValue) === true) {
      csvObj[<keyof typeof csvObj>column.field] = <any>null;
    } else {
      const csvValues = csvObj[<keyof typeof csvObj>column.field]
        ?.toString()
        .trim()
        .split('-');
      const dictOptionDto = dictOptionDtos.find(x => x.key === column.field);
      const optionDtos =
        dictOptionDto?.value.filter(
          x => csvValues?.some(y => y.trim() === x.display) === true
        ) ?? null;

      if (csvValues?.length !== optionDtos?.length) {
        this.AddErrorToSave(
          csvObj,
          column.field +
            ' not found: ' +
            csvObj[<keyof typeof csvObj>column.field]
        );
      } else {
        csvObj[<keyof typeof csvObj>column.field] = <any>optionDtos;
      }
    }
  }

  protected fillBulkSaveData(
    csvObjs$: Observable<T[]>,
    oldObjs$: Observable<T[]>
  ): Observable<BulkSaveData<T>> {
    return combineLatest([csvObjs$, oldObjs$]).pipe(
      map(([csvObjs, oldObjs]) => {
        // Remove objects in error.
        csvObjs = csvObjs.filter(
          x => !this.bulkSaveData.errorToSaves.map(y => y.obj).includes(x)
        );

        for (const oldObj of oldObjs) {
          // TO DELETE
          const found = csvObjs.some(csvObj => csvObj.id === oldObj.id);
          if (!found) {
            this.fillToDeletes(oldObj);
          }

          // TO UPDATE
          const csvObj = csvObjs.find(csvObj => csvObj.id === oldObj.id);
          if (oldObj && csvObj) {
            this.fillToUpdates(oldObj, csvObj);
          }
        }

        for (const csvObj of csvObjs) {
          // TO INSERTS
          if (isEmpty(csvObj.id) === true || csvObj.id === 0) {
            this.fillToInserts(csvObj);
          }
        }

        return this.bulkSaveData;
      })
    );
  }

  protected fillToDeletes(oldObj: T) {
    oldObj.dtoState = DtoState.Deleted;
    this.bulkSaveData.toDeletes.push(oldObj);
  }

  protected fillToUpdates(oldObj: T, csvObj: T) {
    const newObj: T = clone(oldObj);
    for (const prop in newObj) {
      if (csvObj.hasOwnProperty(prop)) {
        Object.assign(newObj, { [prop]: csvObj[prop] });
      }
    }

    this.form.setElement(oldObj);
    const checkObject = this.form.checkObject(newObj);

    if (checkObject.errorMessages.length > 0) {
      this.AddErrorsToSave(csvObj, checkObject.errorMessages);
    } else if (JSON.stringify(oldObj) !== JSON.stringify(newObj)) {
      checkObject.element.dtoState = DtoState.Modified;
      this.bulkSaveData.toUpdates.push(checkObject.element);
    }
  }

  protected fillToInserts(csvObj: T) {
    this.form.setElement(<T>{});
    const checkObject = this.form.checkObject(csvObj);

    if (checkObject.errorMessages.length > 0) {
      this.AddErrorsToSave(csvObj, checkObject.errorMessages);
    } else {
      checkObject.element.dtoState = DtoState.Added;
      this.bulkSaveData.toInserts.push(checkObject.element);
    }
  }

  protected initBulkSaveData() {
    this.bulkSaveData = <BulkSaveData<T>>{
      toDeletes: [],
      toInserts: [],
      toUpdates: [],
      errorToSaves: [],
    };
  }

  protected AddErrorToSave(obj: T, errorMessage: string) {
    const existingErrorToSave = this.bulkSaveData.errorToSaves.find(
      entry => entry.obj === obj
    );
    if (existingErrorToSave) {
      existingErrorToSave.errors.push(errorMessage);
    } else {
      this.bulkSaveData.errorToSaves.push({
        obj: obj,
        errors: [errorMessage],
      });
    }
  }

  protected AddErrorsToSave(obj: T, errorMessages: string[]) {
    const existingErrorToSave = this.bulkSaveData.errorToSaves.find(
      entry => entry.obj === obj
    );
    if (existingErrorToSave) {
      existingErrorToSave.errors.concat(errorMessages);
    } else {
      this.bulkSaveData.errorToSaves.push({
        obj: obj,
        errors: errorMessages,
      });
    }
  }
}
