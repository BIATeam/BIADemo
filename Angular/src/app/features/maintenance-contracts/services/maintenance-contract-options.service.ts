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
// BIAToolKit - Begin Option AircraftMaintenanceCompany
import { getAllAircraftMaintenanceCompanyOptions } from 'src/app/domains/aircraft-maintenance-company-option/store/aircraft-maintenance-company-option.state';
import { DomainAircraftMaintenanceCompanyOptionsActions } from 'src/app/domains/aircraft-maintenance-company-option/store/aircraft-maintenance-company-options-actions';
// BIAToolKit - End Option AircraftMaintenanceCompany
// BIAToolKit - Begin Option Plane
import { getAllPlaneOptions } from 'src/app/domains/plane-option/store/plane-option.state';
import { DomainPlaneOptionsActions } from 'src/app/domains/plane-option/store/plane-options-actions';
// BIAToolKit - End Option Plane
// BIAToolKit - Begin Option Site
import { getAllSiteOptions } from 'src/app/domains/site-option/store/site-option.state';
import { DomainSiteOptionsActions } from 'src/app/domains/site-option/store/site-options-actions';
// BIAToolKit - End Option Site
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceContractOptionsService extends CrudItemOptionsService {
  // BIAToolKit - Begin Option AircraftMaintenanceCompany
  aircraftMaintenanceCompanyOptions$: Observable<OptionDto[]>;
  // BIAToolKit - End Option AircraftMaintenanceCompany
  // BIAToolKit - Begin Option Plane
  planeOptions$: Observable<OptionDto[]>;
  // BIAToolKit - End Option Plane
  // BIAToolKit - Begin Option Site
  siteOptions$: Observable<OptionDto[]>;
  // BIAToolKit - End Option Site

  constructor(
    /* BIAToolKit - Begin Option */
    private store: Store<AppState>
    /* BIAToolKit - End Option */
  ) {
    super();
    // TODO after creation of CRUD MaintenanceContract : get all required option dto use in Table calc and create and edit form
    // BIAToolKit - Begin Option AircraftMaintenanceCompany
    this.aircraftMaintenanceCompanyOptions$ = this.store.select(
      getAllAircraftMaintenanceCompanyOptions
    );
    // BIAToolKit - End Option AircraftMaintenanceCompany
    // BIAToolKit - Begin Option Plane
    this.planeOptions$ = this.store.select(getAllPlaneOptions);
    // BIAToolKit - End Option Plane
    // BIAToolKit - Begin Option Site
    this.siteOptions$ = this.store.select(getAllSiteOptions);
    // BIAToolKit - End Option Site

    /* BIAToolKit - Begin Option */
    let cpt = 0;
    /* BIAToolKit - End Option */
    // BIAToolKit - Begin Option AircraftMaintenanceCompany
    const aircraftMaintenanceCompany = cpt++;
    // BIAToolKit - End Option AircraftMaintenanceCompany
    // BIAToolKit - Begin Option Plane
    const plane = cpt++;
    // BIAToolKit - End Option Plane
    // BIAToolKit - Begin Option Site
    const site = cpt++;
    // BIAToolKit - End Option Site

    this.dictOptionDtos$ = combineLatest([
      // BIAToolKit - Begin Option AircraftMaintenanceCompany
      this.aircraftMaintenanceCompanyOptions$,
      // BIAToolKit - End Option AircraftMaintenanceCompany
      // BIAToolKit - Begin Option Plane
      this.planeOptions$,
      // BIAToolKit - End Option Plane
      // BIAToolKit - Begin Option Site
      this.siteOptions$,
      // BIAToolKit - End Option Site
    ]).pipe(
      map(options => {
        return <DictOptionDto[]>[
          // BIAToolKit - Begin OptionField Plane planes
          new DictOptionDto('planes', options[plane]),
          // BIAToolKit - End OptionField Plane planes
          // BIAToolKit - Begin OptionField AircraftMaintenanceCompany aircraftMaintenanceCompany
          new DictOptionDto(
            'aircraftMaintenanceCompany',
            options[aircraftMaintenanceCompany]
          ),
          // BIAToolKit - End OptionField AircraftMaintenanceCompany aircraftMaintenanceCompany
          // BIAToolKit - Begin OptionField Site site
          new DictOptionDto('site', options[site]),
          // BIAToolKit - End OptionField Site site
        ];
      })
    );
  }

  /* BIAToolKit - Begin Option */
  loadAllOptions() {
    // BIAToolKit - Begin Option AircraftMaintenanceCompany
    this.store.dispatch(
      DomainAircraftMaintenanceCompanyOptionsActions.loadAll()
    );
    // BIAToolKit - End Option AircraftMaintenanceCompany
    // BIAToolKit - Begin Option Plane
    this.store.dispatch(DomainPlaneOptionsActions.loadAll());
    // BIAToolKit - End Option Plane
    // BIAToolKit - Begin Option Site
    this.store.dispatch(DomainSiteOptionsActions.loadAll());
    // BIAToolKit - End Option Site
  }
  /* BIAToolKit - End Option */
}
