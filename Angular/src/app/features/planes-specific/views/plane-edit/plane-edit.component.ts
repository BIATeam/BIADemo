import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeSpecificCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-specific-edit',
  templateUrl: './plane-edit.component.html',
  imports: [NgIf, PlaneFormComponent, AsyncPipe, SpinnerComponent],
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
