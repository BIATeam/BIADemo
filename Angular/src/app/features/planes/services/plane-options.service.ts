import { Injectable } from '@angular/core';
import {
  /* BIAToolKit - Begin Option */
  Observable,
  /* BIAToolKit - End Option */
  combineLatest,
} from 'rxjs';
import { map } from 'rxjs/operators';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
/* BIAToolKit - Begin Option */
import { Store } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
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
    // TODO after creation of CRUD Plane : get all required option dto use in Table calc and create and edit form
    // BIAToolKit - Begin Option PlaneType
    this.planeTypeOptions$ = this.store.select(getAllPlaneTypeOptions);
    // BIAToolKit - End Option PlaneType
    // BIAToolKit - Begin Option Airport
    this.airportOptions$ = this.store.select(getAllAirportOptions);
    // BIAToolKit - End Option Airport

    /* BIAToolKit - Begin Option */
    let cpt = 0;
    /* BIAToolKit - End Option */
    // BIAToolKit - Begin Option PlaneType
    const planeType = cpt++;
    // BIAToolKit - End Option PlaneType
    // BIAToolKit - Begin Option Airport
    const airport = cpt++;
    // BIAToolKit - End Option Airport

    this.dictOptionDtos$ = combineLatest([
      // BIAToolKit - Begin Option PlaneType
      this.planeTypeOptions$,
      // BIAToolKit - End Option PlaneType
      // BIAToolKit - Begin Option Airport
      this.airportOptions$,
      // BIAToolKit - End Option Airport
    ]).pipe(
      map(options => {
        return <DictOptionDto[]>[
          // BIAToolKit - Begin OptionField Airport connectingAirports
          new DictOptionDto('connectingAirports', options[airport]),
          // BIAToolKit - End OptionField Airport connectingAirports
          // BIAToolKit - Begin OptionField PlaneType planeType
          new DictOptionDto('planeType', options[planeType]),
          // BIAToolKit - End OptionField PlaneType planeType
          // BIAToolKit - Begin OptionField PlaneType similarTypes
          new DictOptionDto('similarTypes', options[planeType]),
          // BIAToolKit - End OptionField PlaneType similarTypes
          // BIAToolKit - Begin OptionField Airport currentAirport
          new DictOptionDto('currentAirport', options[airport]),
          // BIAToolKit - End OptionField Airport currentAirport
        ];
      })
    );
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
