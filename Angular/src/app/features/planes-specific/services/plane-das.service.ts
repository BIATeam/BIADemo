import { Injectable, Injector } from '@angular/core';
import { AbstractDasWithListAndItem } from 'src/app/core/bia-core/services/abstract-das-with-list-and-item.service';
import { Plane } from '../model/plane';
import { PlaneSpecific } from '../model/plane-specific';

@Injectable({
  providedIn: 'root',
})
export class PlaneDas extends AbstractDasWithListAndItem<PlaneSpecific, Plane> {
  constructor(injector: Injector) {
    super(injector, 'PlanesSpecific');
  }
}
