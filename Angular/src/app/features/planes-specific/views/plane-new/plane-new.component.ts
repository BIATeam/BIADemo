import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { CrudItemNewComponent } from 'biang/shared';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeSpecificCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-specific-new',
  templateUrl: './plane-new.component.html',
  imports: [PlaneFormComponent, AsyncPipe],
})
export class PlaneNewComponent extends CrudItemNewComponent<PlaneSpecific> {
  crudItem: PlaneSpecific = <PlaneSpecific>{};

  constructor(
    protected injector: Injector,
    public planeService: PlaneService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeSpecificCRUDConfiguration;
  }
}
