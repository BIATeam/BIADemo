import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllRoleOptions } from 'src/app/domains/role-option/store/role-option.state';
import { loadAllRoleOptions } from 'src/app/domains/role-option/store/role-options-actions';
import { getAllUserOptions } from 'src/app/domains/user-option/store/user-option.state';
import { loadAllUserOptions } from 'src/app/domains/user-option/store/user-options-actions';
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

    loadAllOptions() {
        this.store.dispatch(loadAllUserOptions());
        this.store.dispatch(loadAllRoleOptions());
    }
}
