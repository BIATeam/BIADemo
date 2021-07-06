import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllUsers, getUsersTotalCount } from '../../store/user.state';
import { multiRemove, loadAllByPost, synchronize } from '../../store/users-actions';
import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { User } from 'src/app/domains/user/model/user';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { BiaListConfig, PrimeTableColumn } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';

@Component({
  selector: 'app-users-index',
  templateUrl: './users-index.component.html',
  styleUrls: ['./users-index.component.scss']
})
export class UsersIndexComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) userListComponent: BiaTableComponent;
  showColSearch = false;
  globalSearchValue = '';
  pageSize = DEFAULT_PAGE_SIZE;
  totalRecords: number;
  users$: Observable<User[]>;
  selectedUsers: User[];
  totalCount$: Observable<number>;
  displayEditUserDialog = false;
  displayNewUserDialog = false;
  canSync = false;
  canDelete = false;
  canAdd = false;
  viewPreference: string;

  tableConfiguration: BiaListConfig = {
    columns: [
      new PrimeTableColumn('lastName', 'user.lastName'),
      new PrimeTableColumn('firstName', 'user.firstName'),
      new PrimeTableColumn('login', 'user.login')
    ]
  };

  columns: KeyValuePair[] = this.tableConfiguration.columns.map(
    (col) => <KeyValuePair>{ key: col.field, value: col.header }
  );
  displayedColumns: KeyValuePair[] = this.columns;
  lastLazyLoadEvent: LazyLoadEvent;

  constructor(
    private store: Store<AppState>,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.setPermissions();
    this.users$ = this.store.select(getAllUsers).pipe();
    this.totalCount$ = this.store.select(getUsersTotalCount).pipe();
  }

  onCreate() {
    this.displayNewUserDialog = true;
  }

  onDelete() {
    if (this.selectedUsers) {
      this.store.dispatch(multiRemove({ ids: this.selectedUsers.map((x) => x.id) }));
    }
  }

onSelectedElementsChanged(planesTypes: User[]) {
    this.selectedUsers = planesTypes;
  }


  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    this.lastLazyLoadEvent = lazyLoadEvent;
    this.store.dispatch(loadAllByPost({ event: lazyLoadEvent }));
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

  onSynchronize() {
    this.store.dispatch(synchronize());
  }

  onViewChange(viewPreference: string) {
    this.viewPreference = viewPreference;
  }

  private setPermissions() {
    this.canSync = this.authService.hasPermission(Permission.User_Sync);
    this.canDelete = this.authService.hasPermission(Permission.User_Delete);
    this.canAdd = this.authService.hasPermission(Permission.User_Add);
  }
}
