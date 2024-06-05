import { Component, Injector } from '@angular/core';
import { PlaneType } from '../../model/plane-type';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { PlaneTypeService } from '../../services/plane-type.service';
import { PlaneTypeCRUDConfiguration } from '../../plane-type.constants';

@Component({
  selector: 'app-plane-type-new',
  templateUrl: './plane-type-new.component.html',
})
export class PlaneTypeNewComponent extends CrudItemNewComponent<PlaneType> {
  constructor(
    protected injector: Injector,
    public planeTypeService: PlaneTypeService
  ) {
    super(injector, planeTypeService);
    this.crudConfiguration = PlaneTypeCRUDConfiguration;
  }
}
