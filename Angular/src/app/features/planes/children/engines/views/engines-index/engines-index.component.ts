import { Component, Injector, ViewChild } from '@angular/core';
import { Engine } from '../../model/engine';
import { EngineCRUDConfiguration } from '../../engine.constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { EngineService } from '../../services/engine.service';
import { EngineTableComponent } from '../../components/engine-table/engine-table.component';

@Component({
  selector: 'app-engines-index',
  templateUrl: './engines-index.component.html',
  styleUrls: ['./engines-index.component.scss']
})

export class EnginesIndexComponent extends CrudItemsIndexComponent<Engine> {
  
  @ViewChild(EngineTableComponent, { static: false }) crudItemTableComponent: EngineTableComponent;

  constructor(
    protected injector: Injector,
    public engineService: EngineService,
    protected authService: AuthService,
  ) {
    super(injector, engineService);
    this.crudConfiguration = EngineCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Engine_Update);
    this.canDelete = this.authService.hasPermission(Permission.Engine_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Engine_Create);
  }
}
