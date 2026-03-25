import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  BiaFormComponent,
  CrudItemImportComponent,
  CrudItemImportFormComponent,
  CrudItemImportService,
  SAME_LIST_FORM_MODELS,
} from '@bia-team/bia-ng/shared';
import { Permission } from 'src/app/shared/permission';
import { Pilot } from '../../model/pilot';
import { PilotList } from '../../model/pilot-list';
import { pilotCRUDConfiguration } from '../../pilot.constants';
import { PilotService } from '../../services/pilot.service';

@Component({
  selector: 'app-pilot-import',
  templateUrl:
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-import/crud-item-import.component.html',
  imports: [CrudItemImportFormComponent, AsyncPipe, BiaFormComponent],
  providers: [
    CrudItemImportService,
    { provide: SAME_LIST_FORM_MODELS, useValue: false },
  ],
})
export class PilotImportComponent extends CrudItemImportComponent<
  PilotList,
  Pilot
> {
  constructor(
    protected injector: Injector,
    private pilotService: PilotService
  ) {
    super(injector, pilotService);
    this.crudConfiguration = pilotCRUDConfiguration;
    this.setPermissions();
  }

  setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Pilot_Update);
    this.canDelete = this.authService.hasPermission(Permission.Pilot_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Pilot_Create);
  }

  save(toSaves: Pilot[]): void {
    this.pilotService.save(toSaves);
  }
}
