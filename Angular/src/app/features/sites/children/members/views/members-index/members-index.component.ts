import { Component, HostBinding, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable, of, Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { map } from 'rxjs/operators';
import { Member } from '../../../../children/members/model/member';
import { getAllMembers, getMembersTotalCount, getMemberLoadingGetAll } from '../../store/member.state';
import { multiRemove, loadAllByPost, load, openDialogNew, openDialogEdit } from '../../store/members-actions';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { BiaListConfig, PrimeTableColumn } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { Role } from 'src/app/domains/role/model/role';
import { getAllRoles } from 'src/app/domains/role/store/role.state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { TranslateService } from '@ngx-translate/core';
import { TranslateRoleLabelPipe } from 'src/app/shared/bia-shared/pipes/translate-role-label.pipe';
import { SiteService } from 'src/app/features/sites/services/site.service';

interface MemberListVM {
  id: number;
  displayName: string;
  roles: string;
  member: Member;
}

@Component({
  selector: 'app-members-index',
  templateUrl: './members-index.component.html',
  styleUrls: ['./members-index.component.scss']
})
export class MembersIndexComponent implements OnInit, OnDestroy {
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) memberListComponent: BiaTableComponent;
  loading$: Observable<boolean>;
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  roles: Role[];
  members$: Observable<MemberListVM[]>;
  selectedMembers: MemberListVM[];
  totalCount$: Observable<number>;
  private sub = new Subscription();
  headerTitle: string;
  canEdit = false;
  canDelete = false;
  canAdd = false;

  tableConfiguration: BiaListConfig;
  columns: KeyValuePair[] = [];
  displayedColumns: KeyValuePair[] = this.columns;

  constructor(
    private store: Store<AppState>,
    private authService: AuthService,
    private translate: TranslateService,
    private translateRoleLabelPipe: TranslateRoleLabelPipe,
    public siteService: SiteService
  ) {
  }

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();
    this.initRoles();
    this.initMembers();
    this.initTotalCount();
    this.initLoading();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onCreate() {
    this.store.dispatch(openDialogNew());
  }

  onEdit(memberId: number) {
    this.store.dispatch(load({ id: memberId }));
    this.store.dispatch(openDialogEdit());
  }

  onDelete() {
    if (this.selectedMembers) {
      this.store.dispatch(multiRemove({ ids: this.selectedMembers.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(planes: MemberListVM[]) {
    this.selectedMembers = planes;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    if (this.siteService.currentSiteId > 0) {
      const customEvent: any = { siteId: +this.siteService.currentSiteId, ...lazyLoadEvent };
      this.store.dispatch(loadAllByPost({ event: customEvent }));
    }
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

  toMemberListVM(objMember: Member, currentLang: string): MemberListVM {
    return {
      member: objMember,
      id: objMember.id,
      displayName: `${objMember.userLastName} ${objMember.userFirstName}`,
      roles: objMember.roles
        ? this.roles
          .filter((x: Role) => objMember.roles.some((y) => y.roleId === x.id))
          .map((role: Role) => this.translateRoleLabelPipe.transform(role, currentLang))
          .join(', ')
        : ''
    };
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Member_Update);
    this.canDelete = this.authService.hasPermission(Permission.Member_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Member_Save);
  }

  private initTableConfiguration() {
    this.tableConfiguration = {
      columns: [
        Object.assign(new PrimeTableColumn('displayName', 'member.user'), {
          isSortable: false,
          isSearchable: false
        }),
        Object.assign(new PrimeTableColumn('roles', 'member.rolesForSite'), {
          isSortable: false,
          isSearchable: false
        })
      ]
    };

    this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
    this.displayedColumns = [...this.columns];
  }

  private initLoading() {
    this.loading$ = this.store.select(getMemberLoadingGetAll);
  }

  private initTotalCount() {
    this.totalCount$ = this.store.select(getMembersTotalCount);
  }

  private initMembers() {
    this.members$ = combineLatest([this.store
      .select(getAllMembers), of(this.translate.currentLang)])
      .pipe(map((result) => result[0].map((member) => this.toMemberListVM(member, result[1]))));
  }

  private initRoles() {
    this.sub.add(
      this.store
        .select(getAllRoles)
        .subscribe((roles: Role[]) => (this.roles = roles))
    );
  }
}
