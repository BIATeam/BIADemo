import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Pilot } from '../model/pilot';

@Injectable({
  providedIn: 'root',
})
export class PilotDas extends AbstractDas<Pilot> {
  constructor(injector: Injector) {
    super(injector, 'Pilots');
  }
}
