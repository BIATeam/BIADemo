import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  DomainRoleOptionsActions,
  DomainRoleOptionsStore,
} from 'biang/domains';
import { OptionDto } from 'biang/models';
import { TeamTypeId } from 'biang/models/enum';
import { CrudItemOptionsService, DictOptionDto } from 'biang/shared';
import { BiaAppState } from 'biang/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class UserOptionsService extends CrudItemOptionsService {
  roleOptions$: Observable<OptionDto[]>;

  constructor(protected store: Store<BiaAppState>) {
    super();
    // TODO after creation of CRUD User : get all required option dto use in Table calc and create and edit form
    this.roleOptions$ = this.store.select(
      DomainRoleOptionsStore.getAllRoleOptions
    );

    this.dictOptionDtos$ = combineLatest([this.roleOptions$]).pipe(
      map(options => <DictOptionDto[]>[new DictOptionDto('roles', options[0])])
    );
  }

  loadAllOptions() {
    this.store.dispatch(
      DomainRoleOptionsActions.loadAll({ teamTypeId: TeamTypeId.Root })
    );
  }
}
