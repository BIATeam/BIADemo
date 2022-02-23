import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { FeatureAircraftMaintenanceCompaniesActions } from '../../store/aircraft-maintenance-companies-actions';
import { Subscription } from 'rxjs';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AppState } from 'src/app/store/state';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AircraftMaintenanceCompanyOptionsService } from '../../services/aircraft-maintenance-company-options.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';

@Component({
  selector: 'app-aircraft-maintenance-company-edit',
  templateUrl: './aircraft-maintenance-company-edit.component.html',
  styleUrls: ['./aircraft-maintenance-company-edit.component.scss']
})
export class AircraftMaintenanceCompanyEditComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  private sub = new Subscription();

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public aircraftMaintenanceCompanyOptionsService: AircraftMaintenanceCompanyOptionsService,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
    private biaTranslationService: BiaTranslationService,
  ) { }

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

  onSubmitted(aircraftMaintenanceCompanyToUpdate: AircraftMaintenanceCompany) {
    this.store.dispatch(FeatureAircraftMaintenanceCompaniesActions.update({ aircraftMaintenanceCompany: aircraftMaintenanceCompanyToUpdate }));
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }
}
