import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from 'biang/core';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
} from 'biang/shared';
import { PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { filter } from 'rxjs';
import { Permission } from 'src/app/shared/permission';
import { EngineTableComponent } from '../../components/engine-table/engine-table.component';
import { engineCRUDConfiguration } from '../../engine.constants';
import { Engine } from '../../model/engine';
import { EngineOptionsService } from '../../services/engine-options.service';
import { EngineService } from '../../services/engine.service';

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

  // BIAToolKit - Begin EngineIndexTsCanViewChildDeclaration
  // BIAToolKit - End EngineIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public engineService: EngineService,
    protected engineOptionsService: EngineOptionsService,
    protected authService: AuthService
  ) {
    super(injector, engineService);
    this.crudConfiguration = engineCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.parentDisplayItemName$ =
      this.engineService.planeService.displayItemName$;
  }

  protected setPermissions() {
    // BIAToolKit - Begin EngineIndexTsCanViewChildSet
    // BIAToolKit - End EngineIndexTsCanViewChildSet

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

          this.canSelect = this.canDelete;
        })
    );
  }

  // BIAToolKit - Begin EngineIndexTsOnViewChild
  // BIAToolKit - End EngineIndexTsOnViewChild
}
