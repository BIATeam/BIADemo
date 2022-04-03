import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { getCurrentMaintenanceTeam} from '../../store/maintenance-team.state';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute } from '@angular/router';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';
//import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';

@Component({
  templateUrl: './maintenance-team-item.component.html',
  styleUrls: ['./maintenance-team-item.component.scss']
})
export class MaintenanceTeamItemComponent implements OnInit, OnDestroy {
  maintenanceTeam$: Observable<MaintenanceTeam>;
  private sub = new Subscription();

  constructor(private store: Store<AppState>,
    private route: ActivatedRoute,
    public maintenanceTeamService: MaintenanceTeamService,
    private layoutService: BiaClassicLayoutService,
    //private biaTranslationService: BiaTranslationService,
  ) { }

  ngOnInit() {
    this.maintenanceTeamService.currentMaintenanceTeamId = this.route.snapshot.params.maintenanceTeamId;
    this.sub.add
      (
        this.store.select(getCurrentMaintenanceTeam).subscribe((maintenanceTeam) => {
          if (maintenanceTeam?.title) {
            this.route.data.pipe(first()).subscribe(routeData => {
              routeData['breadcrumb'] = maintenanceTeam.title;
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
