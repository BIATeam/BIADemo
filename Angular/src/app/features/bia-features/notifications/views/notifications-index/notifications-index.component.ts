import { Component, HostBinding, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllNotifications, getNotificationsTotalCount, getNotificationLoadingGetAll, getNotificationById } from '../../store/notification.state';
import { FeatureNotificationsActions } from '../../store/notifications-actions';
import { Observable, Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { NotificationListItem } from '../../model/notificationListItem';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  PrimeTableColumn,
  PropType,
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationDas } from '../../services/notification-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { NotificationsSignalRService } from '../../services/notification-signalr.service';
import { NotificationsEffects } from '../../store/notifications-effects';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { NotificationOptionsService } from '../../services/notification-options.service';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { first, skip } from 'rxjs/operators';

@Component({
  selector: 'bia-notifications-index',
  templateUrl: './notifications-index.component.html',
  styleUrls: ['./notifications-index.component.scss']
})
export class NotificationsIndexComponent implements OnInit, OnDestroy {
  useSignalR = false;
  useView = true;
  useRefreshAtLanguageChange = true;

  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) notificationListComponent: BiaTableComponent;
  private sub = new Subscription();
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  notifications$: Observable<NotificationListItem[]>;
  selectedNotifications: NotificationListItem[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  tableConfiguration: BiaListConfig;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  popupTitle: string;
  tableStateKey = this.useView ? 'notificationsGrid' : undefined;
  parentIds: string[];


  constructor(
    private store: Store<AppState>,
    private router: Router,
    public activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private notificationDas: NotificationDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService,
    private notificationsSignalRService: NotificationsSignalRService,
    public notificationOptionsService: NotificationOptionsService,
  ) {
  }

  ngOnInit() {
    this.parentIds = [];
    this.sub = new Subscription();

    this.initTableConfiguration();
    this.setPermissions();
    /*this.notifications$ = this.store.select(getAllNotifications).pipe(map(notifications => notifications.map(notification => {
      notification.title = this.translateService.instant(notification.title);
      notification.description = this.translateService.instant(notification.description);
      return notification;
    })));*/
    this.notifications$ = this.store.select(getAllNotifications);
    this.totalCount$ = this.store.select(getNotificationsTotalCount);
    this.loading$ = this.store.select(getNotificationLoadingGetAll);
    this.OnDisplay();

    if (this.useRefreshAtLanguageChange) {
      // Reload data if language change.
      this.sub.add(
        this.biaTranslationService.currentCulture$.pipe(skip(1)).subscribe(event => {
          this.onLoadLazy(this.notificationListComponent.getLazyLoadMetadata());
        })
      );
    }
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
    this.OnHide();
  }

  OnDisplay() {
    if (this.useView) {
      this.store.dispatch(loadAllView());
    }


    if (this.useSignalR) {
      this.notificationsSignalRService.initialize();
      NotificationsEffects.useSignalR = true;
    }
  }

  OnHide() {
    if (this.useSignalR) {
      NotificationsEffects.useSignalR = false;
      this.notificationsSignalRService.destroy();
    }
  }

  onCreate() {
    this.router.navigate(['./create'], { relativeTo: this.activatedRoute });
  }

  onEdit(notificationId: number) {
    this.router.navigate(['./' + notificationId + '/edit'], { relativeTo: this.activatedRoute });
  }

  onDetail(notificationId: number) {
    this.router.navigate(['./' + notificationId + '/detail'], { relativeTo: this.activatedRoute });

    this.store.select(getNotificationById(notificationId)).pipe(first()).subscribe(notif => {
      if (notif && !notif.read) {
        // refresh the list to see readed
        setTimeout(() => this.onLoadLazy(this.notificationListComponent.getLazyLoadMetadata()), 500)
      }
    }
    )
  }

  onDelete() {
    if (this.selectedNotifications && this.canDelete) {
      this.store.dispatch(FeatureNotificationsActions.multiRemove({ ids: this.selectedNotifications.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(notifications: NotificationListItem[]) {
    this.selectedNotifications = notifications;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    const pagingAndFilter: PagingFilterFormatDto = { parentIds: this.parentIds, ...lazyLoadEvent };
    this.store.dispatch(FeatureNotificationsActions.loadAllByPost({ event: pagingAndFilter }));
  }

  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: KeyValuePair[]) {
    this.displayedColumns = values;
  }

  onToggleSearch() {
    this.showColSearch = !this.showColSearch;
  }

  onViewChange(viewPreference: string) {
    this.viewPreference = viewPreference;
  }

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.notificationListComponent.getPrimeNgTable().columns.map((x: PrimeTableColumn) => (columns[x.field] = this.translateService.instant(x.header)));
    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds, columns: columns, ...this.notificationListComponent.getLazyLoadMetadata()
    };
    this.notificationDas.getFile(columnsAndFilter).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.notifications') + '.csv');
    });
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Notification_Update);
    this.canDelete = this.authService.hasPermission(Permission.Notification_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Notification_Create);
  }

  private initTableConfiguration() {
    this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          new PrimeTableColumn('titleTranslated', 'notification.title'),
          new PrimeTableColumn('descriptionTranslated', 'notification.description'),
          Object.assign(new PrimeTableColumn('type', 'notification.type.title'), {
            type: PropType.OneToMany,
          }),
          Object.assign(new PrimeTableColumn('read', 'notification.read'), {
            isSearchable: false,
            type: PropType.Boolean
          }),
          Object.assign(new PrimeTableColumn('createdDate', 'notification.createdDate'), {
            type: PropType.Date,
            formatDate: dateFormat.dateFormat
          }),
          Object.assign(new PrimeTableColumn('createdBy', 'notification.createdBy'), {
            type: PropType.OneToMany
          }),
          Object.assign(new PrimeTableColumn('notifiedUsers', 'notification.notifiedUsers'), {
            type: PropType.ManyToMany
          }),
          Object.assign(new PrimeTableColumn('notifiedTeams', 'notification.notifiedTeams'), {
            type: PropType.ManyToMany
          }),
          new PrimeTableColumn('jData', 'notification.jData'),
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    }));
  }
}
