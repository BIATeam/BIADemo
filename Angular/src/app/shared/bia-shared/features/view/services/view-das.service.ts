import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { View } from '../model/view';
import { AssignViewToSite } from '../model/assign-view-to-site';
// import { DefaultView } from '../model/defaultView';

@Injectable({
  providedIn: 'root'
})
export class ViewDas extends AbstractDas<View> {
  constructor(injector: Injector) {
    super(injector, 'Views');
  }

  public getAll(): Observable<Array<View>> {
    return this.http.get<Array<View>>(`${this.route}`);
  }

  public assignViewToSite(assignViewToSite: AssignViewToSite) {
    return this.http.put(`${this.route}${assignViewToSite.viewId}/AssignViewToSite`, assignViewToSite);
  }
}
