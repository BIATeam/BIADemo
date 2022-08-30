import { Component, HostBinding, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllPlanes, getPlanesTotalCount, getPlaneLoadingGetAll } from '../../store/plane.state';
import { FeaturePlanesActions } from '../../store/planes-actions';
import { Observable, Subscription } from 'rxjs';
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
import { ActivatedRoute, Router } from '@angular/router';
import { PlaneDas } from '../../services/plane-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { PlanesSignalRService } from '../../services/plane-signalr.service';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { PlaneOptionsService } from '../../services/plane-options.service';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { PlaneTableComponent } from '../../components/plane-table/plane-table.component';
import { useCalcMode, useSignalR, useView, useViewTeamWithTypeId } from '../../plane.constants';
import { skip } from 'rxjs/operators';

@Component({
  selector: 'app-planes-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss']
})
export class PlanesIndexComponent implements OnInit, OnDestroy {
  useCalcMode = useCalcMode;
  useSignalR = useSignalR;
  useView = useView;
  useRefreshAtLanguageChange = false;

  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) biaTableComponent: BiaTableComponent;
  @ViewChild(PlaneTableComponent, { static: false }) planeTableComponent: PlaneTableComponent;
  private get planeListComponent() {
    if (this.biaTableComponent !== undefined) {
      return this.biaTableComponent;
    }
    return this.planeTableComponent;
  }

  private sub = new Subscription();
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
  tableConfiguration: BiaListConfig;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  popupTitle: string;
  tableStateKey = this.useView ? 'planesGrid' : undefined;
  tableState: string;
  sortFieldValue = 'msn';
  useViewTeamWithTypeId = this.useView ? useViewTeamWithTypeId : null;
  parentIds: string[];

  constructor(
    private store: Store<AppState>,
    private router: Router,
    public activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private planeDas: PlaneDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService,
    private planesSignalRService: PlanesSignalRService,
    public planeOptionsService: PlaneOptionsService,
  ) {
  }

  ngOnInit() {
    this.parentIds = [];
    this.sub = new Subscription();

    this.initTableConfiguration();
    this.setPermissions();
    this.planes$ = this.store.select(getAllPlanes);
    this.totalCount$ = this.store.select(getPlanesTotalCount);
    this.loading$ = this.store.select(getPlaneLoadingGetAll);
    this.OnDisplay();
    if (this.useCalcMode) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(event => {
          this.planeOptionsService.loadAllOptions();
        })
      );
    }
    if (this.useRefreshAtLanguageChange) {
      // Reload data if language change.
      this.sub.add(
        this.biaTranslationService.currentCulture$.pipe(skip(1)).subscribe(event => {
          this.onLoadLazy(this.planeListComponent.getLazyLoadMetadata());
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
      this.planesSignalRService.initialize();
    }
  }

  OnHide() {
    if (this.useSignalR) {
      this.planesSignalRService.destroy();
    }
  }

  onCreate() {
    if (!this.useCalcMode) {
      this.router.navigate(['create'], { relativeTo: this.activatedRoute });
    }
  }

  onEdit(planeId: number) {
    if (!this.useCalcMode) {
      this.router.navigate([planeId, 'edit'], { relativeTo: this.activatedRoute });
    }
  }

  onSave(plane: Plane) {
    if (this.useCalcMode) {
      if (plane?.id > 0) {
        if (this.canEdit) {
          this.store.dispatch(FeaturePlanesActions.update({ plane: plane }));
        }
      } else {
        if (this.canAdd) {
          this.store.dispatch(FeaturePlanesActions.create({ plane: plane }));
        }
      }
    }
  }

  onDelete() {
    if (this.selectedPlanes && this.canDelete) {
      this.store.dispatch(FeaturePlanesActions.multiRemove({ ids: this.selectedPlanes.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(planes: Plane[]) {
    this.selectedPlanes = planes;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    const pagingAndFilter: PagingFilterFormatDto = { parentIds: this.parentIds, ...lazyLoadEvent };
    this.store.dispatch(FeaturePlanesActions.loadAllByPost({ event: pagingAndFilter }));
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

  onStateSave(tableState: string) {
    // this.viewPreference = tableState;
    this.tableState = tableState;
  }

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.planeListComponent.getPrimeNgTable().columns.map((x: PrimeTableColumn) => (columns[x.field] = this.translateService.instant(x.header)));
    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds, columns: columns, ...this.planeListComponent.getLazyLoadMetadata()
    };
    this.planeDas.getFile(columnsAndFilter).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.planes') + '.csv');
    });
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }

  private initTableConfiguration() {
    this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          new PrimeTableColumn('msn', 'plane.msn'),
          Object.assign(new PrimeTableColumn('isActive', 'plane.isActive'), {
            isSearchable: false,
            isSortable: false,
            type: PropType.Boolean
          }),
          Object.assign(new PrimeTableColumn('lastFlightDate', 'plane.lastFlightDate'), {
            type: PropType.DateTime,
            formatDate: dateFormat.dateTimeFormat
          }),
          Object.assign(new PrimeTableColumn('deliveryDate', 'plane.deliveryDate'), {
            type: PropType.Date,
            formatDate: dateFormat.dateFormat
          }),
          Object.assign(new PrimeTableColumn('syncTime', 'plane.syncTime'), {
            type: PropType.TimeSecOnly,
            formatDate: dateFormat.timeFormatSec
          }),
          Object.assign(new PrimeTableColumn('capacity', 'plane.capacity'), {
            type: PropType.Number,
            filterMode: PrimeNGFiltering.Equals
          }),
          Object.assign(new PrimeTableColumn('planeType', 'plane.planeType'), {
            type: PropType.OneToMany
          }),
          Object.assign(new PrimeTableColumn('connectingAirports', 'plane.connectingAirports'), {
            type: PropType.ManyToMany
          })
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    }));
  }
}
