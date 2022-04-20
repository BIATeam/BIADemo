import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { User } from '../model/user';
import { getCurrentUser, getUserLoadingGet } from '../store/user.state';
import { FeatureUsersActions } from '../store/users-actions';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(
        private store: Store<AppState>,
    ) {
        this.InitSub();
        this.loading$ = this.store.select(getUserLoadingGet);
        this.user$ = this.store.select(getCurrentUser);
    }
    private _currentUser: User;
    private _currentUserId: number;
    private sub = new Subscription();
    public loading$: Observable<boolean>;
    public user$: Observable<User>;

    public get currentUser() {
        if (this._currentUser?.id === this._currentUserId) {
            return this._currentUser;
        } else {
            return null;
        }
    }

    public get currentUserId(): number {
        return this._currentUserId;
    }
    public set currentUserId(id: number) {
        this._currentUserId = Number(id);
        this.store.dispatch(FeatureUsersActions.load({ id: id }));
    }

    InitSub() {
        this.sub = new Subscription();
        this.sub.add(
            this.store.select(getCurrentUser).subscribe((user) => {
                if (user) {
                    this._currentUser = user;
                }
            })
        );
    }
}
