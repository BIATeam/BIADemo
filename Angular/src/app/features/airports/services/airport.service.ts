import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { Airport } from '../model/airport';
import { getCurrentAirport, getAirportLoadingGet } from '../store/airport.state';
import { load } from '../store/airports-actions';

@Injectable({
    providedIn: 'root'
})
export class AirportService {
    constructor(
        private store: Store<AppState>,
    ) {
        this.InitSub();
        this.loading$ = this.store.select(getAirportLoadingGet);
        this.airport$ = this.store.select(getCurrentAirport);
    }
    private _currentAirport: Airport;
    private _currentAirportId: number;
    private sub = new Subscription();
    public loading$: Observable<boolean>;
    public airport$: Observable<Airport>;

    public get currentAirport() {
        if (this._currentAirport?.id === this._currentAirportId) {
            return this._currentAirport;
        } else {
            return null;
        }
    }

    public get currentAirportId(): number {
        return this._currentAirportId;
    }
    public set currentAirportId(id: number) {
        this._currentAirportId = Number(id);
        this.store.dispatch(load({ id: id }));
    }

    InitSub() {
        this.sub = new Subscription();
        this.sub.add(
            this.store.select(getCurrentAirport).subscribe((airport) => {
                if (airport) {
                    this._currentAirport = airport;
                }
            })
        );
    }
}
