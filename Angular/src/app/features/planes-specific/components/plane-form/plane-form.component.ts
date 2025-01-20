import {
  Component,
  OnChanges,
  signal,
  SimpleChanges,
  WritableSignal,
} from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { BiaFieldsConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import { CrudHelperService } from 'src/app/shared/bia-shared/services/crud-helper.service';
import { engineCRUDConfiguration } from '../../children/engines/engine.constants';
import { Engine } from '../../model/engine';
import { PlaneSpecific } from '../../model/plane-specific';

@Component({
  selector: 'app-plane-specific-form',
  templateUrl: 'plane-form.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
})
export class PlaneFormComponent
  extends CrudItemFormComponent<PlaneSpecific>
  implements OnChanges
{
  engineCrudConfig: BiaFieldsConfig<Engine> =
    engineCRUDConfiguration.fieldsConfig;
  newId: number = CrudHelperService.newIdStartingValue;
  selectedEngines: Engine[] = [];
  displayedEngines: WritableSignal<Engine[]> = signal([]);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.crudItem) {
      this.setDisplayedEngines();
    }
  }

  setDisplayedEngines() {
    this.displayedEngines.update(() =>
      this.crudItem?.engines
        ? this.crudItem.engines.filter(e => e.dtoState !== DtoState.Deleted)
        : []
    );
  }

  onSelectedEnginesChanged(selectedEngines: Engine[]) {
    this.selectedEngines = selectedEngines;
  }

  onEngineSave(engine: Engine) {
    this.crudItem ??= <PlaneSpecific>{};
    this.crudItem.engines ??= [];
    this.newId = CrudHelperService.onEmbeddedItemSave(
      engine,
      this.crudItem?.engines ?? [],
      this.newId
    );
    this.setDisplayedEngines();
  }

  onDeleteEngines() {
    this.selectedEngines.forEach(e => (e.dtoState = DtoState.Deleted));
    this.setDisplayedEngines();
  }
}
