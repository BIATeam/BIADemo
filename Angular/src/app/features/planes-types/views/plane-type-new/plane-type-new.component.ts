import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { CrudItemNewComponent } from 'packages/bia-ng/shared/public-api';
import { PlaneTypeFormComponent } from '../../components/plane-type-form/plane-type-form.component';
import { PlaneType } from '../../model/plane-type';
import { planeTypeCRUDConfiguration } from '../../plane-type.constants';
import { PlaneTypeService } from '../../services/plane-type.service';

@Component({
  selector: 'app-plane-type-new',
  templateUrl: './plane-type-new.component.html',
  imports: [PlaneTypeFormComponent, AsyncPipe],
})
export class PlaneTypeNewComponent extends CrudItemNewComponent<PlaneType> {
  constructor(
    protected injector: Injector,
    public planeTypeService: PlaneTypeService
  ) {
    super(injector, planeTypeService);
    this.crudConfiguration = planeTypeCRUDConfiguration;
  }
}
