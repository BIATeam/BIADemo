import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { TeamDefaultView } from '../model/team-default-view';
import { TeamView } from '../model/team-view';

@Injectable({
  providedIn: 'root',
})
export class TeamViewDas extends AbstractDas<TeamView> {
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
