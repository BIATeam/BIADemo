import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { Flight } from '../model/flight';

@Injectable({
  providedIn: 'root',
})
export class FlightDas extends AbstractDas<Flight> {
  constructor(injector: Injector) {
    super(injector, 'Flights');
  }
}
