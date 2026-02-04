import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  CrudItemEditComponent,
  FormReadOnlyMode,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { filter } from 'rxjs';
import { EngineFormComponent } from '../../components/engine-form/engine-form.component';
import { engineCRUDConfiguration } from '../../engine.constants';
import { Engine } from '../../model/engine';
import { EngineOptionsService } from '../../services/engine-options.service';
import { EngineService } from '../../services/engine.service';

@Component({
  selector: 'app-engine-edit',
  templateUrl: './engine-edit.component.html',
  imports: [EngineFormComponent, AsyncPipe, SpinnerComponent],
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
}
