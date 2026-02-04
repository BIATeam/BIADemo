import { Injectable } from '@angular/core';
import {
  DomainRoleOptionsActions,
  DomainRoleOptionsStore,
} from '@bia-team/bia-ng/domains';
import { OptionDto } from '@bia-team/bia-ng/models';
import { BiaTeamTypeId } from '@bia-team/bia-ng/models/enum';
import { CrudItemOptionsService, DictOptionDto } from '@bia-team/bia-ng/shared';
import { BiaAppState } from '@bia-team/bia-ng/store';
import { Store } from '@ngrx/store';
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
