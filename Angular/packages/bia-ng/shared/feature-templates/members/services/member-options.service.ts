import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  DomainRoleOptionsActions,
  DomainRoleOptionsStore,
  DomainUserOptionsActions,
  DomainUserOptionsStore,
} from 'bia-ng/domains';
import { OptionDto } from 'bia-ng/models';
import { BiaAppState } from 'bia-ng/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { DictOptionDto } from '../../../components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from '../../crud-items/services/crud-item-options.service';

@Injectable({
  providedIn: 'root',
})
export class MemberOptionsService extends CrudItemOptionsService {
  userOptions$: Observable<OptionDto[]>;
  roleOptions$: Observable<OptionDto[]>;

  constructor(protected store: Store<BiaAppState>) {
    super();
    // TODO after creation of CRUD Member : get all required option dto use in Table calc and create and edit form
    this.userOptions$ = this.store.select(
      DomainUserOptionsStore.getAllUserOptions
    );
    this.roleOptions$ = this.store.select(
      DomainRoleOptionsStore.getAllRoleOptions
    );

    this.dictOptionDtos$ = combineLatest([
      this.userOptions$,
      this.roleOptions$,
    ]).pipe(
      map(
        options =>
          <DictOptionDto[]>[
            new DictOptionDto('user', options[0]),
            new DictOptionDto('roles', options[1]),
          ]
      )
    );
  }

  loadAllOptions(optionFilter: any) {
    this.store.dispatch(DomainUserOptionsActions.loadAll());
    this.store.dispatch(
      DomainRoleOptionsActions.loadAll({ teamTypeId: optionFilter.teamTypeId })
    );
  }

  refreshUsersOptions() {
    this.store.dispatch(DomainUserOptionsActions.loadAll());
  }
}
