import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { AppState } from 'src/app/store/state';
import { Member, Members } from '../model/member';
import { MemberCRUDConfiguration } from '../member.constants';
import { FeatureMembersStore } from '../store/member.state';
import { FeatureMembersActions } from '../store/members-actions';
import { MemberOptionsService } from './member-options.service';
import { MemberDas } from './member-das.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';

@Injectable({
    providedIn: 'root'
})
export class MemberService extends CrudItemService<Member> {

    constructor(private store: Store<AppState>,
        public dasService: MemberDas,
        public signalRService: CrudItemSignalRService<Member>,
        public optionsService: MemberOptionsService,
        // requiered only for parent key
        protected authService: AuthService,
        ) {
        super(dasService,signalRService,optionsService);
    }
    
    teamTypeId: number;

    public getParentIds(): any[]
    {
        // TODO after creation of CRUD Member : adapt the parent Key tothe context. It can be null if root crud
        return [this.authService.getCurrentTeamId(this.teamTypeId)];
    }

    public getFeatureName()  {  return MemberCRUDConfiguration.featureName; };

    public crudItems$: Observable<Member[]> = this.store.select(FeatureMembersStore.getAllMembers);
    public totalCount$: Observable<number> = this.store.select(FeatureMembersStore.getMembersTotalCount);
    public loadingGetAll$: Observable<boolean> = this.store.select(FeatureMembersStore.getMemberLoadingGetAll);;
    public lastLazyLoadEvent$: Observable<LazyLoadEvent> = this.store.select(FeatureMembersStore.getLastLazyLoadEvent);

    public crudItem$: Observable<Member> = this.store.select(FeatureMembersStore.getCurrentMember);
    public loadingGet$: Observable<boolean> = this.store.select(FeatureMembersStore.getMemberLoadingGet);

    public load(id: any){
        this.store.dispatch(FeatureMembersActions.load({ id }));
    }
    public loadAllByPost(event: LazyLoadEvent){
        this.store.dispatch(FeatureMembersActions.loadAllByPost({ event }));
    }
    public create(crudItem: Member){
        // TODO after creation of CRUD Member : map parent Key on the corresponding field
        crudItem.teamId = this.getParentIds()[0],
        this.store.dispatch(FeatureMembersActions.create({ member : crudItem }));
    }
    public createMulti(membersToCreate: Members){
        // TODO after creation of CRUD Member : map parent Key on the corresponding field
        membersToCreate.teamId = this.getParentIds()[0],
        this.store.dispatch(FeatureMembersActions.createMulti({ members: membersToCreate }));
    }
    public update(crudItem: Member){
        this.store.dispatch(FeatureMembersActions.update({ member : crudItem }));
    }
    public remove(id: any){
        this.store.dispatch(FeatureMembersActions.remove({ id }));
    }
    public multiRemove(ids: any[]){
        this.store.dispatch(FeatureMembersActions.multiRemove({ ids }));
    }
}
