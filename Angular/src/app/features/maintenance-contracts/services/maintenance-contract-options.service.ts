import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { OptionDto } from 'bia-ng/models';
import { CrudItemOptionsService, DictOptionDto } from 'bia-ng/shared';
import { Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllAircraftMaintenanceCompanyOptions } from 'src/app/domains/aircraft-maintenance-company-option/store/aircraft-maintenance-company-option.state';
import { DomainAircraftMaintenanceCompanyOptionsActions } from 'src/app/domains/aircraft-maintenance-company-option/store/aircraft-maintenance-company-options-actions';
import { getAllPlaneOptions } from 'src/app/domains/plane-option/store/plane-option.state';
import { DomainPlaneOptionsActions } from 'src/app/domains/plane-option/store/plane-options-actions';
import { getAllSiteOptions } from 'src/app/domains/site-option/store/site-option.state';
import { DomainSiteOptionsActions } from 'src/app/domains/site-option/store/site-options-actions';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceContractOptionsService extends CrudItemOptionsService {
  aircraftMaintenanceCompanyOptions$: Observable<OptionDto[]>;
  planeOptions$: Observable<OptionDto[]>;
  siteOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<AppState>) {
    super();
    // TODO after creation of CRUD MaintenanceContract : get all required option dto use in Table calc and create and edit form
    this.aircraftMaintenanceCompanyOptions$ = this.store.select(
      getAllAircraftMaintenanceCompanyOptions
    );
    this.planeOptions$ = this.store.select(getAllPlaneOptions);
    this.siteOptions$ = this.store.select(getAllSiteOptions);

    let cpt = 0;
    const aircraftMaintenanceCompany = cpt++;
    const plane = cpt++;
    const site = cpt++;

    this.dictOptionDtos$ = combineLatest([
      this.aircraftMaintenanceCompanyOptions$,
      this.planeOptions$,
      this.siteOptions$,
    ]).pipe(
      map(options => {
        return <DictOptionDto[]>[
          new DictOptionDto('planes', options[plane]),
          new DictOptionDto(
            'aircraftMaintenanceCompany',
            options[aircraftMaintenanceCompany]
          ),
          new DictOptionDto('site', options[site]),
        ];
      })
    );
  }

  loadAllOptions() {
    this.store.dispatch(
      DomainAircraftMaintenanceCompanyOptionsActions.loadAll()
    );
    this.store.dispatch(DomainPlaneOptionsActions.loadAll());
    this.store.dispatch(DomainSiteOptionsActions.loadAll());
  }
}
