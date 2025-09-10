import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/option-dto';
import {
  CrudItemOptionsService,
  DictOptionDto,
} from 'packages/bia-ng/shared/public-api';
import { combineLatest, map, Observable } from 'rxjs';
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { DomainAirportOptionsActions } from 'src/app/domains/airport-option/store/airport-options-actions';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class FlightOptionsService extends CrudItemOptionsService {
  airportOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<AppState>) {
    super();
    this.airportOptions$ = this.store.select(getAllAirportOptions);
    let cpt = 0;
    const airport = cpt++;

    // TODO after creation of CRUD Flight : get all required option dto use in Table calc and create and edit form
    this.dictOptionDtos$ = combineLatest([this.airportOptions$]).pipe(
      map(options => {
        return <DictOptionDto[]>[
          new DictOptionDto('departureAirport', options[airport]),
          new DictOptionDto('arrivalAirport', options[airport]),
        ];
      })
    );
  }

  loadAllOptions() {
    this.store.dispatch(DomainAirportOptionsActions.loadAll());
  }
}
