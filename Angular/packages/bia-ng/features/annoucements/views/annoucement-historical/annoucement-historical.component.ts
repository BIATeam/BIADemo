import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import {
  CrudItemHistoricalComponent,
  CrudItemHistoricalTimelineComponent,
} from 'packages/bia-ng/shared/public-api';
import { Button } from 'primeng/button';
import { annoucementCRUDConfiguration } from '../../annoucement.constants';
import { Annoucement } from '../../model/annoucement';
import { AnnoucementService } from '../../services/annoucement.service';

@Component({
  selector: 'app-annoucement-historical',
  imports: [
    CrudItemHistoricalTimelineComponent,
    AsyncPipe,
    TranslateModule,
    Button,
  ],
  templateUrl:
    '../../../../shared/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.html',
  styleUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.scss',
})
export class AnnoucementHistoricalComponent extends CrudItemHistoricalComponent<Annoucement> {
  constructor(
    protected injector: Injector,
    protected annoucementService: AnnoucementService
  ) {
    super(injector, annoucementService, annoucementCRUDConfiguration);
  }
}
