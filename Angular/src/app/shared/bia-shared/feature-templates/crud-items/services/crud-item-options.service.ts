import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';

@Injectable({
    providedIn: 'root'
})
export class CrudItemOptionsService {
    dictOptionDtos$: Observable<DictOptionDto[]>;

    constructor(
        // protected store: Store<AppState>,
    ) {
        // TODO redefine in plane 
        /*
        this.crudItemTypeOptions$ = this.store.select(getAllCrudItemTypeOptions);
        this.airportOptions$ = this.store.select(getAllAirportOptions);

        // [Calc] Dict is used in calc mode only. It map the column name with the list OptionDto.
        this.dictOptionDtos$ = combineLatest([this.crudItemTypeOptions$, this.airportOptions$]).pipe(
            map(
                (options) =>
                <DictOptionDto[]>[
                    new DictOptionDto('crud-itemType', options[0]),
                    new DictOptionDto('connectingAirports', options[1])
                ]
            )
        );
        */
    }

    loadAllOptions() {
        // TODO redefine in plane 
        /*
        this.store.dispatch(DomainCrudItemTypeOptionsActions .loadAll());
        this.store.dispatch(DomainAirportOptionsActions.loadAll());
        */
    }
}
