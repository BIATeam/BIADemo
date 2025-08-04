import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'bia-ng/core';
import { Observable } from 'rxjs';
import { AssignViewToTeam } from '../model/assign-view-to-team';
import { View } from '../model/view';

@Injectable({
  providedIn: 'root',
})
export class ViewDas extends AbstractDas<View> {
  constructor(injector: Injector) {
    super(injector, 'Views');
  }

  public getAll(): Observable<Array<View>> {
    return this.http.get<Array<View>>(`${this.route}`);
  }

  public assignViewToTeam(assignViewToTeam: AssignViewToTeam) {
    return this.http.put(
      `${this.route}${assignViewToTeam.viewId}/AssignViewToTeam`,
      assignViewToTeam
    );
  }
}
