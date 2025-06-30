import { NgIf, NgSwitch, NgSwitchCase } from '@angular/common';
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
import { PrimeTemplate } from 'primeng/api';
import { Checkbox } from 'primeng/checkbox';
import { FloatLabel } from 'primeng/floatlabel';
import { InputText } from 'primeng/inputtext';
import { MultiSelect } from 'primeng/multiselect';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { BiaFieldsConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { CrudHelperService } from 'src/app/shared/bia-shared/services/crud-helper.service';
import { clone } from 'src/app/shared/bia-shared/utils';
import { EngineTableComponent } from '../../children/engines/components/engine-table/engine-table.component';
import { engineCRUDConfiguration } from '../../children/engines/engine.constants';
import { Engine } from '../../model/engine';
import { PlaneSpecific } from '../../model/plane-specific';

@Component({
  selector: 'app-plane-specific-form',
  templateUrl: 'plane-form.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [
    PrimeTemplate,
    FormsModule,
    ReactiveFormsModule,
    NgSwitch,
    NgSwitchCase,
    Checkbox,
    InputText,
    NgIf,
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
      .filter(col => !col.isHideByDefault)
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
