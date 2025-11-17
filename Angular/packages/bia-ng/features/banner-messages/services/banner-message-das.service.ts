import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Observable } from 'rxjs';
import { BannerMessage } from '../model/banner-message';

@Injectable({
  providedIn: 'root',
})
export class BannerMessageDas extends AbstractDas<BannerMessage> {
  constructor(injector: Injector) {
    super(injector, 'BannerMessages');
  }

  getActives(): Observable<BannerMessage[]> {
    return this.getListItems<BannerMessage>({ endpoint: 'actives' });
  }
}
