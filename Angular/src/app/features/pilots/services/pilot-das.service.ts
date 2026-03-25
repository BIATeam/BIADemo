import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { Pilot, pilotFieldsConfiguration } from '../model/pilot';
import { PilotList } from '../model/pilot-list';

@Injectable({
  providedIn: 'root',
})
export class PilotDas extends AbstractDas<PilotList, Pilot> {
  constructor(injector: Injector) {
    super(injector, 'Pilots', pilotFieldsConfiguration);
  }
}
