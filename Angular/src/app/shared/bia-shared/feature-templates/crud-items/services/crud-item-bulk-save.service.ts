import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import * as Papa from 'papaparse';
import FileSaver from 'file-saver';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';

export interface BulkSaveData<T extends BaseDto> {
  toDeletes: T[];
  toInserts: T[];
  toUpdates: T[];
  errorToInserts: T[];
  errorToUpdates: T[];
}

@Injectable({
  providedIn: 'root',
})
export class CrudItemBulkSaveService<T extends BaseDto> {
  private crudItemService: CrudItemService<T>;
  private form: CrudItemFormComponent<T>;

  public downloadCsv(columns: string[]) {
    this.crudItemService.dasService
      .getList({ endpoint: 'all' })
      .subscribe((x: T[]) => {
        let csv = Papa.unparse<T>(x, {
          columns: columns,
        });
        csv = `sep=,\n${csv}`;
        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8' });
        FileSaver.saveAs(blob, 'test.csv');
      });
  }
  public uploadCsv(
    crudItemService: CrudItemService<T>,
    form: CrudItemFormComponent<T>,
    files: FileList
  ): Observable<BulkSaveData<T>> {
    this.crudItemService = crudItemService;
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

  private parseCSV(csv: string): Observable<BulkSaveData<T>> {
    const result = Papa.parse<T>(csv, {
      header: true,
      dynamicTyping: true,
    });

    const allObjs$ = this.crudItemService.dasService.getList({
      endpoint: 'all',
    });
    const toSaves$: Observable<BulkSaveData<T>> = this.check(
      result.data,
      allObjs$
    );

    return toSaves$;
  }

  private check(
    newObjs: T[],
    oldObjs$: Observable<T[]>
  ): Observable<BulkSaveData<T>> {
    return oldObjs$.pipe(
      map((oldObjs: T[]) => {
        const toDeletes: T[] = [];
        const toInserts: T[] = [];
        const toUpdates: T[] = [];

        for (const oldObj of oldObjs) {
          const found = newObjs.some(newObj => newObj.id === oldObj.id);
          if (!found) {
            oldObj.dtoState = DtoState.Deleted;
            toDeletes.push(oldObj);
          }
        }

        for (const newObj of newObjs) {
          const found = oldObjs.some(oldObj => newObj.id === oldObj.id);
          if (!found) {
            newObj.dtoState = DtoState.Added;
            toInserts.push(newObj);
          }
        }

        for (const oldObj of oldObjs) {
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
              Object.assign(oldObj, newObj);
              oldObj.dtoState = DtoState.Modified;
              toUpdates.push(oldObj);
            }
          }
        }

        const bulkSaveData: BulkSaveData<T> = {
          toDeletes: toDeletes,
          toInserts: toInserts.filter(x => this.form.checkObject(x) === true),
          toUpdates: toUpdates.filter(x => this.form.checkObject(x) === true),
          errorToInserts: toInserts.filter(
            x => this.form.checkObject(x) !== true
          ),
          errorToUpdates: toUpdates.filter(
            x => this.form.checkObject(x) !== true
          ),
        };

        return bulkSaveData;
      })
    );
  }
}
