import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { DefaultView } from '../model/default-view';
import { View } from '../model/view';

@Injectable({
  providedIn: 'root',
})
export class UserViewDas extends AbstractDas<View> {
  constructor(injector: Injector) {
    super(injector, 'Views/UserViews');
  }

  public setDefaultView(defaultView: DefaultView) {
    return this.http.put<DefaultView>(
      `${this.route}${defaultView.id}/setDefault`,
      defaultView
    );
  }
}
