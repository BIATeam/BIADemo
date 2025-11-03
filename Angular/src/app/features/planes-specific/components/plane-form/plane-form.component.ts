import {
  Component,
  OnChanges,
  signal,
  SimpleChanges,
  ViewChild,
  WritableSignal,
} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { clone } from 'packages/bia-ng/core/public-api';
import { DtoState } from 'packages/bia-ng/models/enum/public-api';
import {
  BiaFieldsConfig,
  KeyValuePair,
} from 'packages/bia-ng/models/public-api';
import {
  BiaFormComponent,
  BiaTableHeaderComponent,
  CrudHelperService,
  CrudItemFormComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { Checkbox } from 'primeng/checkbox';
import { FloatLabel } from 'primeng/floatlabel';
import { InputText } from 'primeng/inputtext';
import { MultiSelect } from 'primeng/multiselect';
import { EngineTableComponent } from '../../children/engines/components/engine-table/engine-table.component';
import { engineCRUDConfiguration } from '../../children/engines/engine.constants';
import { Engine } from '../../model/engine';
import { PlaneSpecific } from '../../model/plane-specific';

@Component({
  selector: 'app-plane-specific-form',
  templateUrl: 'plane-form.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [
    PrimeTemplate,
    FormsModule,
    ReactiveFormsModule,
    Checkbox,
    InputText,
    EngineTableComponent,
    TranslateModule,
    BiaFormComponent,
    BiaTableHeaderComponent,
    FloatLabel,
    MultiSelect,
  ],
})
export class PlaneFormComponent
  extends CrudItemFormComponent<PlaneSpecific>
  implements OnChanges
{
  @ViewChild(EngineTableComponent) engineTableComponent: EngineTableComponent;

  engineCrudConfig: BiaFieldsConfig<Engine> =
    engineCRUDConfiguration.fieldsConfig;
  engineColumnsToDisplay: KeyValuePair[];
  newId: number = CrudHelperService.newIdStartingValue;
  selectedEngines: Engine[] = [];
  displayedEngines: WritableSignal<Engine[]> = signal([]);
  isEditingEngines = false;
  isEngineTableReadOnly = false;

  constructor() {
    super();
    this.engineColumnsToDisplay = this.engineCrudConfig.columns
      .filter(col => col.isVisibleInTable && !col.isHideByDefault)
      .map(col => <KeyValuePair>{ key: col.field, value: col.header });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.crudItem) {
      this.setDisplayedEngines();
    }
  }

  onIsEditingChange(isEditing: boolean) {
    this.isEditingEngines = isEditing;
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
    engine.planeId = this.crudItem?.id;
    this.newId = CrudHelperService.onEmbeddedItemSave(
      engine,
      this.crudItem.engines,
      this.newId
    );
    this.setDisplayedEngines();
    this.engineTableComponent.resetEditableRow();
  }

  onDeleteEngines() {
    this.selectedEngines.forEach(e => (e.dtoState = DtoState.Deleted));
    this.setDisplayedEngines();
  }

  onCloneEngines() {
    this.engineTableComponent.initEditableRow({
      ...clone(this.selectedEngines[0]),
      id: 0,
    });
    this.engineTableComponent.hasChanged = true;
  }

  onReadOnlyChanged(readOnly: boolean): void {
    this.isEngineTableReadOnly = readOnly;
    this.readOnlyChanged.emit(readOnly);
  }

  onSave(crudItem: PlaneSpecific) {
    crudItem.engines = this.crudItem?.engines ?? [];
    super.onSave(crudItem);
  }
}
