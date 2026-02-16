import { Injectable, Injector } from '@angular/core';
import { DataResult, GetListByPostParam } from '@bia-team/bia-ng/models';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Observable } from 'rxjs';
import { Pilot, pilotFieldsConfiguration } from '../model/pilot';
import { PilotList } from '../model/pilot-list';

@Injectable({
  providedIn: 'root',
})
export class PilotDas extends AbstractDas<PilotList, Pilot> {
  constructor(injector: Injector) {
    super(injector, 'Pilots', pilotFieldsConfiguration);
  }

  getListSingleItemsByPost(
    param: GetListByPostParam
  ): Observable<DataResult<Pilot[]>> {
    return this.getListItemsByPost<Pilot>({ endpoint: 'allItems', ...param });
  }
}
