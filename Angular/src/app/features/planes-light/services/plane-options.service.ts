import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { DomainAirportOptionsActions } from 'src/app/domains/airport-option/store/airport-options-actions';
import { getAllPlaneTypeOptions } from 'src/app/domains/plane-type-option/store/plane-type-option.state';
import { DomainPlaneTypeOptionsActions } from 'src/app/domains/plane-type-option/store/plane-type-options-actions';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { AppState } from 'src/app/store/state';

@Injectable({
    providedIn: 'root'
})
export class PlaneOptionsService extends CrudItemOptionsService {
    planeTypeOptions$: Observable<OptionDto[]>;
    airportOptions$: Observable<OptionDto[]>;

    constructor(
        private store: Store<AppState>,
    ) {
        super();
        this.planeTypeOptions$ = this.store.select(getAllPlaneTypeOptions);
        this.airportOptions$ = this.store.select(getAllAirportOptions);

        // [Calc] Dict is used in calc mode only. It map the column name with the list OptionDto.
        this.dictOptionDtos$ = combineLatest([this.planeTypeOptions$, this.airportOptions$]).pipe(
            map(
                (options) =>
                <DictOptionDto[]>[
                    new DictOptionDto('planeType', options[0]),
                    new DictOptionDto('connectingAirports', options[1])
                ]
            )
        );
    }

    loadAllOptions() {
        this.store.dispatch(DomainPlaneTypeOptionsActions .loadAll());
        this.store.dispatch(DomainAirportOptionsActions.loadAll());
    }
}
