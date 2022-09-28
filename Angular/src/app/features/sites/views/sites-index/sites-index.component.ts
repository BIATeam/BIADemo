import { Component, Injector, ViewChild } from '@angular/core';
import { Site } from '../../model/site';
import { SiteCRUDConfiguration } from '../../site.constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { SiteService } from '../../services/site.service';
import { SiteTableComponent } from '../../components/site-table/site-table.component';
import { SiteAdvancedFilter } from '../../model/site-advanced-filter';

@Component({
  selector: 'app-sites-index',
  templateUrl: './sites-index.component.html',
  styleUrls: ['./sites-index.component.scss']
})

export class SitesIndexComponent extends CrudItemsIndexComponent<Site> {
  // Custo for teams
  canManageMembers = false;

  checkHaveAdvancedFilter()
  {
    this.haveAdvancedFilter =  SiteAdvancedFilter.haveFilter(this.crudConfiguration.fieldsConfig.advancedFilter);
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
    // Custo for teams
    this.canManageMembers = this.authService.hasPermission(Permission.Site_Member_List_Access);
  }
  
  // Custo for teams
  onClickRow(crudItemId: any) {
    this.onManageMember(crudItemId)
  }

  onManageMember(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'members'], { relativeTo: this.activatedRoute });
    }
  }
}
