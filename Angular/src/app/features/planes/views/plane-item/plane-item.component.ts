import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  CrudItemItemComponent,
  CrudItemService,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { Plane } from '../../model/plane';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-planes-item',
  templateUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
  providers: [
    {
      provide: CrudItemService,
      useExisting: PlaneService,
    },
  ],
})
export class PlaneItemComponent extends CrudItemItemComponent<Plane> {}
