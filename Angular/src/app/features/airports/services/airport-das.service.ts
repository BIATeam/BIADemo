import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Airport, airportFieldsConfiguration } from '../model/airport';

@Injectable({
  providedIn: 'root',
})
export class AirportDas extends AbstractDas<Airport> {
  constructor(injector: Injector) {
    super(injector, 'Airports', airportFieldsConfiguration);
  }
}
