import { AsyncPipe, NgClass } from '@angular/common';
import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService, BiaPermission } from 'packages/bia-ng/core/public-api';
import { Announcement } from 'packages/bia-ng/models/public-api';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
  SafeHtmlPipe,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { announcementCRUDConfiguration } from '../../announcement.constants';
import { AnnouncementService } from '../../services/announcement.service';

@Component({
  selector: 'bia-announcements-index',
  templateUrl: './announcements-index.component.html',
  styleUrls: ['./announcements-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
    SafeHtmlPipe,
  ],
  providers: [{ provide: CrudItemService, useExisting: AnnouncementService }],
})
export class AnnouncementsIndexComponent
  extends CrudItemsIndexComponent<Announcement>
  implements OnInit, AfterViewInit
{
  // BIAToolKit - Begin AnnouncementIndexTsCanViewChildDeclaration
  // BIAToolKit - End AnnouncementIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public announcementService: AnnouncementService,
    protected authService: AuthService
  ) {
    super(injector, announcementService);
    this.crudConfiguration = announcementCRUDConfiguration;
  }

  ngAfterViewInit(): void {
    this.multiSortMeta = [{ field: 'start', order: -1 }];
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(
      BiaPermission.Announcement_Update
    );
    this.canDelete = this.authService.hasPermission(
      BiaPermission.Announcement_Delete
    );
    this.canAdd = this.authService.hasPermission(
      BiaPermission.Announcement_Create
    );
    this.canSelect = this.canDelete;
    // BIAToolKit - Begin AnnouncementIndexTsCanViewChildSet
    // BIAToolKit - End AnnouncementIndexTsCanViewChildSet
  }

  // BIAToolKit - Begin AnnouncementIndexTsOnViewChild
  // BIAToolKit - End AnnouncementIndexTsOnViewChild

  protected initSelectedButtonGroup() {
    this.selectionActionsMenuItems = [
      // BIAToolKit - Begin AnnouncementIndexTsSelectedButtonViewChild
      // BIAToolKit - End AnnouncementIndexTsSelectedButtonViewChild
    ];
  }
}
