import { Component, HostBinding, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllUsers, getUsersTotalCount, getUserLoadingGetAll } from '../../store/user.state';
import { FeatureUsersActions } from '../../store/users-actions';
import { Observable, Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { User } from '../../model/user';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaFieldsConfig,
  BiaFieldConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserDas } from '../../services/user-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { UsersSignalRService } from '../../services/user-signalr.service';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { UserOptionsService } from '../../services/user-options.service';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { UserTableComponent } from 'src/app/features/bia-features/users/components/user-table/user-table.component';
import { useCalcMode, useSignalR, useView, useViewTeamWithTypeId } from '../../user.constants';
import { skip } from 'rxjs/operators';
import { getLastUsersAdded } from 'src/app/domains/bia-domains/user-option/store/user-option.state';

@Component({
  selector: 'bia-users-index',
  templateUrl: './users-index.component.html',
  styleUrls: ['./users-index.component.scss']
})
export class UsersIndexComponent implements OnInit, OnDestroy {
  useCalcMode = useCalcMode;
  useSignalR = useSignalR;
  useView = useView;
  useRefreshAtLanguageChange = false;

  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) biaTableComponent: BiaTableComponent;
  @ViewChild(UserTableComponent, { static: false }) userTableComponent: UserTableComponent;
  private get userListComponent() {
    if (this.biaTableComponent !== undefined) {
      return this.biaTableComponent;
    }
    return this.userTableComponent;
  }

  private sub = new Subscription();
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  users$: Observable<User[]>;
  selectedUsers: User[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  canSync = false;
  tableConfiguration: BiaFieldsConfig;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  popupTitle: string;
  tableStateKey = this.useView ? 'usersGrid' : undefined;
  useViewTeamWithTypeId = this.useView ? useViewTeamWithTypeId : null;
  parentIds: string[];

  displayUserAddFromDirectoryDialog = false;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    public activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private userDas: UserDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService,
    private usersSignalRService: UsersSignalRService,
    public userOptionsService: UserOptionsService,
  ) {
  }

  ngOnInit() {
    this.parentIds = [];
    this.sub = new Subscription();

    this.initTableConfiguration();
    this.setPermissions();
    this.users$ = this.store.select(getAllUsers);
    this.totalCount$ = this.store.select(getUsersTotalCount);
    this.loading$ = this.store.select(getUserLoadingGetAll);
    this.OnDisplay();
    if (this.useCalcMode) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(event => {
          this.userOptionsService.loadAllOptions();
        })
      );
    }
    if (this.useRefreshAtLanguageChange) {
      // Reload data if language change.
      this.sub.add(
        this.biaTranslationService.currentCulture$.pipe(skip(1)).subscribe(event => {
          this.onLoadLazy(this.userListComponent.getLazyLoadMetadata());
        })
      );
    }

    this.sub.add(
      this.store.select(getLastUsersAdded).pipe(skip(1)).subscribe(event => {
        setTimeout(() => this.onLoadLazy(this.userListComponent.getLazyLoadMetadata()));
      })
    )
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
    this.OnHide();
  }

  OnDisplay() {
    if (this.useView) {
      this.store.dispatch(loadAllView());
    }


    if (this.useSignalR) {
      this.usersSignalRService.initialize();
    }
  }

  OnHide() {
    if (this.useSignalR) {
      this.usersSignalRService.destroy();
    }
  }

  onCreate() {
    this.displayUserAddFromDirectoryDialog = true;
    /*if (!this.useCalcMode) {
      this.router.navigate(['create'], { relativeTo: this.activatedRoute });
    }*/
  }

  onEdit(userId: number) {
    if (!this.useCalcMode) {
      this.router.navigate([userId, 'edit'], { relativeTo: this.activatedRoute });
    }
  }

  onSave(user: User) {
    if (this.useCalcMode) {
      if (user?.id > 0) {
        if (this.canEdit) {
          this.store.dispatch(FeatureUsersActions.update({ user: user }));
        }
      } else {
        if (this.canAdd) {
          this.store.dispatch(FeatureUsersActions.create({ user: user }));
        }
      }
    }
  }

  onDelete() {
    if (this.selectedUsers && this.canDelete) {
      this.store.dispatch(FeatureUsersActions.multiRemove({ ids: this.selectedUsers.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(users: User[]) {
    this.selectedUsers = users;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    const pagingAndFilter: PagingFilterFormatDto = { parentIds: this.parentIds, ...lazyLoadEvent };
    this.store.dispatch(FeatureUsersActions.loadAllByPost({ event: pagingAndFilter }));
  }

  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: KeyValuePair[]) {
    this.displayedColumns = values;
  }

  onToggleSearch() {
    this.showColSearch = !this.showColSearch;
  }

  onViewChange(viewPreference: string) {
    this.viewPreference = viewPreference;
  }

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.userListComponent.getPrimeNgTable().columns.map((x: BiaFieldConfig) => (columns[x.field] = this.translateService.instant(x.header)));
    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds, columns: columns, ...this.userListComponent.getLazyLoadMetadata()
    };
    this.userDas.getFile(columnsAndFilter).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.users') + '.csv');
    });
  }

  onSynchronize() {
    this.store.dispatch(FeatureUsersActions.synchronize());
  }

  private setPermissions() {
    this.canSync = this.authService.hasPermission(Permission.User_Sync);
    this.canEdit = this.authService.hasPermission(Permission.User_UpdateRoles);
    this.canDelete = this.authService.hasPermission(Permission.User_Delete);
    this.canAdd = this.authService.hasPermission(Permission.User_Add);
  }

  private initTableConfiguration() {
    this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          new BiaFieldConfig('lastName', 'user.lastName'),
          new BiaFieldConfig('firstName', 'user.firstName'),
          new BiaFieldConfig('login', 'user.login'),
          Object.assign(new BiaFieldConfig('roles', 'member.roles'), {
            type: PropType.ManyToMany
          })
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    }));
  }
}
