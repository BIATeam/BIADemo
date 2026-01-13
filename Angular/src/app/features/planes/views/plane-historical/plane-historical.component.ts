import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  CrudItemHistoricalComponent,
  CrudItemHistoricalTimelineComponent,
} from '@bia-team/bia-ng/shared';
import { TranslateModule } from '@ngx-translate/core';
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
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.html',
  styleUrls: [
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.scss',
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
