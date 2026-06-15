import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Flight, flightFieldsConfiguration } from '../model/flight';

@Injectable({
  providedIn: 'root',
})
export class FlightDas extends AbstractDas<Flight> {
  constructor(injector: Injector) {
    super(injector, 'Flights', flightFieldsConfiguration);
  }
}
