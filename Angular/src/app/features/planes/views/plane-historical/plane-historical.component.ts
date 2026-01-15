import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import {
  CrudItemHistoricalComponent,
  CrudItemHistoricalTimelineComponent,
} from 'packages/bia-ng/shared/public-api';
import { Button } from 'primeng/button';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-historical',
  imports: [
    CrudItemHistoricalTimelineComponent,
    AsyncPipe,
    TranslateModule,
    Button,
  ],
  templateUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.scss',
  ],
})
export class PlaneHistoricalComponent extends CrudItemHistoricalComponent<Plane> {
  constructor(
    protected injector: Injector,
    protected planeService: PlaneService
  ) {
    super(injector, planeService, planeCRUDConfiguration);
  }
}
