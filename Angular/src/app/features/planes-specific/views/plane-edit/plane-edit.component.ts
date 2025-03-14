import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeSpecificCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';
import { NgIf, AsyncPipe } from '@angular/common';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';

@Component({
  selector: 'app-plane-specific-edit',
  templateUrl: './plane-edit.component.html',
  imports: [
    NgIf,
    PlaneFormComponent,
    BiaSharedModule,
    AsyncPipe,
    SpinnerComponent,
  ],
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
