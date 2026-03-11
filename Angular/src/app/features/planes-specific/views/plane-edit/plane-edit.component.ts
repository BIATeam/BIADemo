import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  CrudItemEditComponent,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeSpecificCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-specific-edit',
  templateUrl: './plane-edit.component.html',
  imports: [PlaneFormComponent, AsyncPipe, SpinnerComponent],
})
export class PlaneEditComponent extends CrudItemEditComponent<PlaneSpecific> {
  constructor(
    protected injector: Injector,
    public planeService: PlaneService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeSpecificCRUDConfiguration;
  }
}
