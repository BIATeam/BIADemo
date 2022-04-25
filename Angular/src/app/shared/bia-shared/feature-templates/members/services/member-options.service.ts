import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllRoleOptions } from 'src/app/domains/bia-domains/role-option/store/role-option.state';
import { DomainRoleOptionsActions } from 'src/app/domains/bia-domains/role-option/store/role-options-actions';
import { getAllUserOptions } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { DomainUserOptionsActions } from 'src/app/domains/bia-domains/user-option/store/user-options-actions';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { AppState } from 'src/app/store/state';

@Injectable({
    providedIn: 'root'
})
export class MemberOptionsService {
    dictOptionDtos$: Observable<DictOptionDto[]>;

    userOptions$: Observable<OptionDto[]>;
    roleOptions$: Observable<OptionDto[]>;

    constructor(
        private store: Store<AppState>,
    ) {
        this.userOptions$ = this.store.select(getAllUserOptions);
        this.roleOptions$ = this.store.select(getAllRoleOptions);

        // [Calc] Dict is used in calc mode only. It map the column name with the list OptionDto.
        this.dictOptionDtos$ = combineLatest([this.userOptions$, this.roleOptions$]).pipe(
            map(
                (options) =>
                <DictOptionDto[]>[
                    new DictOptionDto('user', options[0]),
                    new DictOptionDto('roles', options[1])
                ]
            )
        );
    }

    loadAllOptions(teamTypeId:number) {
        this.store.dispatch(DomainUserOptionsActions.loadAll());
        this.store.dispatch(DomainRoleOptionsActions.loadAll({ teamTypeId: teamTypeId }));
    }

    refreshUsersOptions() {
        this.store.dispatch(DomainUserOptionsActions.loadAll());
    }
}
