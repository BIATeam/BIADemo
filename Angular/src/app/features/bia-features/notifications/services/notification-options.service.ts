import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllLanguageOptions } from 'src/app/domains/bia-domains/language-option/store/language-option.state';
import { DomainLanguageOptionsActions } from 'src/app/domains/bia-domains/language-option/store/language-options-actions';
import { getAllNotificationTypeOptions } from 'src/app/domains/bia-domains/notification-type-option/store/notification-type-option.state';
import { DomainNotificationTypeOptionsActions } from 'src/app/domains/bia-domains/notification-type-option/store/notification-type-options-actions';
import { getAllRoleOptions } from 'src/app/domains/bia-domains/role-option/store/role-option.state';
import { DomainRoleOptionsActions } from 'src/app/domains/bia-domains/role-option/store/role-options-actions';
import { getAllTeamOptions } from 'src/app/domains/bia-domains/team-option/store/team-option.state';
import { DomainTeamOptionsActions } from 'src/app/domains/bia-domains/team-option/store/team-options-actions';
import { getAllUserOptions } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { DomainUserOptionsActions } from 'src/app/domains/bia-domains/user-option/store/user-options-actions';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class NotificationOptionsService extends CrudItemOptionsService {
  notificationTypeOptions$: Observable<OptionDto[]>;
  roleOptions$: Observable<OptionDto[]>;
  userOptions$: Observable<OptionDto[]>;
  teamOptions$: Observable<OptionDto[]>;
  languageOptions$: Observable<OptionDto[]>;

  constructor(protected store: Store<AppState>) {
    super();
    this.notificationTypeOptions$ = this.store.select(
      getAllNotificationTypeOptions
    );
    this.roleOptions$ = this.store.select(getAllRoleOptions);
    this.userOptions$ = this.store.select(getAllUserOptions);
    this.teamOptions$ = this.store.select(getAllTeamOptions);
    this.languageOptions$ = this.store.select(getAllLanguageOptions);

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
      DomainRoleOptionsActions.loadAll({ teamTypeId: TeamTypeId.All })
    );
    this.store.dispatch(DomainUserOptionsActions.loadAll());
    this.store.dispatch(DomainLanguageOptionsActions.loadAll());
    this.store.dispatch(DomainTeamOptionsActions.loadAll());
  }
}
