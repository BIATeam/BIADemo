import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  CrudItemItemComponent,
  CrudItemService,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { Engine } from '../../model/engine';
import { EngineService } from '../../services/engine.service';

@Component({
  selector: 'app-engines-item',
  templateUrl:
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
  providers: [
    {
      provide: CrudItemService,
      useExisting: EngineService,
    },
  ],
})
export class EngineItemComponent extends CrudItemItemComponent<Engine> {}
