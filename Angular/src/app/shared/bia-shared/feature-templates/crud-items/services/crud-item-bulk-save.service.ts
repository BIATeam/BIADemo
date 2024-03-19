import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import * as Papa from 'papaparse';
import FileSaver from 'file-saver';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';

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

  constructor(private crudItemService: CrudItemService<T>) {}

  public downloadCsv(columns: string[], fileName: string) {
    this.crudItemService.dasService
      .getList({ endpoint: 'all' })
      .pipe(
        map(planes => {
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
    files: FileList
  ): Observable<BulkSaveData<T>> {
    this.form = form;

    const file = files.item(0);
    const reader = new FileReader();

    return new Observable(observer => {
      reader.onload = (e: any) => {
        const csv = e.target.result;
        this.parseCSV(csv).subscribe((data: BulkSaveData<T>) => {
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

  protected customMapCsvToJson(oldObj: T, newObj: TCsv): string[] {
    return [];
  }

  private parseCSV(csv: string): Observable<BulkSaveData<T>> {
    const result = Papa.parse<TCsv>(csv, {
      header: true,
      dynamicTyping: true,
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
    newObjs: TCsv[],
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
          const found = newObjs.some(newObj => newObj.id === oldObj.id);
          if (!found) {
            oldObj.dtoState = DtoState.Deleted;
            toDeletes.push(oldObj);
          }

          // TO UPDATE
          const newObj = newObjs.find(newObj => newObj.id === oldObj.id);
          if (oldObj && newObj) {
            const hasDifferentProperties = Object.keys(oldObj).some(key => {
              let oldValue = (<any>oldObj)[key];
              let newValue = (<any>newObj)[key];
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
              for (const prop in oldObj) {
                if (newObj.hasOwnProperty(prop)) {
                  Object.assign(oldObj, { [prop]: newObj[prop] });
                }
              }

              const mapErrors = this.customMapCsvToJson(oldObj, newObj);
              const checkObject = this.form.checkObject(oldObj);

              const errors = [...checkObject.errorMessages, ...mapErrors];
              if (errors.length > 0) {
                errorToUpdates.push({ ...newObj, errors: errors });
              } else {
                oldObj.dtoState = DtoState.Modified;
                toUpdates.push(oldObj);
              }
            }
          }
        }

        for (const newObj of newObjs) {
          // TO INSERTS
          if (newObj.id === 0) {
            const oldObj = this.form.checkObject(newObj).element;
            const mapErrors = this.customMapCsvToJson(oldObj, newObj);
            const checkObject = this.form.checkObject(oldObj);

            const errors = [...checkObject.errorMessages, ...mapErrors];
            if (errors.length > 0) {
              errorToInserts.push({ ...newObj, errors: errors });
            } else {
              oldObj.dtoState = DtoState.Added;
              toInserts.push(oldObj);
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
