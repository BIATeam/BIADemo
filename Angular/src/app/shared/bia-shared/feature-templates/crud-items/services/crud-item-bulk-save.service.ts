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
import { PropType } from '../../../model/bia-field-config';

export interface BulkSaveDataError extends BaseDto {
  errors: string[];
}

export interface BulkSaveData<T extends BaseDto> {
  toDeletes: T[];
  toInserts: T[];
  toUpdates: T[];
  errorToSaves: BulkSaveDataError[];
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

  protected fillOptionDto(
    csvObjs: T[],
    crudConfig: CrudConfig
  ): Observable<T[]> {
    return this.crudItemService.optionsService.dictOptionDtos$.pipe(
      map((dictOptionDtos: DictOptionDto[]) => {
        for (const column of crudConfig.fieldsConfig.columns) {
          // value = Property name
          const dictOptionDto = dictOptionDtos.find(
            x => x.key === column.field
          );
          // If there is an dictOptionDto for this property
          if (dictOptionDto) {
            // So, it's an OptionDto. In the csv, we have the display value (string).
            // You must find the OptionDto and correctly fill the property.(string -> OptionDto)
            if (column.type === PropType.ManyToMany) {
              csvObjs.map(csvObj => {
                const csvValues = csvObj[<keyof typeof csvObj>column.field]
                  ?.toString()
                  .trim()
                  .split(' - ');
                const optionDtos =
                  dictOptionDto.value.filter(
                    x => csvValues?.some(y => y === x.display) === true
                  ) ?? null;

                if (csvValues?.length !== optionDtos.length) {
                  // this.bulkSaveData.errorToSaves.push({ ...csvObjs, errors });
                } else {
                  csvObj[<keyof typeof csvObj>column.field] = <any>optionDtos;
                }
              });
            } else if (column.type === PropType.OneToMany) {
              csvObjs.map(csvObj => {
                const csvValue = csvObj[<keyof typeof csvObj>column.field]
                  ?.toString()
                  .trim();
                const optionDto =
                  dictOptionDto.value.find(
                    x =>
                      x.display ===
                      csvObj[<keyof typeof csvObj>column.field]
                        ?.toString()
                        .trim()
                  ) ?? null;
                const isNotEmpty = csvValue && csvValue.length > 0;
                const hasOptionDto = optionDto && optionDto?.display.length > 0;
                if (
                  (isNotEmpty === true && hasOptionDto === true) ||
                  (isNotEmpty !== true && hasOptionDto !== true)
                ) {
                  csvObj[<keyof typeof csvObj>column.field] = <any>optionDto;
                } else {
                  // this.bulkSaveData.errorToSaves.push({ ...csvObjs, errors });
                }
              });
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

    const resultData$ = this.fillOptionDto(result.data, crudConfig);

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
        const toDeletes: T[] = [];
        const toInserts: T[] = [];
        const toUpdates: T[] = [];
        const errorToInserts: BulkSaveDataError[] = [];
        const errorToUpdates: BulkSaveDataError[] = [];

        for (const oldObj of oldObjs) {
          // TO DELETE
          const found = csvObjs.some(csvObj => csvObj.id === oldObj.id);
          if (!found) {
            oldObj.dtoState = DtoState.Deleted;
            toDeletes.push(oldObj);
          }

          // TO UPDATE
          const csvObj = csvObjs.find(csvObj => csvObj.id === oldObj.id);
          if (oldObj && csvObj) {
            if (this.compareObjects(oldObj, csvObj) !== true) {
              const newObj: T = { ...oldObj };
              for (const prop in newObj) {
                if (csvObj.hasOwnProperty(prop)) {
                  Object.assign(newObj, { [prop]: csvObj[prop] });
                }
              }

              const checkObject = this.form.checkObject(newObj);

              if (checkObject.errorMessages.length > 0) {
                errorToUpdates.push({
                  ...csvObj,
                  errors: checkObject.errorMessages,
                });
              } else {
                newObj.dtoState = DtoState.Modified;
                toUpdates.push(newObj);
              }
            }
          }
        }

        for (const csvObj of csvObjs) {
          // TO INSERTS
          if (parseInt(csvObj.id, 10) === 0) {
            const newObj = this.form.checkObject(csvObj).element;
            const checkObject = this.form.checkObject(newObj);

            if (checkObject.errorMessages.length > 0) {
              errorToInserts.push({
                ...csvObj,
                errors: checkObject.errorMessages,
              });
            } else {
              newObj.dtoState = DtoState.Added;
              toInserts.push(newObj);
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

  private compareObjects(obj1: any, obj2: any): boolean {
    // Check if the objects are of the same type
    if (typeof obj1 !== 'object' || typeof obj2 !== 'object') {
      return false;
    }

    // Check if the objects have the same number of properties
    const props1 = Object.keys(obj1);
    const props2 = Object.keys(obj2);
    if (props1.length !== props2.length) {
      return false;
    }

    // Check each property of obj1
    for (const prop of props1) {
      // Check if the property exists in obj2
      if (!(prop in obj2)) {
        return false;
      }

      // Check if the property values are different
      const val1 = obj1[prop];
      const val2 = obj2[prop];
      if (typeof val1 === 'object' && typeof val2 === 'object') {
        // Recursively compare child objects
        if (!this.compareObjects(val1, val2)) {
          return false;
        }
      } else if (val1 !== val2) {
        return false;
      }
    }

    // If all checks pass, the objects are identical
    return true;
  }
}
