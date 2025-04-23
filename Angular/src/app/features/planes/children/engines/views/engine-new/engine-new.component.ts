import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { EngineFormComponent } from '../../components/engine-form/engine-form.component';
import { engineCRUDConfiguration } from '../../engine.constants';
import { Engine } from '../../model/engine';
import { EngineOptionsService } from '../../services/engine-options.service';
import { EngineService } from '../../services/engine.service';

@Component({
  selector: 'app-engine-new',
  templateUrl: './engine-new.component.html',
  imports: [EngineFormComponent, AsyncPipe],
})
export class EngineNewComponent
  extends CrudItemNewComponent<Engine>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    protected engineOptionsService: EngineOptionsService,
    public engineService: EngineService
  ) {
    super(injector, engineService);
    this.crudConfiguration = engineCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.engineOptionsService.loadAllOptions();
      })
    );
  }
}
