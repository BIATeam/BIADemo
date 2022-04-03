import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { FeatureMaintenanceTeamsActions } from '../../store/maintenance-teams-actions';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { AppState } from 'src/app/store/state';
import { MaintenanceTeamOptionsService } from '../../services/maintenance-team-options.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-maintenance-team-new',
  templateUrl: './maintenance-team-new.component.html',
  styleUrls: ['./maintenance-team-new.component.scss']
})
export class MaintenanceTeamNewComponent implements OnInit, OnDestroy  {
  private sub = new Subscription();

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public maintenanceTeamOptionsService: MaintenanceTeamOptionsService,
    private biaTranslationService: BiaTranslationService,

  ) {}

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

  onSubmitted(maintenanceTeamToCreate: MaintenanceTeam) {
    this.store.dispatch(FeatureMaintenanceTeamsActions.create({ maintenanceTeam: maintenanceTeamToCreate }));
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
