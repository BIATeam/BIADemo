import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllRoleOptions } from 'src/app/domains/bia-domains/role-option/store/role-option.state';
import { DomainRoleOptionsActions } from 'src/app/domains/bia-domains/role-option/store/role-options-actions';
import { getAllUserOptions } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { DomainUserOptionsActions } from 'src/app/domains/bia-domains/user-option/store/user-options-actions';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class MemberOptionsService extends CrudItemOptionsService {
  userOptions$: Observable<OptionDto[]>;
  roleOptions$: Observable<OptionDto[]>;

  constructor(protected store: Store<AppState>) {
    super();
    // TODO after creation of CRUD Member : get all required option dto use in Table calc and create and edit form
    this.userOptions$ = this.store.select(getAllUserOptions);
    this.roleOptions$ = this.store.select(getAllRoleOptions);

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
