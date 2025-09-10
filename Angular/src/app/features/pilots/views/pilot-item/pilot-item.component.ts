import { AsyncPipe, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  CrudItemItemComponent,
  CrudItemService,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { Pilot } from '../../model/pilot';
import { PilotService } from '../../services/pilot.service';

@Component({
  selector: 'app-pilots-item',
  templateUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, AsyncPipe, SpinnerComponent],
  providers: [
    {
      provide: CrudItemService,
      useExisting: PilotService,
    },
  ],
})
export class PilotItemComponent extends CrudItemItemComponent<Pilot> {}
