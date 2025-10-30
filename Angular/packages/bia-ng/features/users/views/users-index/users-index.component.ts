import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import {
  AppSettingsService,
  AuthService,
  BiaPermission,
} from 'packages/bia-ng/core/public-api';
import { DomainUserOptionsStore } from 'packages/bia-ng/domains/public-api';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
  UserAddFromLdapComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { skip } from 'rxjs/operators';
import { UserTableComponent } from '../../components/user-table/user-table.component';
import { UserTeamsComponent } from '../../components/user-teams/user-teams.component';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { FeatureUsersActions } from '../../store/users-actions';
import { userCRUDConfiguration } from '../../user.constants';

@Component({
  selector: 'bia-users-index',
  templateUrl: './users-index.component.html',
  styleUrls: ['./users-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    ButtonDirective,
    UserTableComponent,
    UserAddFromLdapComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
    UserTeamsComponent
],
  providers: [
    {
      provide: CrudItemService,
      useExisting: UserService,
    },
  ],
})
export class UsersIndexComponent
  extends CrudItemsIndexComponent<User>
  implements OnInit
{
  canSync = false;
  displayUserAddFromDirectoryDialog = false;

  @ViewChild(UserTableComponent, { static: false })
  crudItemTableComponent: UserTableComponent;

  constructor(
    protected injector: Injector,
    public userService: UserService,
    protected authService: AuthService,
    protected appSettingsService: AppSettingsService
  ) {
    super(injector, userService);
    this.useRefreshAtLanguageChange = true;
    this.crudConfiguration = userCRUDConfiguration;
  }

  protected setPermissions() {
    this.canSync = false; // This button is no longer useful with the UserInDB mode;
    this.canEdit = this.authService.hasPermission(
      BiaPermission.User_UpdateRoles
    );
    this.canDelete = this.authService.hasPermission(BiaPermission.User_Delete);
    this.canAdd = this.authService.hasPermission(BiaPermission.User_Add);
    this.canSave = this.authService.hasPermission(BiaPermission.User_Save);
  }
  ngOnInit() {
    super.ngOnInit();

    this.sub.add(
      this.store
        .select(DomainUserOptionsStore.getLastUsersAdded)
        .pipe(skip(1))
        .subscribe(() => {
          if (!userCRUDConfiguration.useSignalR) {
            setTimeout(() =>
              this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata())
            );
          }
        })
    );
  }
  onCreate() {
    this.displayUserAddFromDirectoryDialog = true;
    /*if (!this.useCalcMode) {
      this.router.navigate(['create'], { relativeTo: this.activatedRoute });
    }*/
  }

  onSynchronize() {
    this.store.dispatch(FeatureUsersActions.synchronize());
  }
}
