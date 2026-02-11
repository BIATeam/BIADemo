import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  DomainRoleOptionsActions,
  DomainRoleOptionsStore,
} from 'packages/bia-ng/domains/public-api';
import { BiaTeamTypeId } from 'packages/bia-ng/models/enum/public-api';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import {
  CrudItemOptionsService,
  DictOptionDto,
} from 'packages/bia-ng/shared/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
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
      DomainRoleOptionsActions.loadAll({ teamTypeId: BiaTeamTypeId.Root })
    );
  }
}
