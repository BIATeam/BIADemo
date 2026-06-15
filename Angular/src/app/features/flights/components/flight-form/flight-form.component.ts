import { Component } from '@angular/core';
import {
  BiaFormComponent,
  CrudItemFormComponent,
} from 'packages/bia-ng/shared/public-api';
import { Flight } from '../../model/flight';

@Component({
  selector: 'app-flight-form',
  templateUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class FlightFormComponent extends CrudItemFormComponent<Flight> {}
