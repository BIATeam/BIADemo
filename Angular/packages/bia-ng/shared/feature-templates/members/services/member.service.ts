import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService } from 'packages/bia-ng/core/public-api';
import { BaseDto } from 'packages/bia-ng/models/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable } from 'rxjs';
import { CrudItemSignalRService } from '../../crud-items/services/crud-item-signalr.service';
import { CrudItemService } from '../../crud-items/services/crud-item.service';
import { memberCRUDConfiguration } from '../member.constants';
import { Member, Members } from '../model/member';
import { FeatureMembersStore } from '../store/member.state';
import { FeatureMembersActions } from '../store/members-actions';
import { MemberDas } from './member-das.service';
import { MemberOptionsService } from './member-options.service';

@Injectable({
  providedIn: 'root',
})
export class MemberService extends CrudItemService<Member> {
  constructor(
    protected store: Store<BiaAppState>,
    public dasService: MemberDas,
    public signalRService: CrudItemSignalRService<Member>,
    public optionsService: MemberOptionsService,
    protected injector: Injector,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService, injector);
    this.crudItems$ = this.store.select(FeatureMembersStore.getAllMembers);
    this.totalCount$ = this.store.select(
      FeatureMembersStore.getMembersTotalCount
    );
    this.loadingGetAll$ = this.store.select(
      FeatureMembersStore.getMemberLoadingGetAll
    );
    this.lastLazyLoadEvent$ = this.store.select(
      FeatureMembersStore.getLastLazyLoadEvent
    );

    this.crudItem$ = this.store.select(FeatureMembersStore.getCurrentMember);
    this.loadingGet$ = this.store.select(
      FeatureMembersStore.getMemberLoadingGet
    );
  }

  teamTypeId: number;

  public parentService: CrudItemService<BaseDto>;

  public getParentIds(): any[] {
    // TODO after creation of CRUD Member : adapt the parent Key tothe context. It can be null if root crud
    return [this.authService.getCurrentTeamId(this.teamTypeId)];
  }

  public getFeatureName() {
    return memberCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Member[]>;
  public totalCount$: Observable<number>;
  public loadingGetAll$: Observable<boolean>;
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent>;
  public crudItem$: Observable<Member>;
  public loadingGet$: Observable<boolean>;

  public load(id: any) {
    this.store.dispatch(FeatureMembersActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureMembersActions.loadAllByPost({ event }));
  }
  public create(crudItem: Member) {
    // TODO after creation of CRUD Member : map parent Key on the corresponding field
    crudItem.teamId = this.getParentIds()[0];
    this.store.dispatch(FeatureMembersActions.create({ member: crudItem }));
  }
  public save(crudItems: Member[]) {
    // TODO after creation of CRUD Member : map parent Key on the corresponding field
    crudItems.map(x => (x.teamId = this.getParentIds()[0]));
    this.store.dispatch(FeatureMembersActions.save({ members: crudItems }));
  }
  public createMulti(membersToCreate: Members) {
    // TODO after creation of CRUD Member : map parent Key on the corresponding field
    membersToCreate.teamId = this.getParentIds()[0];
    this.store.dispatch(
      FeatureMembersActions.createMulti({ members: membersToCreate })
    );
  }
  public update(crudItem: Member) {
    crudItem.teamId = this.getParentIds()[0];
    this.store.dispatch(FeatureMembersActions.update({ member: crudItem }));
  }
  public remove(id: any) {
    this.store.dispatch(FeatureMembersActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureMembersActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureMembersActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <Member>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureMembersActions.clearCurrent());
  }
}
