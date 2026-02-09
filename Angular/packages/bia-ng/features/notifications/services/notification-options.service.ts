import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  DomainLanguageOptionsActions,
  DomainLanguageOptionsStore,
  DomainNotificationTypeOptionsActions,
  DomainNotificationTypesStore,
  DomainRoleOptionsActions,
  DomainRoleOptionsStore,
  DomainTeamOptionsActions,
  DomainTeamOptionsStore,
  DomainUserOptionsActions,
  DomainUserOptionsStore,
} from 'packages/bia-ng/domains/public-api';
import { BiaTeamTypeId } from 'packages/bia-ng/models/enum/public-api';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import {
  CrudItemOptionsService,
  DictOptionDto,
} from 'packages/bia-ng/shared/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class NotificationOptionsService extends CrudItemOptionsService {
  notificationTypeOptions$: Observable<OptionDto[]>;
  roleOptions$: Observable<OptionDto[]>;
  userOptions$: Observable<OptionDto[]>;
  teamOptions$: Observable<OptionDto[]>;
  languageOptions$: Observable<OptionDto[]>;

  constructor(protected store: Store<BiaAppState>) {
    super();
    this.notificationTypeOptions$ = this.store.select(
      DomainNotificationTypesStore.getAllNotificationTypeOptions
    );
    this.roleOptions$ = this.store.select(
      DomainRoleOptionsStore.getAllRoleOptions
    );
    this.userOptions$ = this.store.select(
      DomainUserOptionsStore.getAllUserOptions
    );
    this.teamOptions$ = this.store.select(
      DomainTeamOptionsStore.getAllTeamOptions
    );
    this.languageOptions$ = this.store.select(
      DomainLanguageOptionsStore.getAllLanguageOptions
    );

    // [Calc] Dict is used in calc mode only. It map the column name with the list OptionDto.
    this.dictOptionDtos$ = combineLatest([
      this.notificationTypeOptions$,
      this.roleOptions$,
      this.userOptions$,
      this.teamOptions$,
      this.languageOptions$,
    ]).pipe(
      map(
        options =>
          <DictOptionDto[]>[
            new DictOptionDto('type', options[0]),
            new DictOptionDto('notifiedRoles', options[1]),
            new DictOptionDto('notifiedUsers', options[2]),
            new DictOptionDto('createdBy', options[2]),
            new DictOptionDto('team', options[3]),
            new DictOptionDto('language', options[4]),
          ]
      )
    );
  }

  loadAllOptions() {
    this.store.dispatch(DomainNotificationTypeOptionsActions.loadAll());
    this.store.dispatch(
      DomainRoleOptionsActions.loadAll({ teamTypeId: BiaTeamTypeId.All })
    );
    this.store.dispatch(DomainUserOptionsActions.loadAll());
    this.store.dispatch(DomainLanguageOptionsActions.loadAll());
    this.store.dispatch(DomainTeamOptionsActions.loadAll());
  }
}
