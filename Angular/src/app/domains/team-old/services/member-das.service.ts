import { Injectable } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MemberDas {
  constructor(private http: HttpClient) {}

  public setDefaultTeam(teamTypeId: number, teamId: number) {
    const route = AbstractDas.buildRoute(`Members/TeamType/${teamTypeId}/setDefault/${teamId}`);
    return this.http.put(route, null);
  }

  public setDefaultRoles(teamId: number, roleIds: number[]) {
    const route = AbstractDas.buildRoute(`Members/Team/${teamId}/setDefaultRoles`);
    return this.http.put(route, roleIds);
  }
}
