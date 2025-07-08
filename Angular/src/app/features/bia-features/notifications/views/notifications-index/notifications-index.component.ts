import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { skip } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { Permission } from 'src/app/shared/permission';
import { Notification } from '../../model/notification';
import { NotificationListItem } from '../../model/notification-list-item';
import { notificationCRUDConfiguration } from '../../notification.constants';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'bia-notifications-index',
  templateUrl: './notifications-index.component.html',
  styleUrls: ['./notifications-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [
    {
      provide: CrudItemService,
      useExisting: NotificationService,
    },
  ],
})
export class NotificationsIndexComponent
  extends CrudItemsIndexComponent<NotificationListItem, Notification>
  implements OnInit
{
  canRead = false;
  useRefreshAtLanguageChange = true;

  constructor(
    protected injector: Injector,
    public notificationService: NotificationService,
    protected authService: AuthService
  ) {
    super(injector, notificationService);
    this.crudConfiguration = notificationCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();

    if (this.useRefreshAtLanguageChange) {
      // Reload data if language change.
      this.sub.add(
        this.biaTranslationService.currentCulture$
          .pipe(skip(1))
          .subscribe(() => {
            this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata());
          })
      );
    }
  }

  onDetail(notificationId: number) {
    this.router.navigate(['./' + notificationId + '/detail'], {
      relativeTo: this.activatedRoute,
    });
  }

  protected setPermissions() {
    this.canRead = this.authService.hasPermission(Permission.Notification_Read);
    this.canDelete = this.authService.hasPermission(
      Permission.Notification_Delete
    );
    this.canAdd = this.authService.hasPermission(
      Permission.Notification_Create
    );
  }
}
