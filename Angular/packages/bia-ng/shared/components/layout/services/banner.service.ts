import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { BannerMessage } from 'packages/bia-ng/features/public-api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BannerService extends AbstractDas<BannerMessage> {
  constructor(protected injector: Injector) {
    super(injector, 'BannerMessages');
  }
  getActives(): Observable<BannerMessage[]> {
    return this.getListItems<BannerMessage>({ endpoint: 'actives' });
  }
}
