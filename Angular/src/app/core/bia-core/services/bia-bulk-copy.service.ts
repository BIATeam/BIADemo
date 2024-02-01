import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import * as Papa from 'papaparse';
import FileSaver from 'file-saver';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';

@Injectable({
  providedIn: 'root',
})
export class BiaBulkCopyService<T extends BaseDto> {
  private crudItemService: CrudItemService<T>;

  initCrudItemService(crudItemService: CrudItemService<T>) {
    this.crudItemService = crudItemService;
  }
  handleFileInput(files: FileList) {
    const file = files.item(0);
    const reader = new FileReader();

    reader.onload = (e: any) => {
      const csv = e.target.result;
      this.parseCSV(csv);
    };
    if (file) {
      reader.readAsText(file);
    }
  }

  parseCSV(csv: string) {
    const result = Papa.parse<T>(csv, {
      header: true,
      dynamicTyping: true,
      // complete: result => {
      //   result.data.map(plane =>
      //     DateHelperService.fillDate(plane)
      //   );
      // },
    });
    // result.data.forEach(plane => DateHelperService.fillDate(plane));

    const allPlanes$ = this.crudItemService.dasService.getList({
      endpoint: 'all',
    });
    const toSaves$: Observable<T[]> = this.check(result.data, allPlanes$);

    toSaves$
      .pipe(map(x => this.crudItemService.dasService.save({ items: x })))
      .subscribe();
  }

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

  check(newPlanes: T[], oldPlanes$: Observable<T[]>): Observable<T[]> {
    return oldPlanes$.pipe(
      map((oldPlanes: T[]) => {
        const toDeletes: T[] = [];
        const toInserts: T[] = [];
        const toUpdates: T[] = [];

        // Chercher les avions à supprimer
        for (const oldPlane of oldPlanes) {
          const found = newPlanes.some(newPlane => newPlane.id === oldPlane.id);
          if (!found) {
            oldPlane.dtoState = DtoState.Deleted;
            toDeletes.push(oldPlane);
          }
        }

        // Chercher les nouveaux avions à ajouter
        for (const newPlane of newPlanes) {
          const found = oldPlanes.some(oldPlane => newPlane.id === oldPlane.id);
          if (!found) {
            newPlane.dtoState = DtoState.Added;
            toInserts.push(newPlane);
          }
        }

        // Chercher les avions à mettre à jour
        for (const oldPlane of oldPlanes) {
          const newPlane = newPlanes.find(
            newPlane => newPlane.id === oldPlane.id
          );
          if (oldPlane && newPlane) {
            const hasDifferentProperties = Object.keys(oldPlane).some(key => {
              let oldValue = (<any>oldPlane)[key];
              let newValue = (<any>newPlane)[key];
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
              Object.assign(oldPlane, newPlane);
              oldPlane.dtoState = DtoState.Modified;
              toUpdates.push(oldPlane);
            }
          }
        }

        console.log('toDeletes');
        console.log(toDeletes);
        console.log('toInserts');
        console.log(toInserts);
        console.log('toUpdates');
        console.log(toUpdates);

        return toDeletes.concat(toInserts).concat(toUpdates);
      })
    );
  }
}
