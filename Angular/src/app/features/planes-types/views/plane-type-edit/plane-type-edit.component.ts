import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { PlaneType } from '../../model/plane-type';
import { planeTypeCRUDConfiguration } from '../../plane-type.constants';
import { PlaneTypeService } from '../../services/plane-type.service';

@Component({
    selector: 'app-plane-type-edit',
    templateUrl: './plane-type-edit.component.html',
    standalone: false
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
