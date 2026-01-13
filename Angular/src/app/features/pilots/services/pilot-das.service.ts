import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { Pilot } from '../model/pilot';

@Injectable({
  providedIn: 'root',
})
export class PilotDas extends AbstractDas<Pilot> {
  constructor(injector: Injector) {
    super(injector, 'Pilots');
  }
}
