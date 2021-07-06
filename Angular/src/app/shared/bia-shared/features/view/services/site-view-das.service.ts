import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { SiteDefaultView } from '../model/site-default-view';
import { SiteView } from '../model/site-view';

@Injectable({
  providedIn: 'root'
})
export class SiteViewDas extends AbstractDas<SiteView> {
  constructor(injector: Injector) {
    super(injector, 'Views/SiteViews');
  }

  public setDefaultView(defaultView: SiteDefaultView) {
    return this.http.put<SiteDefaultView>(`${this.route}${defaultView.id}/setDefault`, defaultView);
  }
}
