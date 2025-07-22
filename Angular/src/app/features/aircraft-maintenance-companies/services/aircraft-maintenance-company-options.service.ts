import { Injectable } from '@angular/core';
import { CrudItemOptionsService, DictOptionDto } from 'biang/shared';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AircraftMaintenanceCompanyOptionsService extends CrudItemOptionsService {
  constructor() {
    super();
    // TODO after creation of CRUD Team AircraftMaintenanceCompany : get all required option dto use in Table calc and create and edit form

    this.dictOptionDtos$ = combineLatest([]).pipe(
      map(() => <DictOptionDto[]>[])
    );
  }
}
