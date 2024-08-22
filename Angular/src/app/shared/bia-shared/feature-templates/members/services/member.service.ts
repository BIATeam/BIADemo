import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { AppState } from 'src/app/store/state';
import { Member, Members } from '../model/member';
import { memberCRUDConfiguration } from '../member.constants';
import { FeatureMembersStore } from '../store/member.state';
import { FeatureMembersActions } from '../store/members-actions';
import { MemberOptionsService } from './member-options.service';
import { MemberDas } from './member-das.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';

@Injectable({
  providedIn: 'root',
})
export class MemberService extends CrudItemService<Member> {
  constructor(
    protected store: Store<AppState>,
    public dasService: MemberDas,
    public signalRService: CrudItemSignalRService<Member>,
    public optionsService: MemberOptionsService,
    // requiered only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService);
  }

  teamTypeId: number;

  getParentIds(): any[] {
    // TODO after creation of CRUD Member : adapt the parent Key tothe context. It can be null if root crud
    return [this.authService.getCurrentTeamId(this.teamTypeId)];
  }

  getFeatureName() {
    return memberCRUDConfiguration.featureName;
  }

  crudItems$: Observable<Member[]> = this.store.select(
    FeatureMembersStore.getAllMembers
  );
  totalCount$: Observable<number> = this.store.select(
    FeatureMembersStore.getMembersTotalCount
  );
  loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureMembersStore.getMemberLoadingGetAll
  );
  lastLazyLoadEvent$: Observable<LazyLoadEvent> = this.store.select(
    FeatureMembersStore.getLastLazyLoadEvent
  );

  crudItem$: Observable<Member> = this.store.select(
    FeatureMembersStore.getCurrentMember
  );
  loadingGet$: Observable<boolean> = this.store.select(
    FeatureMembersStore.getMemberLoadingGet
  );

  load(id: any) {
    this.store.dispatch(FeatureMembersActions.load({ id }));
  }
  loadAllByPost(event: LazyLoadEvent) {
    this.store.dispatch(FeatureMembersActions.loadAllByPost({ event }));
  }
  create(crudItem: Member) {
    // TODO after creation of CRUD Member : map parent Key on the corresponding field
    (crudItem.teamId = this.getParentIds()[0]),
      this.store.dispatch(FeatureMembersActions.create({ member: crudItem }));
  }
  createMulti(membersToCreate: Members) {
    // TODO after creation of CRUD Member : map parent Key on the corresponding field
    (membersToCreate.teamId = this.getParentIds()[0]),
      this.store.dispatch(
        FeatureMembersActions.createMulti({ members: membersToCreate })
      );
  }
  update(crudItem: Member) {
    (crudItem.teamId = this.getParentIds()[0]),
      this.store.dispatch(FeatureMembersActions.update({ member: crudItem }));
  }
  remove(id: any) {
    this.store.dispatch(FeatureMembersActions.remove({ id }));
  }
  multiRemove(ids: any[]) {
    this.store.dispatch(FeatureMembersActions.multiRemove({ ids }));
  }
  clearAll() {
    this.store.dispatch(FeatureMembersActions.clearAll());
  }
  clearCurrent() {
    this._currentCrudItem = <Member>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureMembersActions.clearCurrent());
  }
}
