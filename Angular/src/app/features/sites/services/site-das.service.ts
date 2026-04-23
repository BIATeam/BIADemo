import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Site, siteFieldsConfiguration } from '../model/site';

@Injectable({
  providedIn: 'root',
})
export class SiteDas extends AbstractDas<Site> {
  constructor(injector: Injector) {
    super(injector, 'Sites', siteFieldsConfiguration);
  }
}
