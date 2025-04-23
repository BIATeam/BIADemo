import { Component, Injector, OnInit } from '@angular/core';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { engineCRUDConfiguration } from '../../engine.constants';
import { Engine } from '../../model/engine';
import { EngineService } from '../../services/engine.service';
// Begin BIADemo
import { filter } from 'rxjs';
import { FormReadOnlyMode } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
// End BIADemo
import { AsyncPipe, NgIf } from '@angular/common';
import { EngineFormComponent } from '../../components/engine-form/engine-form.component';
import { EngineOptionsService } from '../../services/engine-options.service';

@Component({
  selector: 'app-engine-edit',
  templateUrl: './engine-edit.component.html',
  imports: [NgIf, EngineFormComponent, AsyncPipe, SpinnerComponent],
})
export class EngineEditComponent
  extends CrudItemEditComponent<Engine>
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
  // Begin BIADemo
  protected setPermissions(): void {
    super.setPermissions();

    this.permissionSub.add(
      this.crudItemService.crudItem$
        .pipe(filter(engine => !!engine && Object.keys(engine).length > 0))
        .subscribe(engine => {
          if (
            this.crudConfiguration.isFixable === true &&
            engine.isFixed === true
          ) {
            this.formReadOnlyMode = FormReadOnlyMode.on;
          }
        })
    );
  }
  // End BIADemo
}
