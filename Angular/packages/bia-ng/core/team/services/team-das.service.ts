import { Injectable, Injector } from '@angular/core';
import { Team } from 'packages/bia-ng/models/public-api';
import { AbstractDas } from '../../services/abstract-das.service';

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

  public resetDefaultTeam(teamTypeId: number) {
    const route = AbstractDas.buildRoute(
      `Teams/TeamType/${teamTypeId}/resetDefault`
    );
    return this.http.put(route, null);
  }

  public setDefaultRoles(teamId: number, roleIds: number[]) {
    const route = AbstractDas.buildRoute(
      `Teams/Team/${teamId}/setDefaultRoles`
    );
    return this.http.put(route, roleIds);
  }

  public resetDefaultRoles(teamId: number) {
    const route = AbstractDas.buildRoute(
      `Teams/Team/${teamId}/resetDefaultRoles`
    );
    return this.http.put(route, null);
  }
}
