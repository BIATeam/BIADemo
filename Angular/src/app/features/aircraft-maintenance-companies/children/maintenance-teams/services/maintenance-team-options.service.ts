import { Injectable } from '@angular/core';
import {
  // BIAToolKit - Begin Option
  Observable,
  // BIAToolKit - End Option
  combineLatest,
} from 'rxjs';
import { map } from 'rxjs/operators';
// BIAToolKit - Begin Option Airport
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { DomainAirportOptionsActions } from 'src/app/domains/airport-option/store/airport-options-actions';
// BIAToolKit - End Option Airport
// BIAToolKit - Begin Option Country
import { getAllCountryOptions } from 'src/app/domains/country-option/store/country-option.state';
import { DomainCountryOptionsActions } from 'src/app/domains/country-option/store/country-options-actions';
// BIAToolKit - End Option Country
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';
// BIAToolKit - Begin Option
import { Store } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { AppState } from 'src/app/store/state';
// BIAToolKit - End Option

@Injectable({
  providedIn: 'root',
})
export class MaintenanceTeamOptionsService extends CrudItemOptionsService {
  // BIAToolKit - Begin Option Country
  countriesOptions$: Observable<OptionDto[]>;
  // BIAToolKit - End Option Country
  // BIAToolKit - Begin Option Airport
  airportOptions$: Observable<OptionDto[]>;
  // BIAToolKit - End Option Airport

  constructor(
    // BIAToolKit - Begin Option
    private store: Store<AppState>
    // BIAToolKit - End Option
  ) {
    super();
    // TODO after creation of CRUD Team MaintenanceTeam : get all requiered option dto use in Table calc and create and edit form
    // BIAToolKit - Begin Option Country
    this.countriesOptions$ = this.store.select(getAllCountryOptions);
    // BIAToolKit - End Option Country
    // BIAToolKit - Begin Option Airport
    this.airportOptions$ = this.store.select(getAllAirportOptions);
    // BIAToolKit - End Option Airport
    // BIAToolKit - Begin Option
    let cpt = 0;
    // BIAToolKit - End Option
    // BIAToolKit - Begin Option Country
    const countryType = cpt++;
    // BIAToolKit - End Option Country
    // BIAToolKit - Begin Option Airport
    const airport = cpt++;
    // BIAToolKit - End Option Airport

    this.dictOptionDtos$ = combineLatest([
      // BIAToolKit - Begin Option Country
      this.countriesOptions$,
      // BIAToolKit - End Option Country
      // BIAToolKit - Begin Option Airport
      this.airportOptions$,
      // BIAToolKit - End Option Airport
    ]).pipe(
      map(
        (
          // BIAToolKit - Begin Option
          options
          // BIAToolKit - End Option
        ) => {
          return <DictOptionDto[]>[
            // BIAToolKit - Begin Option Airport
            new DictOptionDto('operationAirports', options[airport]),
            new DictOptionDto('currentAirport', options[airport]),
            // BIAToolKit - End Option Airport
            // BIAToolKit - Begin Option Country
            new DictOptionDto('currentCountry', options[countryType]),
            new DictOptionDto('operationCountries', options[countryType]),
            // BIAToolKit - End Option Country
          ];
        }
      )
    );
  }

  // BIAToolKit - Begin Option
  loadAllOptions() {
    // BIAToolKit - End Option
    // BIAToolKit - Begin Option Country
    this.store.dispatch(DomainCountryOptionsActions.loadAll());
    // BIAToolKit - End Option Country
    // BIAToolKit - Begin Option Airport
    this.store.dispatch(DomainAirportOptionsActions.loadAll());
    // BIAToolKit - End Option Airport
    // BIAToolKit - Begin Option
  }
  // BIAToolKit - End Option
}
