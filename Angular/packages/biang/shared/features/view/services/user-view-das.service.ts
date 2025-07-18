import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'biang/core';
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
