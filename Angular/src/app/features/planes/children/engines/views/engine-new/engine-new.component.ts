import { Component, Injector } from '@angular/core';
import { Engine } from '../../model/engine';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { EngineService } from '../../services/engine.service';
import { EngineCRUDConfiguration } from '../../engine.constants';

@Component({
  selector: 'app-engine-new',
  templateUrl: './engine-new.component.html',
})
export class EngineNewComponent extends CrudItemNewComponent<Engine>  {
   constructor(
    protected injector: Injector,
    public engineService: EngineService,
  ) {
     super(injector, engineService);
     this.crudConfiguration = EngineCRUDConfiguration;
   }
}