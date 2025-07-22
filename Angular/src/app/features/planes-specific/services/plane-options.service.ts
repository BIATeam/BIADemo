import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { OptionDto } from 'biang/models';
import { CrudItemOptionsService, DictOptionDto } from 'biang/shared';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { DomainAirportOptionsActions } from 'src/app/domains/airport-option/store/airport-options-actions';
import { getAllPlaneTypeOptions } from 'src/app/domains/plane-type-option/store/plane-type-option.state';
import { DomainPlaneTypeOptionsActions } from 'src/app/domains/plane-type-option/store/plane-type-options-actions';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class PlaneOptionsService extends CrudItemOptionsService {
  planeTypeOptions$: Observable<OptionDto[]>;
  airportOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<AppState>) {
    super();
    // TODO after creation of CRUD Plane : get all required option dto use in Table calc and create and edit form
    this.planeTypeOptions$ = this.store.select(getAllPlaneTypeOptions);
    this.airportOptions$ = this.store.select(getAllAirportOptions);

    this.dictOptionDtos$ = combineLatest([
      this.planeTypeOptions$,
      this.airportOptions$,
    ]).pipe(
      map(
        options =>
          <DictOptionDto[]>[
            new DictOptionDto('planeType', options[0]),
            new DictOptionDto('connectingAirports', options[1]),
            new DictOptionDto('currentAirport', options[1]),
          ]
      )
    );
  }

  loadAllOptions() {
    this.store.dispatch(DomainPlaneTypeOptionsActions.loadAll());
    this.store.dispatch(DomainAirportOptionsActions.loadAll());
  }
}
