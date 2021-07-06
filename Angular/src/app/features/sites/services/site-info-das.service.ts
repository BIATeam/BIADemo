import { Injectable, Injector } from '@angular/core';
import { SiteInfo } from '../model/site/site-info';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';

@Injectable({
  providedIn: 'root'
})
export class SiteInfoDas extends AbstractDas<SiteInfo> {
  constructor(injector: Injector) {
    super(injector, 'sites');
  }
}
