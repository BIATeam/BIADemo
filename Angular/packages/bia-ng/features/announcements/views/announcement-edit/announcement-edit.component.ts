import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { Announcement } from '@bia-team/bia-ng/models';
import {
  CrudItemEditComponent,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { announcementCRUDConfiguration } from '../../announcement.constants';
import { AnnouncementFormComponent } from '../../components/announcement-form/announcement-form.component';
import { AnnouncementService } from '../../services/announcement.service';

@Component({
  selector: 'bia-announcement-edit',
  templateUrl: './announcement-edit.component.html',
  imports: [AnnouncementFormComponent, AsyncPipe, SpinnerComponent],
})
export class AnnouncementEditComponent
  extends CrudItemEditComponent<Announcement>
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
