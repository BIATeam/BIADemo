import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
// Begin BIADemo
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { DomainAirportOptionsActions } from 'src/app/domains/airport-option/store/airport-options-actions';
import { getAllCountryOptions } from 'src/app/domains/country-option/store/country-option.state';
import { DomainCountryOptionsActions } from 'src/app/domains/country-option/store/country-options-actions';
// End BIADemo
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceTeamOptionsService extends CrudItemOptionsService {
  countriesOptions$: Observable<OptionDto[]>;
  airportOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<AppState>) {
    super();
    // TODO after creation of CRUD Team MaintenanceTeam : get all requiered option dto use in Table calc and create and edit form
    // Begin BIADemo
    this.countriesOptions$ = this.store.select(getAllCountryOptions);
    this.airportOptions$ = this.store.select(getAllAirportOptions);
    let cpt = 0;
    const countryType = cpt++;
    const airport = cpt++;
    // End BIADemo

    this.dictOptionDtos$ = combineLatest([
      // Begin BIADemo
      this.countriesOptions$,
      this.airportOptions$,
      // End BIADemo
    ]).pipe(
      map(options => {
        return <DictOptionDto[]>[
          // Begin BIADemo
          new DictOptionDto('operationAirports', options[airport]),
          new DictOptionDto('currentCountry', options[countryType]),
          new DictOptionDto('operationCountries', options[countryType]),
          new DictOptionDto('currentAirport', options[airport]),
          // End BIADemo
        ];
      })
    );
  }

  // Begin BIADemo
  loadAllOptions() {
    console.log('LoadAllOptions');
    this.store.dispatch(DomainCountryOptionsActions.loadAll());
    this.store.dispatch(DomainAirportOptionsActions.loadAll());
  }
  // End BIADemo
}
