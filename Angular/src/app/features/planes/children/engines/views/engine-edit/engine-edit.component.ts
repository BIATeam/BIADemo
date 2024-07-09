import { Component, Injector } from '@angular/core';
import { Engine } from '../../model/engine';
import { engineCRUDConfiguration } from '../../engine.constants';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { EngineService } from '../../services/engine.service';

@Component({
  selector: 'app-engine-edit',
  templateUrl: './engine-edit.component.html',
})
export class EngineEditComponent extends CrudItemEditComponent<Engine> {
  constructor(
    protected injector: Injector,
    public engineService: EngineService
  ) {
    super(injector, engineService);
    this.crudConfiguration = engineCRUDConfiguration;
  }
}
