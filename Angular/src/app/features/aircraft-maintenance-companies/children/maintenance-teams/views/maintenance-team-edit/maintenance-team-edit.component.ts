import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { FeatureMaintenanceTeamsActions } from '../../store/maintenance-teams-actions';
import { Subscription } from 'rxjs';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { AppState } from 'src/app/store/state';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MaintenanceTeamOptionsService } from '../../services/maintenance-team-options.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';

@Component({
  selector: 'app-maintenance-team-edit',
  templateUrl: './maintenance-team-edit.component.html',
  styleUrls: ['./maintenance-team-edit.component.scss']
})
export class MaintenanceTeamEditComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  private sub = new Subscription();

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public maintenanceTeamOptionsService: MaintenanceTeamOptionsService,
    public maintenanceTeamService: MaintenanceTeamService,
    private biaTranslationService: BiaTranslationService,
  ) { }

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(event => {
          this.maintenanceTeamOptionsService.loadAllOptions();
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(maintenanceTeamToUpdate: MaintenanceTeam) {
    this.store.dispatch(FeatureMaintenanceTeamsActions.update({ maintenanceTeam: maintenanceTeamToUpdate }));
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }
}
