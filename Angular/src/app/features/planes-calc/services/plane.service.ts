import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { Plane } from '../model/plane';
import { getCurrentPlane, getPlaneLoadingGet } from '../store/plane.state';
import { load } from '../store/planes-actions';

@Injectable({
    providedIn: 'root'
})
export class PlaneService {
    constructor(
        private store: Store<AppState>,
    ) {
        this.InitSub();
        this.loading$ = this.store.select(getPlaneLoadingGet);
        this.plane$ = this.store.select(getCurrentPlane);
    }
    private _currentPlane: Plane;
    private _currentPlaneId: number;
    private sub = new Subscription();
    public loading$: Observable<boolean>;
    public plane$: Observable<Plane>;

    public get currentPlane() {
        if (this._currentPlane?.id === this._currentPlaneId) {
            return this._currentPlane;
        } else {
            return null;
        }
    }

    public get currentPlaneId(): number {
        return this._currentPlaneId;
    }
    public set currentPlaneId(id: number) {
        this._currentPlaneId = Number(id);
        this.store.dispatch(load({ id: id }));
    }

    InitSub() {
        this.sub = new Subscription();
        this.sub.add(
            this.store.select(getCurrentPlane).subscribe((plane) => {
                if (plane) {
                    this._currentPlane = plane;
                }
            })
        );
    }
}
