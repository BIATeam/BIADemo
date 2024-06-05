import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { Team } from '../model/team';

@Injectable({
  providedIn: 'root',
})
export class TeamDas extends AbstractDas<Team> {
  constructor(injector: Injector) {
    super(injector, 'Teams');
  }

  public setDefaultTeam(teamTypeId: number, teamId: number) {
    const route = AbstractDas.buildRoute(
      `Teams/TeamType/${teamTypeId}/setDefault/${teamId}`
    );
    return this.http.put(route, null);
  }

  public setDefaultRoles(teamId: number, roleIds: number[]) {
    const route = AbstractDas.buildRoute(
      `Teams/Team/${teamId}/setDefaultRoles`
    );
    return this.http.put(route, roleIds);
  }
}
