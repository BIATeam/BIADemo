import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { getCurrentAircraftMaintenanceCompany} from '../../store/aircraft-maintenance-company.state';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute } from '@angular/router';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';

@Component({
  templateUrl: './aircraft-maintenance-company-item.component.html',
  styleUrls: ['./aircraft-maintenance-company-item.component.scss']
})
export class AircraftMaintenanceCompanyItemComponent implements OnInit, OnDestroy {
  aircraftMaintenanceCompany$: Observable<AircraftMaintenanceCompany>;
  private sub = new Subscription();

  constructor(private store: Store<AppState>,
    private route: ActivatedRoute,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
    private layoutService: BiaClassicLayoutService,
    private biaTranslationService: BiaTranslationService,
  ) { }

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(event => {
        this.aircraftMaintenanceCompanyService.currentAircraftMaintenanceCompanyId = this.route.snapshot.params.aircraftMaintenanceCompanyId;
      })
    );
    this.sub.add
      (
        this.store.select(getCurrentAircraftMaintenanceCompany).subscribe((aircraftMaintenanceCompany) => {
          if (aircraftMaintenanceCompany?.title) {
            this.route.data.pipe(first()).subscribe(routeData => {
              routeData['breadcrumb'] = aircraftMaintenanceCompany.title;
            });
            this.layoutService.refreshBreadcrumb();
          }
        })
      );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}
