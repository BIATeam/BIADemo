import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { Member } from '../model/member';
import { getCurrentMember, getMemberLoadingGet } from '../store/member.state';
import { load } from '../store/members-actions';

@Injectable({
    providedIn: 'root'
})
export class MemberService {
    constructor(
        private store: Store<AppState>,
    ) {
        this.InitSub();
        this.loading$ = this.store.select(getMemberLoadingGet);
        this.member$ = this.store.select(getCurrentMember);
    }
    private _currentMember: Member;
    private _currentMemberId: number;
    private sub = new Subscription();
    public loading$: Observable<boolean>;
    public member$: Observable<Member>;

    public get currentMember() {
        if (this._currentMember?.id === this._currentMemberId) {
            return this._currentMember;
        } else {
            return null;
        }
    }

    public get currentMemberId(): number {
        return this._currentMemberId;
    }
    public set currentMemberId(id: number) {
        this._currentMemberId = Number(id);
        this.store.dispatch(load({ id: id }));
    }

    InitSub() {
        this.sub = new Subscription();
        this.sub.add(
            this.store.select(getCurrentMember).subscribe((member) => {
                if (member) {
                    this._currentMember = member;
                }
            })
        );
    }
}
