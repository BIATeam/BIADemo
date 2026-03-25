import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { Plane, planeFieldsConfiguration } from '../model/plane';
import { PlaneSpecific } from '../model/plane-specific';

@Injectable({
  providedIn: 'root',
})
export class PlaneDas extends AbstractDas<Plane, PlaneSpecific> {
  constructor(injector: Injector) {
    super(injector, 'PlanesSpecific', planeFieldsConfiguration);
  }
}
