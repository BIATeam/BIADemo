import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { TeamDefaultView } from '../model/team-default-view';
import { View } from '../model/view';

@Injectable({
  providedIn: 'root',
})
export class TeamViewDas extends AbstractDas<View> {
  constructor(injector: Injector) {
    super(injector, 'Views/TeamViews');
  }

  public setDefaultView(defaultView: TeamDefaultView) {
    return this.http.put<TeamDefaultView>(
      `${this.route}${defaultView.id}/setDefault`,
      defaultView
    );
  }
}
