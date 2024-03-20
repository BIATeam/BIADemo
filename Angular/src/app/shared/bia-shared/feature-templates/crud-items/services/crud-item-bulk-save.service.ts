import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import * as Papa from 'papaparse';
import FileSaver from 'file-saver';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { CrudConfig } from '../model/crud-config';
import { TranslateService } from '@ngx-translate/core';

export interface BulkSaveDataError extends BaseDto {
  errors: string[];
}

export interface BulkSaveData<T extends BaseDto> {
  toDeletes: T[];
  toInserts: T[];
  toUpdates: T[];
  errorToInserts: BulkSaveDataError[];
  errorToUpdates: BulkSaveDataError[];
}

@Injectable({
  providedIn: 'root',
})
export class CrudItemBulkSaveService<T extends BaseDto, TCsv extends T> {
  protected form: CrudItemFormComponent<T>;

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

    const file = files.item(0);
    const reader = new FileReader();

    const columnMapping = crudConfig.fieldsConfig.columns.reduce(
      (map: { [key: string]: string }, obj) => {
        map[this.translateService.instant(obj.header)] = obj.field;
        return map;
      },
      {}
    );

    return new Observable(observer => {
      reader.onload = (e: any) => {
        const csv = e.target.result;
        this.parseCSV(csv, columnMapping).subscribe((data: BulkSaveData<T>) => {
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

  protected customMapCsvToJson(oldObj: T, csvObj: TCsv): string[] {
    return [];
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
    columnMapping: { [key: string]: string }
  ): Observable<BulkSaveData<T>> {
    const cleanedCSVData = this.cleanCSVFormat(csv);

    // TODO: DEPLACER LE customMapCsvToJson ICI
    const result = Papa.parse<TCsv>(cleanedCSVData, {
      skipEmptyLines: 'greedy',
      header: true,
      dynamicTyping: true,
      transformHeader: header => columnMapping[header],
    });

    const allObjs$ = this.crudItemService.dasService.getList({
      endpoint: 'all',
    });
    const toSaves$: Observable<BulkSaveData<T>> = this.fillBulkSaveData(
      result.data,
      allObjs$
    );

    return toSaves$;
  }

  private fillBulkSaveData(
    csvObjs: TCsv[],
    oldObjs$: Observable<T[]>
  ): Observable<BulkSaveData<T>> {
    return oldObjs$.pipe(
      map((oldObjs: T[]) => {
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
            const hasDifferentProperties = Object.keys(oldObj).some(key => {
              let oldValue = (<any>oldObj)[key];
              let newValue = (<any>csvObj)[key];
              if (newValue === undefined) {
                return false;
              }
              if (oldValue instanceof Date === true) {
                oldValue = oldValue?.toISOString();
                newValue = newValue?.toISOString();
              }
              return newValue !== oldValue;
            });
            if (hasDifferentProperties) {
              const newObj: T = { ...oldObj };
              for (const prop in newObj) {
                if (csvObj.hasOwnProperty(prop)) {
                  Object.assign(newObj, { [prop]: csvObj[prop] });
                }
              }

              const mapErrors = this.customMapCsvToJson(newObj, csvObj);
              const checkObject = this.form.checkObject(newObj);

              const errors = [...checkObject.errorMessages, ...mapErrors];
              if (errors.length > 0) {
                errorToUpdates.push({ ...csvObj, errors: errors });
              } else {
                newObj.dtoState = DtoState.Modified;
                toUpdates.push(newObj);
              }
            }
          }
        }

        for (const csvObj of csvObjs) {
          // TO INSERTS
          if (csvObj.id === 0) {
            const newObj = this.form.checkObject(csvObj).element;
            const mapErrors = this.customMapCsvToJson(newObj, csvObj);
            const checkObject = this.form.checkObject(newObj);

            const errors = [...checkObject.errorMessages, ...mapErrors];
            if (errors.length > 0) {
              errorToInserts.push({ ...csvObj, errors: errors });
            } else {
              newObj.dtoState = DtoState.Added;
              toInserts.push(newObj);
            }
          }
        }

        const bulkSaveData: BulkSaveData<T> = {
          toDeletes: toDeletes,
          toInserts: toInserts,
          toUpdates: toUpdates,
          errorToInserts: errorToInserts,
          errorToUpdates: errorToUpdates,
        };

        return bulkSaveData;
      })
    );
  }
}
