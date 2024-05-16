import { Injectable } from '@angular/core';
import { Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import * as Papa from 'papaparse';
import FileSaver from 'file-saver';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { CrudConfig } from '../model/crud-config';
import { TranslateService } from '@ngx-translate/core';
import { DictOptionDto } from '../../../components/table/bia-table/dict-option-dto';
import { BiaFieldConfig, PropType } from '../../../model/bia-field-config';
import { clone, isEmpty } from '../../../utils';

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
    private crudItemService: CrudItemService<T>,
    private translateService: TranslateService
  ) {}

  public downloadCsv(columns: string[], fileName: string) {
    this.crudItemService.dasService
      .getList({ endpoint: 'all' })
      .pipe(
        map((planes: T[]) => {
          return this.customMapJsonToCsv(planes);
        })
      )
      .subscribe((x: T[]) => {
        let csv = Papa.unparse<T>(x, {
          columns: columns,
        });
        csv = `sep=,\n${csv}`;
        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8' });
        FileSaver.saveAs(blob, fileName + '.csv');
      });
  }

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

  protected customMapJsonToCsv(planes: T[]): T[] {
    return planes;
  }

  protected parseCSVCustom(
    csvObjs: T[],
    crudConfig: CrudConfig,
    separator = ' - '
  ): Observable<T[]> {
    return this.crudItemService.optionsService.dictOptionDtos$.pipe(
      map((dictOptionDtos: DictOptionDto[]) => {
        for (const column of crudConfig.fieldsConfig.columns) {
          if (column.type === PropType.String) {
            this.parseCSVString(csvObjs, column.field);
          } else if (
            column.type === PropType.Date ||
            column.type === PropType.DateTime
          ) {
            this.parseCSVDate(csvObjs, column);
          } else if (column.type === PropType.Boolean) {
            this.parseCSVBoolean(csvObjs, column.field);
          } else if (column.type === PropType.Number) {
            this.parseCSVNumber(csvObjs, column.field);
          } else if (
            column.type === PropType.OneToMany ||
            column.type === PropType.ManyToMany
          ) {
            const dictOptionDto = dictOptionDtos.find(
              x => x.key === column.field
            );
            // If there is an dictOptionDto for this property
            if (dictOptionDto) {
              // So, it's an OptionDto. In the csv, we have the display value (string).
              // You must find the OptionDto and correctly fill the property.(string -> OptionDto)
              if (column.type === PropType.ManyToMany) {
                this.parseCSVManyToMany(
                  csvObjs,
                  column.field,
                  separator,
                  dictOptionDto
                );
              } else if (column.type === PropType.OneToMany) {
                this.parseCSVOneToMany(csvObjs, column.field, dictOptionDto);
              }
            }
          }
        }
        return csvObjs;
      })
    );
  }

  private cleanCSVFormat(csvData: string): string {
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

  protected parseCSVString(csvObjs: T[], field: string) {
    const regex1 = /"=""(.+?)"""/g;
    const regex2 = /=""(.+?)""/g;

    csvObjs.map(csvObj => {
      csvObj[<keyof typeof csvObj>field] = <any>String(
        csvObj[<keyof typeof csvObj>field]
      )
        .replace(regex1, (match, p1) => p1)
        .replace(regex2, (match, p2) => p2);
    });
  }

  protected parseCSVDate(csvObjs: T[], column: BiaFieldConfig) {
    csvObjs.map(csvObj => {
      if (<any>csvObj[<keyof typeof csvObj>column.field] instanceof Date !== true) {
        let value = String(csvObj[<keyof typeof csvObj>column.field]);
        if (column.type === PropType.Date) {
          value += ' 00:00';
        }
        csvObj[<keyof typeof csvObj>column.field] = <any>new Date(value);
      }
    });
  }

  protected parseCSVBoolean(csvObjs: T[], field: string) {
    csvObjs.map(csvObj => {
      if (<any>csvObj[<keyof typeof csvObj>field] instanceof Boolean !== true) {
        csvObj[<keyof typeof csvObj>field] = <any>(
          (String(csvObj[<keyof typeof csvObj>field]) === 'X')
        );
      }
    });
  }

  protected parseCSVNumber(csvObjs: T[], field: string) {
    csvObjs.map(csvObj => {
      if (<any>csvObj[<keyof typeof csvObj>field] instanceof Number !== true) {
        csvObj[<keyof typeof csvObj>field] = <any>(
          Number(csvObj[<keyof typeof csvObj>field])
        );
      }
    });
  }

  protected parseCSVOneToMany(
    csvObjs: T[],
    field: string,
    dictOptionDto: DictOptionDto
  ) {
    csvObjs.map(csvObj => {
      const csvValue = csvObj[<keyof typeof csvObj>field]?.toString().trim();
      const optionDto =
        dictOptionDto.value.find(
          x =>
            x.display === csvObj[<keyof typeof csvObj>field]?.toString().trim()
        ) ?? null;
      if (
        (isEmpty(csvValue) === true && isEmpty(optionDto) === true) ||
        (isEmpty(csvValue) !== true && isEmpty(optionDto) !== true)
      ) {
        csvObj[<keyof typeof csvObj>field] = <any>optionDto;
      } else {
        this.bulkSaveData.errorToSaves.push({
          obj: csvObj,
          errors: [field + ' not found'],
        });
      }
    });
  }

  protected parseCSVManyToMany(
    csvObjs: T[],
    field: string,
    separator: string,
    dictOptionDto: DictOptionDto
  ) {
    csvObjs.map(csvObj => {
      const csvValues = csvObj[<keyof typeof csvObj>field]
        ?.toString()
        .trim()
        .split(separator);
      const optionDtos =
        dictOptionDto.value.filter(
          x => csvValues?.some(y => y === x.display) === true
        ) ?? null;

      if (csvValues?.length !== optionDtos.length) {
        this.bulkSaveData.errorToSaves.push({
          obj: csvObj,
          errors: [field + ' not found'],
        });
      } else {
        csvObj[<keyof typeof csvObj>field] = <any>optionDtos;
      }
    });
  }

  private parseCSV(
    csv: string,
    crudConfig: CrudConfig
  ): Observable<BulkSaveData<T>> {
    const cleanedCSVData = this.cleanCSVFormat(csv);

    const columnMapping = crudConfig.fieldsConfig.columns.reduce(
      (map: { [key: string]: string }, obj) => {
        map[this.translateService.instant(obj.header)] = obj.field;
        return map;
      },
      {}
    );

    const result = Papa.parse<T>(cleanedCSVData, {
      skipEmptyLines: 'greedy',
      header: true,
      dynamicTyping: true,
      transformHeader: header => columnMapping[header],
    });

    const resultData$ = this.parseCSVCustom(result.data, crudConfig);

    const allObjs$ = this.crudItemService.dasService.getList({
      endpoint: 'all',
    });
    const toSaves$: Observable<BulkSaveData<T>> = this.fillBulkSaveData(
      resultData$,
      allObjs$
    );

    return toSaves$;
  }

  private fillBulkSaveData(
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
            oldObj.dtoState = DtoState.Deleted;
            this.bulkSaveData.toDeletes.push(oldObj);
          }

          // TO UPDATE
          const csvObj = csvObjs.find(csvObj => csvObj.id === oldObj.id);
          if (oldObj && csvObj) {
            const newObj: T = clone(oldObj);
            for (const prop in newObj) {
              if (csvObj.hasOwnProperty(prop)) {
                Object.assign(newObj, { [prop]: csvObj[prop] });
              }
            }

            this.form.setElement(oldObj);
            const checkObject = this.form.checkObject(newObj);

            if (checkObject.errorMessages.length > 0) {
              this.bulkSaveData.errorToSaves.push({
                obj: csvObj,
                errors: checkObject.errorMessages,
              });
            } else if (JSON.stringify(oldObj) !== JSON.stringify(newObj)) {
              checkObject.element.dtoState = DtoState.Modified;
              this.bulkSaveData.toUpdates.push(checkObject.element);
            }
          }
        }

        for (const csvObj of csvObjs) {
          // TO INSERTS
          if (isEmpty(csvObj.id) === true || csvObj.id === 0) {
            const newObj = this.form.checkObject(csvObj).element;
            const checkObject = this.form.checkObject(newObj);

            if (checkObject.errorMessages.length > 0) {
              this.bulkSaveData.errorToSaves.push({
                obj: csvObj,
                errors: checkObject.errorMessages,
              });
            } else {
              newObj.dtoState = DtoState.Added;
              this.bulkSaveData.toInserts.push(newObj);
            }
          }
        }

        return this.bulkSaveData;
      })
    );
  }

  private initBulkSaveData() {
    this.bulkSaveData = <BulkSaveData<T>>{
      toDeletes: [],
      toInserts: [],
      toUpdates: [],
      errorToSaves: [],
    };
  }
}
