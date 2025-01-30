import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { Plane } from '../model/plane';
import { PlaneSpecific } from '../model/plane-specific';

@Injectable({
  providedIn: 'root',
})
export class PlaneDas extends AbstractDas<Plane, PlaneSpecific> {
  constructor(injector: Injector) {
    super(injector, 'PlanesSpecific');
  }
}
