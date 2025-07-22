﻿import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'biang/core';
import { PlaneType } from '../model/plane-type';

@Injectable({
  providedIn: 'root',
})
export class PlaneTypeDas extends AbstractDas<PlaneType> {
  constructor(injector: Injector) {
    super(injector, 'PlanesTypes');
  }
}
