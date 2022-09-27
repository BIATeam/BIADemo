import { Injectable } from '@angular/core';
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';

@Injectable({
    providedIn: 'root'
})
export class SiteOptionsService extends CrudItemOptionsService {
    /*siteTypeOptions$: Observable<OptionDto[]>;
    airportOptions$: Observable<OptionDto[]>;*/

    constructor(/*
        private store: Store<AppState>,
    */) {
        super();
        // TODO after creation of CRUD Team Site : get all requiered option dto use in Table calc and create and edit form
/*        this.siteTypeOptions$ = this.store.select(getAllSiteTypeOptions);
        this.airportOptions$ = this.store.select(getAllAirportOptions);

        this.dictOptionDtos$ = combineLatest([this.siteTypeOptions$, this.airportOptions$]).pipe(
            map(
                (options) =>
                <DictOptionDto[]>[
                    new DictOptionDto('siteType', options[0]),
                    new DictOptionDto('connectingAirports', options[1])
                ]
            )
        );*/
    }

    loadAllOptions() {
/*        this.store.dispatch(DomainSiteTypeOptionsActions .loadAll());
        this.store.dispatch(DomainAirportOptionsActions.loadAll());*/
    }
}
