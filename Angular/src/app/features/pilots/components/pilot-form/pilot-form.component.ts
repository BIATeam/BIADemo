import { Component } from '@angular/core';
import {
  BiaFormComponent,
  CrudItemFormComponent,
} from 'packages/bia-ng/shared/public-api';
import { Pilot } from '../../model/pilot';

@Component({
  selector: 'app-pilot-form',
  templateUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class PilotFormComponent extends CrudItemFormComponent<Pilot> {}
