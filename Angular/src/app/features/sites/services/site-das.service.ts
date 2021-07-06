import { Injectable, Injector } from '@angular/core';
import { Site } from '../model/site/site';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';

@Injectable({
  providedIn: 'root'
})
export class SiteDas extends AbstractDas<Site> {
  constructor(injector: Injector) {
    super(injector, 'sites');
  }
}
