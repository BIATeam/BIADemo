import { Component, Injector, ViewChild } from '@angular/core';
import { Plane } from '../../model/plane';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { PlaneService } from '../../services/plane.service';
import { PlaneCRUDConfiguration } from '../../plane.constants';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import * as Papa from 'papaparse';
import FileSaver from 'file-saver';

@Component({
  selector: 'app-plane-bulk-copy',
  templateUrl: './plane-bulk-copy.component.html',
  styleUrls: ['./plane-bulk-copy.component.scss']
})
export class PlaneBulkCopyComponent extends CrudItemNewComponent<Plane> {

  @ViewChild(PlaneFormComponent) planeFormComponent: PlaneFormComponent;
  showPlaneForm = true;

  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
  ) {
    super(injector, planeService);
    this.crudConfiguration = PlaneCRUDConfiguration;
  }

  checkObject(crudItem: any) {
    const b = this.planeFormComponent.checkObject(crudItem);
    console.log(b);
  }

  public downloadCsv() {

    this.planeService.dasService.getList({ endpoint: 'all' }).subscribe(
      (x: Plane[]) => {
        let csv = Papa.unparse<Plane>(x, { columns: ["id", "msn", "isActive", "lastFlightDate", "deliveryDate", "capacity"] });
        csv = `sep=,\n${csv}`;
        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8' });
        FileSaver.saveAs(blob, 'test.csv');
      }
    );
  }

  onFileSelected(event: any) {
    // const selectedFile: File = event.target.files[0];
    this.handleFileInput(event.target.files);
    // let fileContent = '';
    // const fileReader = new FileReader();
    // fileReader.onload = () => {
    //   fileContent = fileReader.result as string;
    // };
    // fileReader.readAsText(selectedFile);

    // const list = Papa.parse<Plane>(fileContent);
    // console.log(list);
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
    const result = Papa.parse<Plane>(csv, {
      header: true,
      // complete: (result) => {
      //   const planes: Plane[] = result.data.map((row) => ({
      //     // Définir les propriétés Plane en fonction des colonnes du CSV
      //     id: parseInt(row.id),
      //     name: row.name,
      //     // ...
      //   }));

      //   // Utilisez la liste 'planes' selon vos besoins
      //   console.log(planes);
      // }
    });

    console.log(result.data);
    this.check(result.data);
  }

  check(newPlanes:Plane[]){

    this.planeService.dasService.getList({ endpoint: 'all' }).subscribe(
      (oldPlanes: Plane[]) => {
        const toDeletes = oldPlanes.filter(oldPlane =>
          !newPlanes.some(newPlane => newPlane.id === oldPlane.id)
        );
        const toInserts = newPlanes.filter(newPlane =>
          !oldPlanes.some(oldPlane => newPlane.id === oldPlane.id)
        );
        console.log(toDeletes);
        console.log(toInserts);
      }
    );

    
  }



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