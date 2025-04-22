import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
// Begin BIADemo
import { Permission } from 'src/app/shared/permission';
// End BIADemo
import { EngineTableComponent } from '../../components/engine-table/engine-table.component';
import { engineCRUDConfiguration } from '../../engine.constants';
import { Engine } from '../../model/engine';
import { EngineService } from '../../services/engine.service';
// Begin BIADemo
import { filter } from 'rxjs';
// End BIADemo
import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { EngineOptionsService } from '../../services/engine-options.service';

@Component({
  selector: 'app-engines-index',
  templateUrl: './engines-index.component.html',
  styleUrls: ['./engines-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    NgIf,
    ButtonDirective,
    EngineTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [{ provide: CrudItemService, useExisting: EngineService }],
})
export class EnginesIndexComponent
  extends CrudItemsIndexComponent<Engine>
  implements OnInit
{
  @ViewChild(EngineTableComponent, { static: false })
  crudItemTableComponent: EngineTableComponent;
  // BIAToolKit - Begin MaintenanceContractIndexTsCanViewChildDeclaration
  // BIAToolKit - End MaintenanceContractIndexTsCanViewChildDeclaration

  // Begin BIADemo
  isParentFixed = false;
  // End BIADemo

  constructor(
    protected injector: Injector,
    public engineService: EngineService,
    protected engineOptionsService: EngineOptionsService,
    protected authService: AuthService
  ) {
    super(injector, engineService);
    this.crudConfiguration = engineCRUDConfiguration;
    this.reorderableColumns = false;
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.parentDisplayItemName$ =
      this.engineService.planeService.displayItemName$;
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.engineOptionsService.loadAllOptions();
      })
    );
  }
  protected async setPermissions() {
    // BIAToolKit - Begin MaintenanceContractIndexTsCanViewChildSet
    // BIAToolKit - End MaintenanceContractIndexTsCanViewChildSet

    // Begin BIADemo
    super.setPermissions();

    this.permissionSub.add(
      this.engineService.planeService.crudItem$
        .pipe(filter(plane => !!plane && Object.keys(plane).length > 0))
        .subscribe(plane => {
          this.isParentFixed = plane.isFixed === true;

          this.canEdit =
            plane.isFixed === false &&
            this.authService.hasPermission(Permission.Engine_Update);
          this.canDelete =
            plane.isFixed === false &&
            this.authService.hasPermission(Permission.Engine_Delete);
          this.canAdd =
            plane.isFixed === false &&
            this.authService.hasPermission(Permission.Engine_Create);
          this.canSave =
            plane.isFixed === false &&
            this.authService.hasPermission(Permission.Engine_Save);
        })
    );
    // End BIADemo
  }
}
