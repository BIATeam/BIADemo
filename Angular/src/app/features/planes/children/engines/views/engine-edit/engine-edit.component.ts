import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { engineCRUDConfiguration } from '../../engine.constants';
import { Engine } from '../../model/engine';
import { EngineService } from '../../services/engine.service';
// BIAToolKit - Begin Option
import { EngineOptionsService } from '../../services/engine-options.service';
// BIAToolKit - End Option

@Component({
  selector: 'app-engine-edit',
  templateUrl: './engine-edit.component.html',
})
export class EngineEditComponent
  extends CrudItemEditComponent<Engine>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    // BIAToolKit - Begin Option
    protected engineOptionsService: EngineOptionsService,
    // BIAToolKit - End Option
    public engineService: EngineService
  ) {
    super(injector, engineService);
    this.crudConfiguration = engineCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();
    // BIAToolKit - Begin Option
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.engineOptionsService.loadAllOptions();
      })
    );
    // BIAToolKit - End Option
  }
}
