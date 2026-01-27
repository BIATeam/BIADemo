import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Plane, planeFieldsConfiguration } from '../model/plane';

@Injectable({
  providedIn: 'root',
})
export class PlaneDas extends AbstractDas<Plane> {
  constructor(injector: Injector) {
    super(injector, 'Planes', planeFieldsConfiguration);
  }
}
