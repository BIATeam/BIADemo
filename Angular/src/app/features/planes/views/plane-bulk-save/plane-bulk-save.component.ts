import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { Plane } from '../../model/plane';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { PlaneService } from '../../services/plane.service';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { take } from 'rxjs';
import { PlaneCRUDConfiguration } from '../../plane.constants';
import { PlaneBulkSaveService } from '../../services/plane-bulk-save.service';
import { BulkSaveData } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk-save.service';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';

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
  bulkSaveData: BulkSaveData<Plane>;
  displayedColumns: KeyValuePair[];
  sortFieldValue = '';
  deleteChecked = false;
  updateChecked = false;
  insertChecked = false;

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
    this.displayedColumns = this.crudConfiguration.fieldsConfig.columns.map(
      col => <KeyValuePair>{ key: col.field, value: col.header }
    );
    this.sortFieldValue = this.displayedColumns[0].key;
  }

  onFileSelected(event: any) {
    this.planeBulkSaveService
      .uploadCsv(this.planeFormComponent, event.files, this.crudConfiguration)
      .pipe(take(1))
      .subscribe(
        (bulkSaveData: BulkSaveData<Plane>) =>
          (this.bulkSaveData = bulkSaveData)
      );
  }

  onCancel() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onSave() {
    let toSaves: Plane[] = [];

    if (this.deleteChecked === true) {
      toSaves = toSaves.concat(this.bulkSaveData.toDeletes);
    }

    if (this.insertChecked === true) {
      toSaves = toSaves.concat(this.bulkSaveData.toInserts);
    }

    if (this.updateChecked === true) {
      toSaves = toSaves.concat(this.bulkSaveData.toUpdates);
    }

    if (toSaves.length > 0) {
      this.planeService.save(toSaves);
    }

    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
