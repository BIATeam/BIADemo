import { Component, Injector, ViewChild } from '@angular/core';
import { Plane } from '../../model/plane';
import { PlaneService } from '../../services/plane.service';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { PlaneCRUDConfiguration } from '../../plane.constants';
import { PlaneBulkSaveService } from '../../services/plane-bulk-save.service';
import { CrudItemBulkComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-bulk/crud-item-bulk.component';

@Component({
  selector: 'app-plane-bulk-save',
  templateUrl: './plane-bulk-save.component.html',
})
export class PlaneBulkSaveComponent extends CrudItemBulkComponent<Plane> {
  @ViewChild(PlaneFormComponent) planeFormComponent: PlaneFormComponent;

  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    protected planeBulkSaveService: PlaneBulkSaveService
  ) {
    super(injector, planeBulkSaveService);
    this.crudConfiguration = PlaneCRUDConfiguration;
  }
  save(toSaves: Plane[]): void {
    this.planeService.save(toSaves);
  }

  getForm(): PlaneFormComponent {
    return this.planeFormComponent;
  }
}
