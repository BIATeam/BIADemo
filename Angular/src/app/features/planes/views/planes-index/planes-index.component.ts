import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
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
    NgIf,
    ButtonDirective,
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

  // Begin BIADemo
  // BIAToolKit - Begin Partial PlaneIndexTsCanViewChildDeclaration Engine
  canViewEngines = false;
  // BIAToolKit - End Partial PlaneIndexTsCanViewChildDeclaration Engine
  // End BIADemo
  // BIAToolKit - Begin PlaneIndexTsCanViewChildDeclaration
  // BIAToolKit - End PlaneIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public crudItemService: PlaneService,
    protected planeOptionsService: PlaneOptionsService,
    protected authService: AuthService
  ) {
    super(injector, crudItemService);
    this.crudConfiguration = planeCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.planeOptionsService.loadAllOptions();
      })
    );
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
    this.canSave = this.authService.hasPermission(Permission.Plane_Save);
    this.canFix = this.authService.hasPermission(Permission.Plane_Fix);
    this.canSelect = this.canDelete;
    // Begin BIADemo
    // BIAToolKit - Begin Partial PlaneIndexTsCanViewChildSet Engine
    this.canViewEngines = this.authService.hasPermission(
      Permission.Engine_List_Access
    );
    this.canSelect = this.canSelect || this.canViewEngines;
    // BIAToolKit - End Partial PlaneIndexTsCanViewChildSet Engine
    // End BIADemo
    // BIAToolKit - Begin PlaneIndexTsCanViewChildSet
    // BIAToolKit - End PlaneIndexTsCanViewChildSet
  }
  // Begin BIADemo
  // BIAToolKit - Begin Partial PlaneIndexTsOnViewChild Engine
  onViewEngines(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'engines'], {
        relativeTo: this.activatedRoute,
      });
    }
  }
  // BIAToolKit - End Partial PlaneIndexTsOnViewChild Engine
  // End BIADemo

  // BIAToolKit - Begin PlaneIndexTsOnViewChild
  // BIAToolKit - End PlaneIndexTsOnViewChild
}
