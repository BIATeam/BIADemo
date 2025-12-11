import { Injectable } from '@angular/core';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { DictOptionDto } from '../../../components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from '../../../feature-templates/crud-items/services/crud-item-options.service';

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
