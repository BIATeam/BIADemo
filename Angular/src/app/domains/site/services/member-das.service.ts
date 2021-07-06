import { Injectable } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MemberDas {
  constructor(private http: HttpClient) {}

  public setDefaultSite(id: number) {
    const route = AbstractDas.buildRoute(`Members/Sites/${id}/setDefault`);
    return this.http.put(route, null);
  }

  public setDefaultRole(id: number) {
    const route = AbstractDas.buildRoute(`Members/Roles/${id}/setDefault`);
    return this.http.put(route, null);
  }
}
