import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { MaintenanceTeam } from '../model/maintenance-team';
import { getCurrentMaintenanceTeam, getMaintenanceTeamLoadingGet } from '../store/maintenance-team.state';
import { FeatureMaintenanceTeamsActions } from '../store/maintenance-teams-actions';
import { TeamTypeId } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class MaintenanceTeamService {
    constructor(
        private store: Store<AppState>,
        private authService: AuthService,
    ) {
        this.InitSub();
        this.loading$ = this.store.select(getMaintenanceTeamLoadingGet);
        this.maintenanceTeam$ = this.store.select(getCurrentMaintenanceTeam);
    }
    private _currentMaintenanceTeam: MaintenanceTeam;
    private _currentMaintenanceTeamId: number;
    private sub = new Subscription();
    public loading$: Observable<boolean>;
    public maintenanceTeam$: Observable<MaintenanceTeam>;

    public get currentMaintenanceTeam() {
        if (this._currentMaintenanceTeam?.id === this._currentMaintenanceTeamId) {
            return this._currentMaintenanceTeam;
        } else {
            return null;
        }
    }

    public get currentMaintenanceTeamId(): number {
        return this._currentMaintenanceTeamId;
    }
    public set currentMaintenanceTeamId(id: number) {
        if (this._currentMaintenanceTeamId !== id)
        {
            this._currentMaintenanceTeamId = Number(id);
            this.authService.changeCurrentTeamId(TeamTypeId.MaintenanceTeam, id);
            this.store.dispatch(FeatureMaintenanceTeamsActions.load({ id: id }));
        }
    }

    InitSub() {
        this.sub = new Subscription();
        this.sub.add(
            this.store.select(getCurrentMaintenanceTeam).subscribe((maintenanceTeam) => {
                if (maintenanceTeam) {
                    this._currentMaintenanceTeam = maintenanceTeam;
                }
            })
        );
    }
}
