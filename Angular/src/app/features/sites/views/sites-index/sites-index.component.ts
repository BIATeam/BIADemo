import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Injector, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import {
  BiaButtonGroupComponent,
  BiaButtonGroupItem,
} from 'src/app/shared/bia-shared/components/bia-button-group/bia-button-group.component';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { TeamAdvancedFilterComponent } from 'src/app/shared/bia-shared/components/team-advanced-filter/team-advanced-filter.component';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { TeamAdvancedFilterDto } from 'src/app/shared/bia-shared/model/team-advanced-filter-dto';
import { Permission } from 'src/app/shared/permission';
import { SiteTableComponent } from '../../components/site-table/site-table.component';
import { Site } from '../../model/site';
import { SiteService } from '../../services/site.service';
import { siteCRUDConfiguration } from '../../site.constants';

@Component({
  selector: 'app-sites-index',
  templateUrl: './sites-index.component.html',
  styleUrls: ['./sites-index.component.scss'],
  imports: [
    NgIf,
    NgClass,
    PrimeTemplate,
    SiteTableComponent,
    AsyncPipe,
    TranslateModule,
    TeamAdvancedFilterComponent,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
    BiaButtonGroupComponent,
  ],
  providers: [{ provide: CrudItemService, useExisting: SiteService }],
})
export class SitesIndexComponent extends CrudItemsIndexComponent<Site> {
  // Customization for teams
  canViewMembers = false;
  canSelectElement = false;
  // BIAToolKit - Begin SiteIndexTsCanViewChildDeclaration
  // BIAToolKit - End SiteIndexTsCanViewChildDeclaration

  checkhasAdvancedFilter() {
    this.hasAdvancedFilter = TeamAdvancedFilterDto.hasFilter(
      this.crudConfiguration.fieldsConfig.advancedFilter
    );
  }

  @ViewChild(SiteTableComponent, { static: false })
  crudItemTableComponent: SiteTableComponent;

  constructor(
    protected injector: Injector,
    public siteService: SiteService,
    protected authService: AuthService
  ) {
    super(injector, siteService);
    this.crudConfiguration = siteCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Site_Update);
    this.canDelete = this.authService.hasPermission(Permission.Site_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Site_Create);
    // Customization for teams
    this.canViewMembers = this.authService.hasPermission(
      Permission.Site_Member_List_Access
    );
    // BIAToolKit - Begin SiteIndexTsCanViewChildSet
    // BIAToolKit - End SiteIndexTsCanViewChildSet
    this.canSelectElement =
      // BIAToolKit - Begin SiteIndexTsCanSelectElementChildSet
      // BIAToolKit - End SiteIndexTsCanSelectElementChildSet
      this.canViewMembers || this.canDelete;
  }

  protected initSelectedButtonGroup() {
    this.selectedButtonGroup = [
      new BiaButtonGroupItem(
        this.translateService.instant('site.edit'),
        () => this.onEdit(this.selectedCrudItems[0].id),
        this.canEdit,
        this.selectedCrudItems.length !== 1,
        this.translateService.instant('site.edit')
      ),
      // BIAToolKit - Begin SiteIndexTsChildTeamButton
      // BIAToolKit - End SiteIndexTsChildTeamButton
      new BiaButtonGroupItem(
        this.translateService.instant('app.members'),
        () => this.onViewMembers(this.selectedCrudItems[0].id),
        this.canViewMembers,
        this.selectedCrudItems.length !== 1 ||
          !this.selectedCrudItems[0].canMemberListAccess,
        this.translateService.instant('app.members')
      ),
    ];
  }

  // Customization for teams
  onClickRowData(crudItem: Site) {
    if (crudItem.canMemberListAccess) {
      this.onViewMembers(crudItem.id);
    }
  }

  onViewMembers(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'members'], {
        relativeTo: this.activatedRoute,
      });
    }
  }

  onSelectedElementsChanged(crudItems: Site[]) {
    super.onSelectedElementsChanged(crudItems);
    if (crudItems.length === 1) {
      this.siteService.currentCrudItemId = crudItems[0].id;
    }
  }

  onDelete(): void {
    super.onDelete();
    this.authService.reLogin();
  }

  // BIAToolKit - Begin SiteIndexTsOnViewChild
  // BIAToolKit - End SiteIndexTsOnViewChild
}
