import { Injectable } from '@angular/core';
/* BIAToolKit - Begin Option */
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { AppState } from 'src/app/store/state';
/* BIAToolKit - End Option */
// BIAToolKit - Begin Option Airport
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { DomainAirportOptionsActions } from 'src/app/domains/airport-option/store/airport-options-actions';
// BIAToolKit - End Option Airport
// BIAToolKit - Begin Option PlaneType
import { getAllPlaneTypeOptions } from 'src/app/domains/plane-type-option/store/plane-type-option.state';
import { DomainPlaneTypeOptionsActions } from 'src/app/domains/plane-type-option/store/plane-type-options-actions';
// BIAToolKit - End Option PlaneType
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';

@Injectable({
  providedIn: 'root',
})
export class PlaneOptionsService extends CrudItemOptionsService {
  // BIAToolKit - Begin Option PlaneType
  planeTypeOptions$: Observable<OptionDto[]>;
  // BIAToolKit - End Option PlaneType
  // BIAToolKit - Begin Option Airport
  airportOptions$: Observable<OptionDto[]>;
  // BIAToolKit - End Option Airport

  constructor(
    /* BIAToolKit - Begin Option */
    private store: Store<AppState>
    /* BIAToolKit - End Option */
  ) {
    super();
    // TODO after creation of CRUD Plane : get all requiered option dto use in Table calc and create and edit form
    // BIAToolKit - Begin Option PlaneType
    this.planeTypeOptions$ = this.store.select(getAllPlaneTypeOptions);
    // BIAToolKit - End Option PlaneType
    // BIAToolKit - Begin Option Airport
    this.airportOptions$ = this.store.select(getAllAirportOptions);
    // BIAToolKit - End Option Airport

    /* BIAToolKit - Begin Option */
    let cpt: number = 0;
    this.dictOptionDtos$ = combineLatest([
      // BIAToolKit - Begin Option PlaneType
      this.planeTypeOptions$,
      // BIAToolKit - End Option PlaneType
      // BIAToolKit - Begin Option Airport
      this.airportOptions$,
      // BIAToolKit - End Option Airport
    ]).pipe(
      map(
        options =>
          <DictOptionDto[]>[
            // BIAToolKit - Begin Option PlaneType
            new DictOptionDto('planeType', options[cpt++]),
            // BIAToolKit - End Option PlaneType
            // BIAToolKit - Begin Option Airport
            new DictOptionDto('connectingAirports', options[cpt++]),
            // BIAToolKit - End Option Airport
          ]
      )
    );
    /* BIAToolKit - End Option */
  }

  /* BIAToolKit - Begin Option */
  loadAllOptions() {
    // BIAToolKit - Begin Option PlaneType
    this.store.dispatch(DomainPlaneTypeOptionsActions.loadAll());
    // BIAToolKit - End Option PlaneType
    // BIAToolKit - Begin Option Airport
    this.store.dispatch(DomainAirportOptionsActions.loadAll());
    // BIAToolKit - End Option Airport
  }
  /* BIAToolKit - End Option */
}
