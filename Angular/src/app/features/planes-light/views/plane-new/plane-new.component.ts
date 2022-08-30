import { Component, Injector } from '@angular/core';
import { Plane, PlaneCRUDConfiguration } from '../../model/plane';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-new',
  templateUrl: './plane-new.component.html',
})
export class PlaneNewComponent extends CrudItemNewComponent<Plane>  {
   constructor(
    protected injector: Injector,
    public planeService: PlaneService,
  ) {
     super(injector, planeService);
     this.crudConfiguration = PlaneCRUDConfiguration;
   }
}