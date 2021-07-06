import { Injectable, Injector } from '@angular/core';
import { PlaneType } from '../model/plane-type';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';

@Injectable({
  providedIn: 'root'
})
export class PlaneTypeDas extends AbstractDas<PlaneType> {
  constructor(injector: Injector) {
    super(injector, 'PlanesTypes');
  }
}
