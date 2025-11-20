import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  CrudItemItemComponent,
  CrudItemService,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { Announcement } from '../../model/announcement';
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
