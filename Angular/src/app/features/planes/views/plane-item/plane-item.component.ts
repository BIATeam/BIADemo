import { Component, Injector } from '@angular/core';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { Plane } from '../../model/plane';
import { PlaneService } from '../../services/plane.service';

@Component({
    selector: 'app-planes-item',
    templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
    styleUrls: [
        '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
    ],
    standalone: false
})
export class PlaneItemComponent extends CrudItemItemComponent<Plane> {
  constructor(
    protected injector: Injector,
    public planeService: PlaneService
  ) {
    super(injector, planeService);
  }
}
