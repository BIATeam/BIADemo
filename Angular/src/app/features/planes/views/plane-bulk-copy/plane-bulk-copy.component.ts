import { Component, Injector, ViewChild } from '@angular/core';
import { Plane } from '../../model/plane';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { PlaneService } from '../../services/plane.service';
import { PlaneCRUDConfiguration } from '../../plane.constants';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { BiaBulkCopyService } from 'src/app/core/bia-core/services/bia-bulk-copy.service';

@Component({
  selector: 'app-plane-bulk-copy',
  templateUrl: './plane-bulk-copy.component.html',
  styleUrls: ['./plane-bulk-copy.component.scss'],
})
export class PlaneBulkCopyComponent extends CrudItemNewComponent<Plane> {
  @ViewChild(PlaneFormComponent) planeFormComponent: PlaneFormComponent;

  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    private biaBulkCopyService: BiaBulkCopyService<Plane>
  ) {
    super(injector, planeService);
    this.crudConfiguration = PlaneCRUDConfiguration;
    this.biaBulkCopyService.initCrudItemService(planeService);
  }

  checkObject(crudItem: any) {
    const b = this.planeFormComponent.checkObject(crudItem);
    console.log(b);
  }

  public downloadCsv() {
    const columns: string[] = [
      'id',
      'msn',
      'isActive',
      'lastFlightDate',
      'deliveryDate',
      'capacity',
    ];

    this.biaBulkCopyService.downloadCsv(columns);
    // this.planeService.dasService
    //   .getList({ endpoint: 'all' })
    //   .subscribe((x: Plane[]) => {
    //     let csv = Papa.unparse<Plane>(x, {
    //       columns: [
    //         'id',
    //         'msn',
    //         'isActive',
    //         'lastFlightDate',
    //         'deliveryDate',
    //         'capacity',
    //       ],
    //     });
    //     csv = `sep=,\n${csv}`;
    //     const blob = new Blob([csv], { type: 'text/csv;charset=utf-8' });
    //     FileSaver.saveAs(blob, 'test.csv');
    //   });
  }

  onFileSelected(event: any) {
    // const selectedFile: File = event.target.files[0];
    this.biaBulkCopyService.handleFileInput(event.target.files);
    // let fileContent = '';
    // const fileReader = new FileReader();
    // fileReader.onload = () => {
    //   fileContent = fileReader.result as string;
    // };
    // fileReader.readAsText(selectedFile);

    // const list = Papa.parse<Plane>(fileContent);
    // console.log(list);
  }

  // check<T extends BaseDto>(
  //   newPlanes: T[],
  //   oldPlanes$: Observable<T[]>
  // ): Observable<T[]> {
  //   return oldPlanes$.pipe(
  //     map((oldPlanes: T[]) => {
  //       const toDeletes: T[] = [];
  //       const toInserts: T[] = [];
  //       const toUpdates: T[] = [];

  //       // Chercher les avions à supprimer
  //       for (const oldPlane of oldPlanes) {
  //         const found = newPlanes.some(newPlane => newPlane.id === oldPlane.id);
  //         if (!found) {
  //           oldPlane.dtoState = DtoState.Deleted;
  //           toDeletes.push(oldPlane);
  //         }
  //       }

  //       // Chercher les nouveaux avions à ajouter
  //       for (const newPlane of newPlanes) {
  //         const found = oldPlanes.some(oldPlane => newPlane.id === oldPlane.id);
  //         if (!found) {
  //           newPlane.dtoState = DtoState.Added;
  //           toInserts.push(newPlane);
  //         }
  //       }

  //       // Chercher les avions à mettre à jour
  //       for (const oldPlane of oldPlanes) {
  //         const newPlane = newPlanes.find(
  //           newPlane => newPlane.id === oldPlane.id
  //         );
  //         if (oldPlane && newPlane) {
  //           const hasDifferentProperties = Object.keys(oldPlane).some(key => {
  //             let oldValue = (<any>oldPlane)[key];
  //             let newValue = (<any>newPlane)[key];
  //             if (newValue === undefined) {
  //               return false;
  //             }
  //             if (oldValue instanceof Date === true) {
  //               oldValue = oldValue?.toISOString();
  //               newValue = newValue?.toISOString();
  //             }
  //             return newValue !== oldValue;
  //           });
  //           if (hasDifferentProperties) {
  //             Object.assign(oldPlane, newPlane);
  //             oldPlane.dtoState = DtoState.Modified;
  //             toUpdates.push(oldPlane);
  //           }
  //         }
  //       }

  //       console.log('toDeletes');
  //       console.log(toDeletes);
  //       console.log('toInserts');
  //       console.log(toInserts);
  //       console.log('toUpdates');
  //       console.log(toUpdates);

  //       return toDeletes.concat(toInserts).concat(toUpdates);
  //     })
  //   );
  // }

  // check55(newPlanes: BaseDto[]) {
  //   this.planeService.dasService
  //     .getList({ endpoint: 'all' })
  //     .subscribe((oldPlanes: BaseDto[]) => {
  //       const toDeletes: BaseDto[] = [];
  //       const toInserts: BaseDto[] = [];
  //       const toUpdates: BaseDto[] = [];

  //       // Chercher les avions à supprimer
  //       for (const oldPlane of oldPlanes) {
  //         const found = newPlanes.some(newPlane => newPlane.id === oldPlane.id);
  //         if (!found) {
  //           oldPlane.dtoState = DtoState.Deleted;
  //           toDeletes.push(oldPlane);
  //         }
  //       }

  //       // Chercher les nouveaux avions à ajouter
  //       for (const newPlane of newPlanes) {
  //         const found = oldPlanes.some(oldPlane => newPlane.id === oldPlane.id);
  //         if (!found) {
  //           newPlane.dtoState = DtoState.Added;
  //           toInserts.push(newPlane);
  //         }
  //       }

  //       // Chercher les avions à mettre à jour
  //       for (const oldPlane of oldPlanes) {
  //         const newPlane = newPlanes.find(
  //           newPlane => newPlane.id === oldPlane.id
  //         );
  //         if (oldPlane && newPlane) {
  //           const hasDifferentProperties = Object.keys(oldPlane).some(key => {
  //             let oldValue = (<any>oldPlane)[key];
  //             let newValue = (<any>newPlane)[key];
  //             if (newValue === undefined) {
  //               return false;
  //             }
  //             if (oldValue instanceof Date === true) {
  //               oldValue = oldValue?.toISOString();
  //               newValue = newValue?.toISOString();
  //             }
  //             return newValue !== oldValue;
  //           });
  //           if (hasDifferentProperties) {
  //             Object.assign(oldPlane, newPlane);
  //             oldPlane.dtoState = DtoState.Modified;
  //             toUpdates.push(oldPlane);
  //           }
  //         }
  //       }

  //       console.log('toDeletes');
  //       console.log(toDeletes);
  //       console.log('toInserts');
  //       console.log(toInserts);
  //       console.log('toUpdates');
  //       console.log(toUpdates);
  //     });
  // }

  // check3(newPlanes: BaseDto[]) {
  //   this.planeService.dasService
  //     .getList({ endpoint: 'all' })
  //     .subscribe((oldPlanes: BaseDto[]) => {
  //       const toDeletes: BaseDto[] = [];
  //       const toInserts: BaseDto[] = [];
  //       const toUpdates: BaseDto[] = [];

  //       // Chercher les avions à supprimer
  //       for (const oldPlane of oldPlanes) {
  //         const found = newPlanes.some(newPlane => newPlane.id === oldPlane.id);
  //         if (!found) {
  //           oldPlane.dtoState = DtoState.Deleted;
  //           toDeletes.push(oldPlane);
  //         }
  //       }

  //       // Chercher les nouveaux avions à ajouter
  //       for (const newPlane of newPlanes) {
  //         const found = oldPlanes.some(oldPlane => newPlane.id === oldPlane.id);
  //         if (!found) {
  //           newPlane.dtoState = DtoState.Added;
  //           toInserts.push(newPlane);
  //         }
  //       }

  //       // Chercher les avions à mettre à jour
  //       for (const oldPlane of oldPlanes) {
  //         const newPlane = newPlanes.find(
  //           newPlane => newPlane.id === oldPlane.id
  //         );
  //         if (oldPlane && newPlane) {
  //           const hasDifferentProperties = Object.keys(oldPlane).some(key => {
  //             let oldValue = (<any>oldPlane)[key];
  //             let newValue = (<any>newPlane)[key];
  //             if (newValue === undefined) {
  //               return false;
  //             }
  //             if (oldValue instanceof Date === true) {
  //               oldValue = oldValue?.toISOString();
  //               newValue = newValue?.toISOString();
  //             }
  //             return newValue !== oldValue;
  //           });
  //           if (hasDifferentProperties) {
  //             Object.assign(oldPlane, newPlane);
  //             oldPlane.dtoState = DtoState.Modified;
  //             toUpdates.push(oldPlane);
  //           }
  //         }
  //       }

  //       console.log('toDeletes');
  //       console.log(toDeletes);
  //       console.log('toInserts');
  //       console.log(toInserts);
  //       console.log('toUpdates');
  //       console.log(toUpdates);

  //       const toSaves: BaseDto[] = toDeletes
  //         .concat(toInserts)
  //         .concat(toUpdates);

  //       return toSaves;
  //     });
  // }

  public test() {
    const jsonString = `{
      "msn": "MSN5aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
      "isActive": true,
      "lastFlightDate": "2023-10-25T00:00:00",
      "deliveryDate": "2023-01-10T00:00:00",
      "syncTime": null,
      "capacity": 1,
      "siteId": 3,
      "connectingAirports": [],
      "planeType": {
        "display": "AZERTYU",
        "id": 5,
        "dtoState": 0
      },
      "dtoState": 0
    }`;

    const mavar = JSON.parse(jsonString);
    this.checkObject(mavar);
  }
}
