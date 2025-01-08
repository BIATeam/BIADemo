import { Component, Injector } from '@angular/core';
import { CrudItemBulkComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-bulk/crud-item-bulk.component';
import { Permission } from 'src/app/shared/permission';
import { engineCRUDConfiguration } from '../../engine.constants';
import { Engine } from '../../model/engine';
import { EngineService } from '../../services/engine.service';

@Component({
  selector: 'app-engine-bulk',
  templateUrl:
    '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-bulk/crud-item-bulk.component.html',
})
export class EngineBulkComponent extends CrudItemBulkComponent<Engine> {
  constructor(
    protected injector: Injector,
    private engineService: EngineService
  ) {
    super(injector, engineService);
    this.crudConfiguration = engineCRUDConfiguration;
    this.setPermissions();
  }

  setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Engine_Update);
    this.canDelete = this.authService.hasPermission(Permission.Engine_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Engine_Create);
  }

  save(toSaves: Engine[]): void {
    this.engineService.save(toSaves);
  }
}
