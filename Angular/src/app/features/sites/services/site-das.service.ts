﻿import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'biang/core';
import { Site } from '../model/site';

@Injectable({
  providedIn: 'root',
})
export class SiteDas extends AbstractDas<Site> {
  constructor(injector: Injector) {
    super(injector, 'Sites');
  }
}
