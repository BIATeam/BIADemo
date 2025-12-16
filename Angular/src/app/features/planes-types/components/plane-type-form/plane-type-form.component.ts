import { Component } from '@angular/core';
import {
  BiaFormComponent,
  CrudItemFormComponent,
} from '@bia-team/bia-ng/shared';
import { PlaneType } from '../../model/plane-type';

@Component({
  selector: 'app-plane-type-form',
  templateUrl:
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class PlaneTypeFormComponent extends CrudItemFormComponent<PlaneType> {}
