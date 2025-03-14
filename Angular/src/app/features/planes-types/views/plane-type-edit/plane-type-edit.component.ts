import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { PlaneType } from '../../model/plane-type';
import { planeTypeCRUDConfiguration } from '../../plane-type.constants';
import { PlaneTypeService } from '../../services/plane-type.service';
import { NgIf, AsyncPipe } from '@angular/common';
import { PlaneTypeFormComponent } from '../../components/plane-type-form/plane-type-form.component';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-plane-type-edit',
    templateUrl: './plane-type-edit.component.html',
    imports: [NgIf, PlaneTypeFormComponent, BiaSharedModule, AsyncPipe, SpinnerComponent]
})
export class PlaneTypeEditComponent extends CrudItemEditComponent<PlaneType> {
  constructor(
    protected injector: Injector,
    public planeTypeService: PlaneTypeService
  ) {
    super(injector, planeTypeService);
    this.crudConfiguration = planeTypeCRUDConfiguration;
  }
}
