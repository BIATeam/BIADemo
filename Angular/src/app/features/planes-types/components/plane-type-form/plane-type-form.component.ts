import { Component } from '@angular/core';
import {
  BiaFormComponent,
  CrudItemFormComponent,
} from 'packages/bia-ng/shared/public-api';
import { PlaneType } from '../../model/plane-type';

@Component({
  selector: 'app-plane-type-form',
  templateUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class PlaneTypeFormComponent extends CrudItemFormComponent<PlaneType> {}
