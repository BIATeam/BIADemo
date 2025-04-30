import {
  AsyncPipe,
  NgClass,
  NgIf,
  NgSwitch,
  NgSwitchCase,
} from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { skip } from 'rxjs/operators';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import { getLastUsersAdded } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { Permission } from 'src/app/shared/permission';
import { UserAddFromLdapComponent } from '../../../users-from-directory/views/user-add-from-directory-dialog/user-add-from-directory-dialog.component';
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
    NgSwitch,
    NgSwitchCase,
    PrimeTemplate,
    NgIf,
    ButtonDirective,
    UserTableComponent,
    UserAddFromLdapComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
    UserTeamsComponent,
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
    this.canEdit = this.authService.hasPermission(Permission.User_UpdateRoles);
    this.canDelete = this.authService.hasPermission(Permission.User_Delete);
    this.canAdd = this.authService.hasPermission(Permission.User_Add);
    this.canSave = this.authService.hasPermission(Permission.User_Save);
  }
  ngOnInit() {
    super.ngOnInit();

    this.sub.add(
      this.store
        .select(getLastUsersAdded)
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
