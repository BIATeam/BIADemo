import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { Plane } from '../../model/plane';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { PlaneService } from '../../services/plane.service';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { take } from 'rxjs';
import { PlaneCRUDConfiguration } from '../../plane.constants';
import { PlaneBulkSaveService } from '../../services/plane-bulk-save.service';
import { BulkSaveData } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk-save.service';

@Component({
  selector: 'app-plane-bulk-save',
  templateUrl: './plane-bulk-save.component.html',
  styleUrls: ['./plane-bulk-save.component.scss'],
})
export class PlaneBulkSaveComponent
  extends CrudItemNewComponent<Plane>
  implements OnInit
{
  @ViewChild(PlaneFormComponent) planeFormComponent: PlaneFormComponent;

  constructor(
    protected injector: Injector,
    protected planeService: PlaneService,
    private planeBulkSaveService: PlaneBulkSaveService
  ) {
    super(injector, planeService);
    this.crudConfiguration = PlaneCRUDConfiguration;
  }

  ngOnInit() {
    super.ngOnInit();
  }

  onFileSelected(event: any) {
    this.planeBulkSaveService
      .uploadCsv(
        this.planeFormComponent,
        event.target.files,
        this.crudConfiguration
      )
      .pipe(take(1))
      .subscribe((bulkSaveData: BulkSaveData<Plane>) => {
        const toSaves = [...bulkSaveData.toInserts, ...bulkSaveData.toUpdates];
        this.planeService.save(toSaves);
        console.log(bulkSaveData);
      });
  }
}
