import { Component, HostBinding, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllSites, getSitesTotalCount, getSiteLoadingGetAll } from '../../store/site.state';
import { multiRemove, loadAllByPost, load, openDialogNew, openDialogEdit } from '../../store/sites-actions';
import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { map } from 'rxjs/operators';
import { SiteInfo } from '../../model/site/site-info';
import { SiteMember } from '../../model/site/site-member';
import { BiaListConfig, PrimeTableColumn } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { User } from 'src/app/domains/user/model/user';
import { loadAllByFilter } from 'src/app/domains/user/store/users-actions';
import { getAllUsers } from 'src/app/domains/user/store/user.state';
import { DEFAULT_PAGE_SIZE, DEFAULT_VIEW } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { SiteAdvancedFilter } from '../../model/site/site-advanced-filter';
import { ActivatedRoute, Router } from '@angular/router';

interface SiteListVM {
  id: number;
  title: string;
  siteAdmin: string;
}

@Component({
  selector: 'app-sites-index',
  templateUrl: './sites-index.component.html',
  styleUrls: ['./sites-index.component.scss']
})
export class SitesIndexComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  users$: Observable<User[]>;
  sites$: Observable<SiteListVM[]>;
  selectedSites: SiteListVM[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  showFilter = false;
  haveFilter = false;
  private lastLazyLoadEvent: LazyLoadEvent;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  canManageMembers = false;
  tableConfiguration: BiaListConfig;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  advancedFilter: SiteAdvancedFilter;

  constructor(private store: Store<AppState>, private authService: AuthService, private router: Router, public route: ActivatedRoute) { }

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();
    this.initUsers();
    this.initSites();
    this.initTotalCount();
    this.initLoading();
  }

  onCreate() {
    this.store.dispatch(openDialogNew());
  }

  onEdit() {
    if (this.selectedSites && this.selectedSites.length === 1) {
      this.store.dispatch(load({ id: this.selectedSites[0].id }));
      this.store.dispatch(openDialogEdit());
    }
  }

  onDelete() {
    if (this.selectedSites) {
      this.store.dispatch(multiRemove({ ids: this.selectedSites.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(planesTypes: SiteListVM[]) {
    this.selectedSites = planesTypes;
  }

  onManageMember(siteId: number) {
    if (siteId && siteId > 0) {
      this.router.navigate(['/sites', siteId, 'members']);
    }
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    this.lastLazyLoadEvent = lazyLoadEvent;

    const userId: number = this.advancedFilter && this.advancedFilter.userId > 0 ? this.advancedFilter.userId : 0;
    const customEvent: any = { userId: userId, ...lazyLoadEvent };

    this.store.dispatch(loadAllByPost({ event: customEvent }));
  }

  onSearchUsers(value: string) {
    this.store.dispatch(loadAllByFilter({ filter: value }));
  }

  onFilter(advancedFilter: SiteAdvancedFilter) {
    this.advancedFilter = advancedFilter;
    this.haveFilter = this.advancedFilter && this.advancedFilter.userId > 0;
    this.onLoadLazy(this.lastLazyLoadEvent);
  }

  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: KeyValuePair[]) {
    this.displayedColumns = values;
  }

  onCloseFilter() {
    this.showFilter = false;
  }

  onOpenFilter() {
    this.showFilter = true;
  }

  onToggleSearch() {
    this.showColSearch = !this.showColSearch;
  }

  toSiteListVM(site: SiteInfo): SiteListVM {
    return {
      id: site.id,
      title: site.title,
      siteAdmin: site.siteAdmins
        ? site.siteAdmins
          .map((siteMember: SiteMember) => `${siteMember.userLastName} ${siteMember.userFirstName}`)
          .join(', ')
        : ''
    };
  }

  onViewChange(viewPreference: string) {
    this.updateAdvancedFilterByView(viewPreference);
    this.viewPreference = viewPreference;
  }

  private updateAdvancedFilterByView(viewPreference: string) {
    let advancedFilter: SiteAdvancedFilter = <SiteAdvancedFilter>{};
    let haveFilter = false;

    if (viewPreference && viewPreference !== DEFAULT_VIEW) {
      const state = JSON.parse(viewPreference);
      if (state && state.advancedFilter) {
        advancedFilter = state.advancedFilter;
        haveFilter = this.advancedFilter && this.advancedFilter.userId > 0;
      }
    }

    this.advancedFilter = advancedFilter;
    this.haveFilter = haveFilter;
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Site_Update);
    this.canDelete = this.authService.hasPermission(Permission.Site_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Site_Create);
    this.canManageMembers = this.authService.hasPermission(Permission.Member_List_Access);
  }

  private initTableConfiguration() {
    this.tableConfiguration = {
      columns: [
        new PrimeTableColumn('title', 'site.title'),
        Object.assign(new PrimeTableColumn('siteAdmin', 'site.admins'), {
          isSortable: false,
          isSearchable: false
        })
      ]
    };
    this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
    this.displayedColumns = this.columns;
  }

  private initLoading() {
    this.loading$ = this.store.select(getSiteLoadingGetAll).pipe();
  }

  private initTotalCount() {
    this.totalCount$ = this.store.select(getSitesTotalCount).pipe();
  }

  private initSites() {
    this.sites$ = this.store
      .select(getAllSites)
      .pipe()
      .pipe(map((siteInfos) => siteInfos.map((siteInfo) => this.toSiteListVM(siteInfo))));
  }

  private initUsers() {
    this.users$ = this.store.select(getAllUsers);
  }
}
