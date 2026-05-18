import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '@bia-team/bia-ng/core';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
} from '@bia-team/bia-ng/shared';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { Permission } from 'src/app/shared/permission';
import { PlaneTableComponent } from '../../components/plane-table/plane-table.component';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneOptionsService } from '../../services/plane-options.service';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-planes-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    PlaneTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [{ provide: CrudItemService, useExisting: PlaneService }],
})
export class PlanesIndexComponent
  extends CrudItemsIndexComponent<Plane>
  implements OnInit
{
  @ViewChild(PlaneTableComponent, { static: false })
  crudItemTableComponent: PlaneTableComponent;

  // BIAToolKit - Begin PlaneIndexTsCanViewChildDeclaration
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial PlaneIndexTsCanViewChildDeclaration Engine
  canViewEngines = false;
  // BIAToolKit - End Partial PlaneIndexTsCanViewChildDeclaration Engine
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End PlaneIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    protected planeOptionsService: PlaneOptionsService,
    protected authService: AuthService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
    this.canSave = this.authService.hasPermission(Permission.Plane_Save);
    this.canSelect = this.canDelete;
    this.canFix = this.authService.hasPermission(Permission.Plane_Fix);
    // BIAToolKit - Begin PlaneIndexTsCanViewChildSet
    // Begin BIAToolKit Generation Ignore
    // BIAToolKit - Begin Partial PlaneIndexTsCanViewChildSet Engine
    this.canViewEngines = this.authService.hasPermission(
      Permission.Engine_List_Access
    );
    this.canSelect = this.canSelect || this.canViewEngines;
    // BIAToolKit - End Partial PlaneIndexTsCanViewChildSet Engine
    // End BIAToolKit Generation Ignore
    // BIAToolKit - End PlaneIndexTsCanViewChildSet
  }

  // BIAToolKit - Begin PlaneIndexTsOnViewChild
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial PlaneIndexTsOnViewChild Engine
  onViewEngines(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'engines'], {
        relativeTo: this.activatedRoute,
      });
    }
  }
  // BIAToolKit - End Partial PlaneIndexTsOnViewChild Engine
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End PlaneIndexTsOnViewChild

  protected initSelectedButtonGroup() {
    this.selectionActionsMenuItems = [
      // BIAToolKit - Begin PlaneIndexTsSelectedButtonViewChild
      // Begin BIAToolKit Generation Ignore
      // BIAToolKit - Begin Partial PlaneIndexTsSelectedButtonViewChild Engine
      {
        visible: this.canViewEngines,
        disabled: this.selectedCrudItems.length !== 1,
        label: this.translateService.instant('plane.engines'),
        tooltip: this.translateService.instant('plane.engines'),
        command: () => this.onViewEngines(this.selectedCrudItems[0].id),
        buttonOutlined: true,
      },
      // BIAToolKit - End Partial PlaneIndexTsSelectedButtonViewChild Engine
      // End BIAToolKit Generation Ignore
      // BIAToolKit - End PlaneIndexTsSelectedButtonViewChild
    ];
  }
}
