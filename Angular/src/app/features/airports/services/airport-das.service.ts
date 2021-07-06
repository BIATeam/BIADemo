import { Injectable, Injector } from '@angular/core';
import { Airport } from '../model/airport';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';

@Injectable({
  providedIn: 'root'
})
export class AirportDas extends AbstractDas<Airport> {
  constructor(injector: Injector) {
    super(injector, 'Airports');
  }
}
