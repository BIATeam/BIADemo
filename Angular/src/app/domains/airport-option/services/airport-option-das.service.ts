import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { OptionDto } from 'packages/bia-ng/models/public-api';

@Injectable({
  providedIn: 'root',
})
export class AirportOptionDas extends AbstractDas<OptionDto> {
  constructor(injector: Injector) {
    super(injector, 'AirportOptions');
  }
}
