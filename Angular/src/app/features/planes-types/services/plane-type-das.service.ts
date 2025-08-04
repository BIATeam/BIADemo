import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { PlaneType } from '../model/plane-type';

@Injectable({
  providedIn: 'root',
})
export class PlaneTypeDas extends AbstractDas<PlaneType> {
  constructor(injector: Injector) {
    super(injector, 'PlanesTypes');
  }
}
