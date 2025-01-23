import {
  Component,
  HostBinding,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { saveAs } from 'file-saver';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable, Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { TableHelperService } from 'src/app/shared/bia-shared/services/table-helper.service';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';
import { AppState } from 'src/app/store/state';
import { NotificationListItem } from '../../model/notificationListItem';
import { NotificationDas } from '../../services/notification-das.service';
import { NotificationOptionsService } from '../../services/notification-options.service';
import { NotificationsSignalRService } from '../../services/notification-signalr.service';
import {
  getAllNotifications,
  getNotificationLoadingGetAll,
  getNotificationsTotalCount,
} from '../../store/notification.state';
import { FeatureNotificationsActions } from '../../store/notifications-actions';
import { NotificationsEffects } from '../../store/notifications-effects';

@Component({
  selector: 'bia-notifications-index',
  templateUrl: './notifications-index.component.html',
  styleUrls: ['./notifications-index.component.scss'],
})
export class NotificationsIndexComponent implements OnInit, OnDestroy {
  useSignalR = true;
  useView = true;
  useRefreshAtLanguageChange = true;

  @HostBinding('class') classes = 'bia-flex';
  @ViewChild(BiaTableComponent, { static: false })
  notificationListComponent: BiaTableComponent<NotificationListItem>;
  protected sub = new Subscription();
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  notifications$: Observable<NotificationListItem[]>;
  selectedNotifications: NotificationListItem[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canRead = false;
  canDelete = false;
  canAdd = false;
  tableConfiguration: BiaFieldsConfig<NotificationListItem>;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  popupTitle: string;
  tableStateKey = this.useView ? 'notificationsGrid' : undefined;
  parentIds: string[];
  hasColumnFilter = false;

  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    public activatedRoute: ActivatedRoute,
    protected authService: AuthService,
    protected notificationDas: NotificationDas,
    protected translateService: TranslateService,
    protected biaTranslationService: BiaTranslationService,
    protected notificationsSignalRService: NotificationsSignalRService,
    public notificationOptionsService: NotificationOptionsService,
    protected tableHelperService: TableHelperService
  ) {}

  ngOnInit() {
    this.parentIds = [];
    this.sub = new Subscription();

    this.initTableConfiguration();
    this.sub.add(
      this.authService.authInfo$.subscribe((authInfo: AuthInfo) => {
        if (authInfo && authInfo.token !== '') {
          this.setPermissions();
        }
      })
    );
    /*this.notifications$ = this.store.select(getAllNotifications).pipe(map(notifications => notifications.map(notification => {
      notification.title = this.translateService.instant(notification.title);
      notification.description = this.translateService.instant(notification.description);
      return notification;
    })));*/
    this.notifications$ = this.store.select(getAllNotifications);
    this.totalCount$ = this.store.select(getNotificationsTotalCount);
    this.loading$ = this.store.select(getNotificationLoadingGetAll);
    this.onDisplay();

    if (this.useRefreshAtLanguageChange) {
      // Reload data if language change.
      this.sub.add(
        this.biaTranslationService.currentCulture$
          .pipe(skip(1))
          .subscribe(() => {
            this.onLoadLazy(
              this.notificationListComponent.getLazyLoadMetadata()
            );
          })
      );
    }
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
    this.onHide();
  }

  onDisplay() {
    if (this.useView) {
      this.store.dispatch(loadAllView());
    }

    if (this.useSignalR) {
      this.notificationsSignalRService.initialize();
      NotificationsEffects.useSignalR = true;
    }
  }

  onHide() {
    if (this.useSignalR) {
      NotificationsEffects.useSignalR = false;
      this.notificationsSignalRService.destroy();
    }
  }

  onCreate() {
    this.router.navigate(['./create'], { relativeTo: this.activatedRoute });
  }

  onDetail(notificationId: number) {
    this.router.navigate(['./' + notificationId + '/detail'], {
      relativeTo: this.activatedRoute,
    });
  }

  onDelete() {
    if (this.selectedNotifications && this.canDelete) {
      this.store.dispatch(
        FeatureNotificationsActions.multiRemove({
          ids: this.selectedNotifications.map(x => x.id),
        })
      );
    }
  }

  onSelectedElementsChanged(notifications: NotificationListItem[]) {
    this.selectedNotifications = notifications;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: TableLazyLoadEvent) {
    const pagingAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds,
      ...lazyLoadEvent,
    };
    this.store.dispatch(
      FeatureNotificationsActions.loadAllByPost({ event: pagingAndFilter })
    );
    this.hasColumnFilter = this.tableHelperService.hasFilter(
      this.notificationListComponent
    );
  }

  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: KeyValuePair[]) {
    this.displayedColumns = values;
  }

  onClearFilters() {
    const table = this.notificationListComponent.getPrimeNgTable();
    if (table) {
      Object.keys(table.filters).forEach(key =>
        this.tableHelperService.clearFilterMetaData(table.filters[key])
      );
      table.onLazyLoad.emit(table.createLazyLoadMetadata());
    }
  }

  onToggleSearch() {
    this.showColSearch = !this.showColSearch;
  }

  onViewChange(viewPreference: string) {
    this.viewPreference = viewPreference;
  }

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.notificationListComponent
      .getPrimeNgTable()
      ?.columns?.map(
        (x: BiaFieldConfig<NotificationListItem>) =>
          (columns[x.field] = this.translateService.instant(x.header))
      );
    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds,
      columns: columns,
      ...this.notificationListComponent.getLazyLoadMetadata(),
    };
    this.notificationDas.getFile(columnsAndFilter).subscribe(data => {
      saveAs(
        data,
        this.translateService.instant('notification.listOf') + '.csv'
      );
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

  protected initTableConfiguration() {
    this.tableConfiguration = {
      columns: [
        new BiaFieldConfig('titleTranslated', 'notification.title'),
        new BiaFieldConfig('descriptionTranslated', 'notification.description'),
        Object.assign(new BiaFieldConfig('type', 'notification.type.title'), {
          type: PropType.OneToMany,
        }),
        Object.assign(new BiaFieldConfig('read', 'notification.read'), {
          type: PropType.Boolean,
        }),
        Object.assign(
          new BiaFieldConfig('createdDate', 'notification.createdDate'),
          {
            type: PropType.DateTime,
          }
        ),
        Object.assign(
          new BiaFieldConfig('createdBy', 'notification.createdBy'),
          {
            type: PropType.OneToMany,
          }
        ),
        Object.assign(
          new BiaFieldConfig('notifiedUsers', 'notification.notifiedUsers'),
          {
            type: PropType.ManyToMany,
          }
        ),
        Object.assign(
          new BiaFieldConfig('notifiedTeams', 'notification.notifiedTeams'),
          {
            type: PropType.ManyToMany,
          }
        ),
        new BiaFieldConfig('jData', 'notification.jData'),
      ],
    };

    this.columns = this.tableConfiguration.columns.map(
      col => <KeyValuePair>{ key: col.field, value: col.header }
    );
    this.displayedColumns = [...this.columns];
  }
}
