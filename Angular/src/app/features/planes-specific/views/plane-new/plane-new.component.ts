import { Component, Injector } from '@angular/core';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeSpecificCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-plane-specific-new',
  templateUrl: './plane-new.component.html',
  imports: [PlaneFormComponent, AsyncPipe],
})
export class PlaneNewComponent extends CrudItemNewComponent<PlaneSpecific> {
  newPlane: PlaneSpecific = { engines: [] } as unknown as PlaneSpecific;

  constructor(
    protected injector: Injector,
    public planeService: PlaneService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeSpecificCRUDConfiguration;
  }
}
