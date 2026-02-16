import { Injectable } from '@angular/core';
import { OptionDto } from '@bia-team/bia-ng/models';
import { Store } from '@ngrx/store';
import {
  CrudItemOptionsService,
  DictOptionDto,
} from 'packages/bia-ng/shared/public-api';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { DomainAirportOptionsActions } from 'src/app/domains/airport-option/store/airport-options-actions';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class PilotOptionsService extends CrudItemOptionsService {
  airportOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<AppState>) {
    super();
    // TODO after creation of CRUD Pilot : get all required option dto use in Table calc and create and edit form
    this.airportOptions$ = this.store.select(getAllAirportOptions);
    let cpt = 0;
    const airport = cpt++;

    this.dictOptionDtos$ = combineLatest([this.airportOptions$]).pipe(
      map(options => {
        return <DictOptionDto[]>[
          new DictOptionDto('baseAirport', options[airport]),
        ];
      })
    );
  }

  loadAllOptions() {
    this.store.dispatch(DomainAirportOptionsActions.loadAll());
  }
}
