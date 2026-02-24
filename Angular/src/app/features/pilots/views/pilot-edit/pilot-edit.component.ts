import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  CrudItemEditComponent,
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
  selector: 'app-pilot-edit',
  templateUrl: './pilot-edit.component.html',
  imports: [PilotFormComponent, AsyncPipe, SpinnerComponent],
})
export class PilotEditComponent
  extends CrudItemEditComponent<Pilot>
  implements OnInit
{
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
          this.formReadOnlyMode =
            this.crudConfiguration.isFixable === true && pilot.isFixed === true
              ? FormReadOnlyMode.on
              : FormReadOnlyMode.off;
        })
    );
  }
}
