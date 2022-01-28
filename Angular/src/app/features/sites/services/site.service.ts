import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { Site } from '../model/site/site';
import { getCurrentSite, getSiteLoadingGet } from '../store/site.state';
import { load } from '../store/sites-actions';

@Injectable({
    providedIn: 'root'
})
export class SiteService {
    constructor(
        private store: Store<AppState>,
    ) {
        this.InitSub();
        this.loading$ = this.store.select(getSiteLoadingGet);
    }
    private _currentSite: Site;
    private _currentSiteId: number;
    private sub = new Subscription();
    public loading$: Observable<boolean>;

    public get currentSite() {
        if (this._currentSite?.id === this._currentSiteId) {
            return this._currentSite;
        } else {
            return null;
        }
    }

    public get currentSiteId(): number {
        return this._currentSiteId;
    }
    public set currentSiteId(id: number) {
        this._currentSiteId = Number(id);
        this.store.dispatch(load({ id: id }));
    }

    InitSub() {
        this.sub = new Subscription();
        this.sub.add(
            this.store.select(getCurrentSite).subscribe((site) => {
                if (site) {
                    this._currentSite = site;
                }
            })
        );
    }
}
