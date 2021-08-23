import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllNotifications, getNotificationsTotalCount } from '../../store/notification.state';
import { multiRemove, loadAllByPost, callWorkerWithNotification } from '../../store/notifications-actions';
import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { Notification } from 'src/app/features/notifications/model/notification';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { BiaListConfig, PrimeTableColumn } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { TranslateService } from '@ngx-translate/core';
import FileSaver from 'file-saver';
import { NotificationDas } from '../../service/notification-das.service';

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
  notifications$: Observable<Notification[]>;
  selectedNotifications: Notification[];
  totalCount$: Observable<number>;
  displayEditNotificationDialog = false;
  displayNewNotificationDialog = false;
  viewPreference: string;

  tableConfiguration: BiaListConfig = {
    columns: [
      new PrimeTableColumn('title', 'notification.title'),
      new PrimeTableColumn('description', 'notification.description')
    ]
  };

  columns: KeyValuePair[] = this.tableConfiguration.columns.map(
    (col) => <KeyValuePair>{ key: col.field, value: col.header }
  );
  displayedColumns: KeyValuePair[] = this.columns;
  lastLazyLoadEvent: LazyLoadEvent;

  constructor(
    private store: Store<AppState>,
    private notificationDas: NotificationDas,
    private translate: TranslateService) { }

  ngOnInit() {
    this.setPermissions();
    this.notifications$ = this.store.select(getAllNotifications);
    this.totalCount$ = this.store.select(getNotificationsTotalCount).pipe();
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
    this.store.dispatch(loadAllByPost({ event: lazyLoadEvent }));
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
}
