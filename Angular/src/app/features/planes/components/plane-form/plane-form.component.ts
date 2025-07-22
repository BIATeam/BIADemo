import { Component } from '@angular/core';
import { BiaFormComponent, CrudItemFormComponent } from 'biang/shared';
import { Plane } from '../../model/plane';

@Component({
  selector: 'app-plane-form',
  templateUrl:
    '../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class PlaneFormComponent extends CrudItemFormComponent<Plane> {}
