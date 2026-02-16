import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
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
