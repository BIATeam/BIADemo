import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'biang/core';
import { Airport } from '../model/airport';

@Injectable({
  providedIn: 'root',
})
export class AirportDas extends AbstractDas<Airport> {
  constructor(injector: Injector) {
    super(injector, 'Airports');
  }
}
