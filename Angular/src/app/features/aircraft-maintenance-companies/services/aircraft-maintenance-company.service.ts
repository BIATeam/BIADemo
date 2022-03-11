import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import { getCurrentAircraftMaintenanceCompany, getAircraftMaintenanceCompanyLoadingGet } from '../store/aircraft-maintenance-company.state';
import { FeatureAircraftMaintenanceCompaniesActions } from '../store/aircraft-maintenance-companies-actions';
import { TeamTypeId } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class AircraftMaintenanceCompanyService {
    constructor(
        private store: Store<AppState>,
        private authService: AuthService,
    ) {
        this.InitSub();
        this.loading$ = this.store.select(getAircraftMaintenanceCompanyLoadingGet);
        this.aircraftMaintenanceCompany$ = this.store.select(getCurrentAircraftMaintenanceCompany);
    }
    private _currentAircraftMaintenanceCompany: AircraftMaintenanceCompany;
    private _currentAircraftMaintenanceCompanyId: number;
    private sub = new Subscription();
    public loading$: Observable<boolean>;
    public aircraftMaintenanceCompany$: Observable<AircraftMaintenanceCompany>;

    public get currentAircraftMaintenanceCompany() {
        if (this._currentAircraftMaintenanceCompany?.id === this._currentAircraftMaintenanceCompanyId) {
            return this._currentAircraftMaintenanceCompany;
        } else {
            return null;
        }
    }

    public get currentAircraftMaintenanceCompanyId(): number {
        return this._currentAircraftMaintenanceCompanyId;
    }
    public set currentAircraftMaintenanceCompanyId(id: number) {
        if (this._currentAircraftMaintenanceCompanyId !== id)
        {
            this._currentAircraftMaintenanceCompanyId = Number(id);
            this.authService.changeCurrentTeamId(TeamTypeId.AircraftMaintenanceCompany, id);
            this.store.dispatch(FeatureAircraftMaintenanceCompaniesActions.load({ id: id }));
        }
    }

    InitSub() {
        this.sub = new Subscription();
        this.sub.add(
            this.store.select(getCurrentAircraftMaintenanceCompany).subscribe((aircraftMaintenanceCompany) => {
                if (aircraftMaintenanceCompany) {
                    this._currentAircraftMaintenanceCompany = aircraftMaintenanceCompany;
                }
            })
        );
    }
}
