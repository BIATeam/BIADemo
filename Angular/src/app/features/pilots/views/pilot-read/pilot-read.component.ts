import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  CrudItemReadComponent,
  FormReadOnlyMode,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { filter } from 'rxjs';
import { Permission } from 'src/app/shared/permission';
import { PilotFormComponent } from '../../components/pilot-form/pilot-form.component';
import { Pilot } from '../../model/pilot';
import { pilotCRUDConfiguration } from '../../pilot.constants';
import { PilotService } from '../../services/pilot.service';

@Component({
  selector: 'app-pilot-read',
  templateUrl: './pilot-read.component.html',
  imports: [PilotFormComponent, AsyncPipe, SpinnerComponent],
})
export class PilotReadComponent extends CrudItemReadComponent<Pilot> {
  constructor(
    protected injector: Injector,
    public pilotService: PilotService
  ) {
    super(injector, pilotService);
    this.crudConfiguration = pilotCRUDConfiguration;
  }

  protected setPermissions(): void {
    super.setPermissions();
    this.canFix = this.authService.hasPermission(Permission.Pilot_Fix);
    this.permissionSub.add(
      this.crudItemService.crudItem$
        .pipe(filter(pilot => !!pilot && Object.keys(pilot).length > 0))
        .subscribe(pilot => {
          this.canEdit =
            this.crudConfiguration.isFixable === true && pilot.isFixed === true
              ? false
              : this.authService.hasPermission(Permission.Pilot_Update);

          this.formReadOnlyMode =
            this.canEdit === false &&
            this.crudConfiguration.isFixable === true &&
            pilot.isFixed === true
              ? FormReadOnlyMode.on
              : this.initialFormReadOnlyMode;
        })
    );
  }
}
