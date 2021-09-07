import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllNotifications, getNotificationLoadingGet, getNotificationsTotalCount } from '../../store/notification.state';
import { multiRemove, loadAllNotificationsByPost, callWorkerWithNotification, loadNotification } from '../../store/notifications-actions';
import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { Notification } from 'src/app/features/notifications/model/notification';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { BiaListConfig, PrimeTableColumn, PropType } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { TranslateService } from '@ngx-translate/core';
import FileSaver from 'file-saver';
import { NotificationDas } from '../../service/notification-das.service';
import { map } from 'rxjs/operators';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-notifications-index',
  templateUrl: './notifications-index.component.html',
  styleUrls: ['./notifications-index.component.scss']
})
export class NotificationsIndexComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) notificationListComponent: BiaTableComponent;
  showColSearch = false;
  globalSearchValue = '';
  pageSize = DEFAULT_PAGE_SIZE;
  totalRecords: number;

  displayEditNotificationDialog = false;
  displayNewNotificationDialog = false;
  viewPreference: string;

  notifications$: Observable<Notification[]>;
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;

  selectedNotifications: Notification[];

  tableConfiguration: BiaListConfig;

  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];

  lastLazyLoadEvent: LazyLoadEvent;

  constructor(
    private store: Store<AppState>,
    private notificationDas: NotificationDas,
    private translate: TranslateService,
    private biaTranslationService: BiaTranslationService,
    private router: Router) {
  }

  ngOnInit() {
    this.setPermissions();
    this.initTableConfiguration();

    this.notifications$ = this.store.select(getAllNotifications)
      .pipe(map(notifications => notifications.map(notif => {
        if (typeof notif.type !== 'string') {
          notif.type = this.translate.instant(`notification.type.${notif.typeId}`);
        }
        return notif;
      })));

    this.totalCount$ = this.store.select(getNotificationsTotalCount);
    this.loading$ = this.store.select(getNotificationLoadingGet);
  }

  onCreate() {
    this.displayNewNotificationDialog = true;
  }

  onDelete() {
    if (this.selectedNotifications) {
      this.store.dispatch(multiRemove({ ids: this.selectedNotifications.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(planesTypes: Notification[]) {
    this.selectedNotifications = planesTypes;
  }


  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    this.lastLazyLoadEvent = lazyLoadEvent;
    this.store.dispatch(loadAllNotificationsByPost({ event: lazyLoadEvent }));
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

  private setPermissions() {
  }

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.columns.map((x) => (columns[x.value.split('.')[1]] = this.translate.instant(x.value)));
    const customEvent: any = { columns: columns, ...this.notificationListComponent.getLazyLoadMetadata() };
    this.notificationDas.getFile(customEvent).subscribe((data) => {
      FileSaver.saveAs(data, this.translate.instant('app.notifications') + '.csv');
    });
  }

  callWorkerWithNotification() {
    this.store.dispatch(callWorkerWithNotification());
  }

  onEdit(notificationId: number) {
    this.store.dispatch(loadNotification({ id: notificationId }));
    this.router.navigate(['notifications/edit']);
    // this.store.select(getNotificationById(notificationId)).pipe(first()).subscribe(notif => {
    //   if (!notif) {
    //     return;
    //   }

    //   this.store.dispatch(load({ id: notificationId }));

    //   if (notif.targetRoute && notif.targetRoute.length > 0) {
    //     this.router.navigate([notif.targetRoute, notif.targetId]);
    //   } else {
    //     this.router.navigate(['notifications/edit/' + notif.id]);
    //   }
    // });
  }

  private initTableConfiguration() {
    this.biaTranslationService.culture$.subscribe((dateFormat) => {

      this.tableConfiguration = {
        columns: [
          new PrimeTableColumn('title', 'notification.title'),
          new PrimeTableColumn('description', 'notification.description'),
          new PrimeTableColumn('type', 'notification.type.title'),
          Object.assign(new PrimeTableColumn('createdDate', 'notification.createdDate'), {
            type: PropType.Date,
            formatDate: dateFormat.dateTimeFormat,
          }),
          new PrimeTableColumn('read', 'Read'),
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });

      this.displayedColumns = [...this.columns];
    });
  }
}
