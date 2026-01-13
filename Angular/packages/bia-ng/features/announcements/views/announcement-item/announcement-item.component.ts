import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Announcement } from '@bia-team/bia-ng/models';
import {
  CrudItemItemComponent,
  CrudItemService,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { AnnouncementService } from '../../services/announcement.service';

@Component({
  selector: 'bia-announcements-item',
  templateUrl:
    '../../../../shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
  providers: [
    {
      provide: CrudItemService,
      useExisting: AnnouncementService,
    },
  ],
})
export class AnnouncementItemComponent extends CrudItemItemComponent<Announcement> {}
