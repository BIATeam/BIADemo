import { Component, Injector, ViewChild } from '@angular/core';
import { Plane } from '../../model/plane';
import { PlaneService } from '../../services/plane.service';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { PlaneCRUDConfiguration } from '../../plane.constants';
import { CrudItemBulkComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-bulk/crud-item-bulk.component';

@Component({
  selector: 'app-plane-bulk',
  templateUrl: './plane-bulk.component.html',
})
export class PlaneBulkComponent extends CrudItemBulkComponent<Plane> {
  @ViewChild(PlaneFormComponent) planeFormComponent: PlaneFormComponent;

  constructor(
    protected injector: Injector,
    public planeService: PlaneService
  ) {
    super(injector, planeService);
    this.crudConfiguration = PlaneCRUDConfiguration;
  }
  save(toSaves: Plane[]): void {
    this.planeService.save(toSaves);
  }

  getForm(): PlaneFormComponent {
    return this.planeFormComponent;
  }
}
