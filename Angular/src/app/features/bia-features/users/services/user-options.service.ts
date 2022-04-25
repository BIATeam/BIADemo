import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllRoleOptions } from 'src/app/domains/bia-domains/role-option/store/role-option.state';
import { DomainRoleOptionsActions } from 'src/app/domains/bia-domains/role-option/store/role-options-actions';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';

@Injectable({
    providedIn: 'root'
})
export class UserOptionsService {
    dictOptionDtos$: Observable<DictOptionDto[]>;

    roleOptions$: Observable<OptionDto[]>;

    constructor(
        private store: Store<AppState>,
    ) {
        this.roleOptions$ = this.store.select(getAllRoleOptions);

        // [Calc] Dict is used in calc mode only. It map the column name with the list OptionDto.
        this.dictOptionDtos$ = combineLatest([this.roleOptions$]).pipe(
            map(
                (options) =>
                <DictOptionDto[]>[
                    new DictOptionDto('roles', options[0])
                ]
            )
        );
    }

    loadAllOptions() {
        this.store.dispatch(DomainRoleOptionsActions.loadAll({teamTypeId:TeamTypeId.Root}));
    }
}
