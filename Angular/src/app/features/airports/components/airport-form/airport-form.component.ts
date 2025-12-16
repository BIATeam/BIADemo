import { Component } from '@angular/core';
import {
  BiaFormComponent,
  CrudItemFormComponent,
} from '@bia-team/bia-ng/shared';
import { Airport } from '../../model/airport';

@Component({
  selector: 'app-airport-form',
  templateUrl:
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class AirportFormComponent extends CrudItemFormComponent<Airport> {}
