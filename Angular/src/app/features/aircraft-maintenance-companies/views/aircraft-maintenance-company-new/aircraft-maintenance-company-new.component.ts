import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { FeatureAircraftMaintenanceCompaniesActions } from '../../store/aircraft-maintenance-companies-actions';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AppState } from 'src/app/store/state';
import { AircraftMaintenanceCompanyOptionsService } from '../../services/aircraft-maintenance-company-options.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-aircraft-maintenance-company-new',
  templateUrl: './aircraft-maintenance-company-new.component.html',
  styleUrls: ['./aircraft-maintenance-company-new.component.scss']
})
export class AircraftMaintenanceCompanyNewComponent implements OnInit, OnDestroy  {
  private sub = new Subscription();

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public aircraftMaintenanceCompanyOptionsService: AircraftMaintenanceCompanyOptionsService,
    private biaTranslationService: BiaTranslationService,

  ) {}

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(event => {
          this.aircraftMaintenanceCompanyOptionsService.loadAllOptions();
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(aircraftMaintenanceCompanyToCreate: AircraftMaintenanceCompany) {
    this.store.dispatch(FeatureAircraftMaintenanceCompaniesActions.create({ aircraftMaintenanceCompany: aircraftMaintenanceCompanyToCreate }));
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
