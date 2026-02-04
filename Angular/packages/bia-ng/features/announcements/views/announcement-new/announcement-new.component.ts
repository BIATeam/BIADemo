import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { Announcement } from '@bia-team/bia-ng/models';
import { CrudItemNewComponent } from '@bia-team/bia-ng/shared';
import { announcementCRUDConfiguration } from '../../announcement.constants';
import { AnnouncementFormComponent } from '../../components/announcement-form/announcement-form.component';
import { AnnouncementService } from '../../services/announcement.service';

@Component({
  selector: 'bia-announcement-new',
  templateUrl: './announcement-new.component.html',
  imports: [AnnouncementFormComponent, AsyncPipe],
})
export class AnnouncementNewComponent
  extends CrudItemNewComponent<Announcement>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    public announcementService: AnnouncementService
  ) {
    super(injector, announcementService);
    this.crudConfiguration = announcementCRUDConfiguration;
  }
}
