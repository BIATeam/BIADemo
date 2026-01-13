import { Injectable } from '@angular/core';
import { CrudItemOptionsService, DictOptionDto } from '@bia-team/bia-ng/shared';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class PlaneTypeOptionsService extends CrudItemOptionsService {
  constructor() {
    super();
    // TODO after creation of CRUD PlaneType : get all required option dto use in Table calc and create and edit form

    this.dictOptionDtos$ = combineLatest([]).pipe(
      map(() => <DictOptionDto[]>[])
    );
  }
}
