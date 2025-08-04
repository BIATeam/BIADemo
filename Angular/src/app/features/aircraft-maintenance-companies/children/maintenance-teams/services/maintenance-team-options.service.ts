import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import {
  CrudItemOptionsService,
  DictOptionDto,
} from 'packages/bia-ng/shared/public-api';
import { Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { DomainAirportOptionsActions } from 'src/app/domains/airport-option/store/airport-options-actions';
import { getAllCountryOptions } from 'src/app/domains/country-option/store/country-option.state';
import { DomainCountryOptionsActions } from 'src/app/domains/country-option/store/country-options-actions';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceTeamOptionsService extends CrudItemOptionsService {
  airportOptions$: Observable<OptionDto[]>;
  countryOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<AppState>) {
    super();
    // TODO after creation of CRUD Team MaintenanceTeam : get all required option dto use in Table calc and create and edit form
    this.airportOptions$ = this.store.select(getAllAirportOptions);
    this.countryOptions$ = this.store.select(getAllCountryOptions);
    let cpt = 0;
    const airport = cpt++;
    const country = cpt++;

    this.dictOptionDtos$ = combineLatest([
      this.airportOptions$,
      this.countryOptions$,
    ]).pipe(
      map(options => {
        return <DictOptionDto[]>[
          new DictOptionDto('currentAirport', options[airport]),
          new DictOptionDto('operationAirports', options[airport]),
          new DictOptionDto('currentCountry', options[country]),
          new DictOptionDto('operationCountries', options[country]),
        ];
      })
    );
  }

  loadAllOptions() {
    this.store.dispatch(DomainAirportOptionsActions.loadAll());
    this.store.dispatch(DomainCountryOptionsActions.loadAll());
  }
}
