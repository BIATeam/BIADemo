import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { Announcement } from '@bia-team/bia-ng/models';
import { announcementFieldsConfiguration } from '../public-api';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementDas extends AbstractDas<Announcement> {
  constructor(injector: Injector) {
    super(injector, 'Announcements', announcementFieldsConfiguration);
  }
}
