import { Component } from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { BiaFieldsConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import { engineCRUDConfiguration } from '../../children/engines/engine.constants';
import { Engine } from '../../model/engine';
import { PlaneSpecific } from '../../model/plane-specific';

@Component({
  selector: 'app-plane-form',
  templateUrl: 'plane-form.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
})
export class PlaneFormComponent extends CrudItemFormComponent<PlaneSpecific> {
  engineCrudConfig: BiaFieldsConfig = engineCRUDConfiguration.fieldsConfig;
  newId: number = -1;
  selectedEngines: Engine[] = [];

  get displayedEngines(): Engine[] {
    return this.crudItem.engines
      ? this.crudItem.engines.filter(e => e.dtoState !== DtoState.Deleted)
      : [];
  }

  onSelectedEnginesChanged(selectedEngines: Engine[]) {
    this.selectedEngines = selectedEngines;
  }

  onEngineSave(engine: Engine) {
    if (engine.id !== 0) {
      engine.dtoState = DtoState.Modified;
      this.crudItem.engines[
        this.crudItem.engines.findIndex(oldEngine => engine.id == oldEngine.id)
      ] = engine;
    } else {
      engine.id = this.newId;
      this.newId--;
      this.crudItem.engines = this.crudItem.engines
        ? [...this.crudItem.engines, engine]
        : [engine];
    }
  }

  onDeleteEngines() {
    this.selectedEngines.forEach(e => (e.dtoState = DtoState.Deleted));
  }
}
