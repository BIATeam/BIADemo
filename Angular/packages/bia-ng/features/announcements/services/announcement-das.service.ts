import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Announcement } from 'packages/bia-ng/models/public-api';
import { announcementFieldsConfiguration } from '../public-api';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementDas extends AbstractDas<Announcement> {
  constructor(injector: Injector) {
    super(injector, 'Announcements', announcementFieldsConfiguration);
  }
}
