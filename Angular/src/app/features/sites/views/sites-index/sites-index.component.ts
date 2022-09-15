import { Component, Injector, ViewChild } from '@angular/core';
import { Site } from '../../model/site';
import { SiteCRUDConfiguration } from '../../site.constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { SiteService } from '../../services/site.service';
import { SiteTableComponent } from '../../components/site-table/site-table.component';
import { SiteAdvancedFilter } from '../../model/site-advanced-filter';
import { DEFAULT_VIEW } from 'src/app/shared/constants';
import { DomainUserOptionsActions } from 'src/app/domains/bia-domains/user-option/store/user-options-actions';
import { getAllUserOptions } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { Observable } from 'rxjs';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { LazyLoadEvent } from 'primeng/api';

@Component({
  selector: 'app-sites-index',
  templateUrl: './sites-index.component.html',
  styleUrls: ['./sites-index.component.scss']
})

export class SitesIndexComponent extends CrudItemsIndexComponent<Site> {
  
  showFilter = false;
  haveFilter = false;
  userOptions$: Observable<OptionDto[]>;
  
  onFilter(advancedFilter: SiteAdvancedFilter) {
    this.crudItemListComponent.advancedFilter = advancedFilter;
    this.haveFilter = advancedFilter && advancedFilter.userId > 0;
    this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata());
  }

  onViewChange(viewPreference: string) {
    this.updateAdvancedFilterByView(viewPreference);
    super.onViewChange(viewPreference)
  }

  private updateAdvancedFilterByView(viewPreference: string) {
    let advancedFilter: SiteAdvancedFilter = <SiteAdvancedFilter>{};
    let haveFilter = false;

    if (viewPreference && viewPreference !== DEFAULT_VIEW) {
      const state = JSON.parse(viewPreference);
      if (state && state.advancedFilter) {
        advancedFilter = state.advancedFilter;
        haveFilter = this.crudItemListComponent.advancedFilter && this.crudItemListComponent.advancedFilter.userId > 0;
      }
    }

    this.crudItemListComponent.advancedFilter = advancedFilter;
    this.haveFilter = haveFilter;
  }

  onCloseFilter() {
    this.showFilter = false;
  }

  onOpenFilter() {
    this.showFilter = true;
  }

  onSearchUsers(value: string) {
    this.store.dispatch(DomainUserOptionsActions.loadAllByFilter({ filter: value }));
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    const userId: number = this.crudItemListComponent.advancedFilter && this.crudItemListComponent.advancedFilter.userId > 0 ? this.crudItemListComponent.advancedFilter.userId : 0;
    const customEvent: any = { userId: userId, ...lazyLoadEvent };
    super.onLoadLazy(customEvent)
  }
  
  private initUsers() {
    this.userOptions$ = this.store.select(getAllUserOptions);
  }

  ngOnInit() {
    super.ngOnInit();
    this.initUsers();
  }
  
  @ViewChild(SiteTableComponent, { static: false }) crudItemTableComponent: SiteTableComponent;

  constructor(
    protected injector: Injector,
    public siteService: SiteService,
    protected authService: AuthService,
  ) {
    super(injector, siteService);
    this.crudConfiguration = SiteCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Site_Update);
    this.canDelete = this.authService.hasPermission(Permission.Site_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Site_Create);
  }
  
  // Custo for teams
  onClickRow(crudItemId: any) {
    this.onManageMember(crudItemId)
  }

  onManageMember(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate(['sites', crudItemId, 'members']);
    }
  }
}
