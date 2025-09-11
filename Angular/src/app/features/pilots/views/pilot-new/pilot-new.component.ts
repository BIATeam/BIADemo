import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemNewComponent } from 'packages/bia-ng/shared/public-api';
import { of } from 'rxjs';
import { PilotFormComponent } from '../../components/pilot-form/pilot-form.component';
import { Pilot } from '../../model/pilot';
import { pilotCRUDConfiguration } from '../../pilot.constants';
import { PilotService } from '../../services/pilot.service';

@Component({
  selector: 'app-pilot-new',
  templateUrl: './pilot-new.component.html',
  imports: [PilotFormComponent, AsyncPipe],
})
export class PilotNewComponent
  extends CrudItemNewComponent<Pilot>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    public pilotService: PilotService
  ) {
    super(injector, pilotService);
    if (!this.itemTemplate$) {
      this.itemTemplate$ = of(<Pilot>{ id: '' });
    }
    this.crudConfiguration = pilotCRUDConfiguration;
  }
}
