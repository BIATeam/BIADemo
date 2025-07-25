import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { CrudItemImportFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-import-form/crud-item-import-form.component';
import { CrudItemImportComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component';
import { Permission } from 'src/app/shared/permission';
import { engineCRUDConfiguration } from '../../engine.constants';
import { Engine } from '../../model/engine';
import { EngineService } from '../../services/engine.service';

@Component({
  selector: 'app-engine-import',
  templateUrl:
    '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component.html',
  imports: [CrudItemImportFormComponent, AsyncPipe, BiaFormComponent],
})
export class EngineImportComponent extends CrudItemImportComponent<Engine> {
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
