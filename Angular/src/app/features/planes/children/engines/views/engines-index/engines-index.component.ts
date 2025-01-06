import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { Permission } from 'src/app/shared/permission';
import { EngineTableComponent } from '../../components/engine-table/engine-table.component';
import { engineCRUDConfiguration } from '../../engine.constants';
import { Engine } from '../../model/engine';
import { EngineService } from '../../services/engine.service';
// BIAToolKit - Begin Option
import { EngineOptionsService } from '../../services/engine-options.service';
// BIAToolKit - End Option

@Component({
  selector: 'app-engines-index',
  templateUrl: './engines-index.component.html',
  styleUrls: ['./engines-index.component.scss'],
})
export class EnginesIndexComponent
  extends CrudItemsIndexComponent<Engine>
  implements OnInit
{
  @ViewChild(EngineTableComponent, { static: false })
  crudItemTableComponent: EngineTableComponent;

  constructor(
    protected injector: Injector,
    public engineService: EngineService,
    // BIAToolKit - Begin Option
    protected engineOptionsService: EngineOptionsService,
    // BIAToolKit - End Option
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
    // BIAToolKit - Begin Option
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.engineOptionsService.loadAllOptions();
      })
    );
    // BIAToolKit - End Option
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Engine_Update);
    this.canDelete = this.authService.hasPermission(Permission.Engine_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Engine_Create);
  }
}
