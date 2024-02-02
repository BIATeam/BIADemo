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
  }

  onFileSelected(event: any) {
    this.biaBulkCopyService
      .uploadCsv(this.planeService, this.planeFormComponent, event.target.files)
      .subscribe(x => console.log(x));
  }
}
