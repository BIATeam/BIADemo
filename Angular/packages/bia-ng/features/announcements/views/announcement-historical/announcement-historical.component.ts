import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { Announcement } from '@bia-team/bia-ng/models';
import {
  CrudItemHistoricalComponent,
  CrudItemHistoricalTimelineComponent,
} from '@bia-team/bia-ng/shared';
import { TranslateModule } from '@ngx-translate/core';
import { Button } from 'primeng/button';
import { announcementCRUDConfiguration } from '../../announcement.constants';
import { AnnouncementService } from '../../services/announcement.service';

@Component({
  selector: 'bia-announcement-historical',
  imports: [
    CrudItemHistoricalTimelineComponent,
    AsyncPipe,
    TranslateModule,
    Button,
  ],
  templateUrl:
    '../../../../shared/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.html',
  styleUrls: [
    '../../../../shared/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.scss',
  ],
})
export class AnnouncementHistoricalComponent extends CrudItemHistoricalComponent<Announcement> {
  constructor(
    protected injector: Injector,
    protected announcementService: AnnouncementService
  ) {
    super(injector, announcementService, announcementCRUDConfiguration);
  }
}
