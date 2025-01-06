import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllRoleOptions } from 'src/app/domains/bia-domains/role-option/store/role-option.state';
import { DomainRoleOptionsActions } from 'src/app/domains/bia-domains/role-option/store/role-options-actions';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class UserOptionsService extends CrudItemOptionsService {
  roleOptions$: Observable<OptionDto[]>;

  constructor(protected store: Store<AppState>) {
    super();
    // TODO after creation of CRUD User : get all required option dto use in Table calc and create and edit form
    this.roleOptions$ = this.store.select(getAllRoleOptions);

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
