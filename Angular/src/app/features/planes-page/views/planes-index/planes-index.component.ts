import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllPlanes, getPlanesTotalCount, getPlaneLoadingGetAll } from '../../store/plane.state';
import { multiRemove, loadAllByPost } from '../../store/planes-actions';
import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { Plane } from '../../model/plane';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  PrimeTableColumn,
  PropType,
  PrimeNGFiltering
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Router } from '@angular/router';
import { PlaneDas } from '../../services/plane-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';

@Component({
  selector: 'app-planes-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss']
})
export class PlanesIndexComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) planeListComponent: BiaTableComponent;
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  planes$: Observable<Plane[]>;
  selectedPlanes: Plane[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  createPlaneRoute = '/examples/planes-page/create';
  indexPlaneRoute = '/examples/planes-page/';
  tableConfiguration: BiaListConfig;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private authService: AuthService,
    private planeDas: PlaneDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService
  ) {}

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();
    this.planes$ = this.store.select(getAllPlanes).pipe();
    this.totalCount$ = this.store.select(getPlanesTotalCount).pipe();
    this.loading$ = this.store.select(getPlaneLoadingGetAll).pipe();
  }

  onCreate() {
    this.router.navigate([this.createPlaneRoute]);
  }

  onEdit(planeId: number) {
    this.router.navigate([this.indexPlaneRoute + planeId + '/edit']);
  }

  onDelete() {
    if (this.selectedPlanes) {
      this.store.dispatch(multiRemove({ ids: this.selectedPlanes.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(planes: Plane[]) {
    this.selectedPlanes = planes;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
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

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.columns.map((x) => (columns[x.value.split('.')[1]] = this.translateService.instant(x.value)));
    const customEvent: any = { columns: columns, ...this.planeListComponent.getLazyLoadMetadata() };
    this.planeDas.getFile(customEvent).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.planes') + '.csv');
    });
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }

  private initTableConfiguration() {
    this.biaTranslationService.culture$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          new PrimeTableColumn('msn', 'plane.msn'),
          Object.assign(new PrimeTableColumn('isActive', 'plane.isActive'), {
            isSearchable: false,
            isSortable: false,
            type: PropType.Boolean
          }),
          Object.assign(new PrimeTableColumn('firstFlightDate', 'plane.firstFlightDate'), {
            type: PropType.Date,
            formatDate: dateFormat.dateFormat
          }),
          Object.assign(new PrimeTableColumn('firstFlightTime', 'plane.firstFlightTime'), {
            isSearchable: false,
            isSortable: false,
            type: PropType.Date,
            formatDate: dateFormat.timeFormat
          }),
          Object.assign(new PrimeTableColumn('lastFlightDate', 'plane.lastFlightDate'), {
            type: PropType.Date,
            formatDate: dateFormat.dateTimeFormat
          }),
          Object.assign(new PrimeTableColumn('capacity', 'plane.capacity'), {
            type: PropType.Number,
            filterMode: PrimeNGFiltering.Equals
          })
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    });
  }
}
