import { Injectable } from '@angular/core';
import {
  CrudItemOptionsService,
  DictOptionDto,
} from 'packages/bia-ng/shared/public-api';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ViewOptionsService extends CrudItemOptionsService {
  constructor() {
    super();
    // TODO after creation of CRUD Plane : get all required option dto use in Table calc and create and edit form
    this.dictOptionDtos$ = combineLatest([]).pipe(
      map(() => {
        return <DictOptionDto[]>[];
      })
    );
  }

  loadAllOptions() {}
}
