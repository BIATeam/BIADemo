import { Injectable } from '@angular/core';
import { CrudItemOptionsService } from 'packages/bia-ng/shared/public-api';

@Injectable({
  providedIn: 'root',
})
export class PilotOptionsService extends CrudItemOptionsService {
  constructor() {
    super();
    // TODO after creation of CRUD Pilot : get all required option dto use in Table calc and create and edit form
  }

  loadAllOptions() {}
}
