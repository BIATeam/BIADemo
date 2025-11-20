import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import {
  CrudItemHistoricalComponent,
  CrudItemHistoricalTimelineComponent,
} from 'packages/bia-ng/shared/public-api';
import { Button } from 'primeng/button';
import { announcementCRUDConfiguration } from '../../announcement.constants';
import { Announcement } from '../../model/announcement';
import { AnnouncementService } from '../../services/announcement.service';

@Component({
  selector: 'app-announcement-historical',
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
export class AnnouncementHistoricalComponent extends CrudItemHistoricalComponent<Announcement> {
  constructor(
    protected injector: Injector,
    protected announcementService: AnnouncementService
  ) {
    super(injector, announcementService, announcementCRUDConfiguration);
  }
}
