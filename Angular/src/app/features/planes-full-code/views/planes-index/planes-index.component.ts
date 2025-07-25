import { AsyncPipe, NgIf } from '@angular/common';
import {
  Component,
  HostBinding,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { saveAs } from 'file-saver';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable, Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PrimeNGFiltering,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { TableHelperService } from 'src/app/shared/bia-shared/services/table-helper.service';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';
import { AppState } from 'src/app/store/state';
import { PlaneTableComponent } from '../../components/plane-table/plane-table.component';
import { Plane } from '../../model/plane';
import {
  useCalcMode,
  useSignalR,
  useView,
  useViewTeamWithTypeId,
} from '../../plane.constants';
import { PlaneDas } from '../../services/plane-das.service';
import { PlaneOptionsService } from '../../services/plane-options.service';
import { PlanesSignalRService } from '../../services/plane-signalr.service';
import {
  getAllPlanes,
  getPlaneLoadingGetAll,
  getPlanesTotalCount,
} from '../../store/plane.state';
import { FeaturePlanesActions } from '../../store/planes-actions';

@Component({
  selector: 'app-planes-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss'],
  imports: [
    NgIf,
    PlaneTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableComponent,
  ],
})
export class PlanesIndexComponent implements OnInit, OnDestroy {
  useCalcMode = useCalcMode;
  useSignalR = useSignalR;
  useView = useView;
  useRefreshAtLanguageChange = false;
  hasColumnFilter = false;

  @HostBinding('class') classes = 'bia-flex';
  @ViewChild(BiaTableComponent, { static: false })
  biaTableComponent: BiaTableComponent<Plane>;
  @ViewChild(PlaneTableComponent, { static: false })
  planeTableComponent: PlaneTableComponent;
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
  tableConfiguration: BiaFieldsConfig<Plane>;
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
    private tableHelperService: TableHelperService
  ) {}

  ngOnInit() {
    this.parentIds = [];
    this.sub = new Subscription();

    this.initTableConfiguration();
    this.sub.add(
      this.authService.authInfo$.subscribe(() => {
        this.setPermissions();
      })
    );
    this.planes$ = this.store.select(getAllPlanes);
    this.totalCount$ = this.store.select(getPlanesTotalCount);
    this.loading$ = this.store.select(getPlaneLoadingGetAll);
    this.onDisplay();
    if (this.useCalcMode) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(() => {
          this.planeOptionsService.loadAllOptions();
        })
      );
    }
    if (this.useRefreshAtLanguageChange) {
      // Reload data if language change.
      this.sub.add(
        this.biaTranslationService.currentCulture$
          .pipe(skip(1))
          .subscribe(() => {
            this.onLoadLazy(this.planeListComponent.getLazyLoadMetadata());
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
      this.planesSignalRService.initialize();
    }
  }

  onHide() {
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
      this.router.navigate([planeId, 'edit'], {
        relativeTo: this.activatedRoute,
      });
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
      this.store.dispatch(
        FeaturePlanesActions.multiRemove({
          ids: this.selectedPlanes.map(x => x.id),
        })
      );
    }
  }

  onSelectedElementsChanged(planes: Plane[]) {
    this.selectedPlanes = planes;
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
      FeaturePlanesActions.loadAllByPost({ event: pagingAndFilter })
    );
    this.hasColumnFilter =
      this.tableHelperService.hasFilter(this.biaTableComponent) ||
      this.tableHelperService.hasFilter(this.planeTableComponent);
  }

  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: KeyValuePair[]) {
    this.displayedColumns = values;
  }

  onClearFilters() {
    const table = this.planeListComponent.getPrimeNgTable();
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

  onStateSave(tableState: string) {
    this.viewPreference = tableState;
    this.tableState = tableState;
  }

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.planeListComponent
      .getPrimeNgTable()
      ?.columns?.map(
        (x: BiaFieldConfig<Plane>) =>
          (columns[x.field] = this.translateService.instant(x.header))
      );
    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds,
      columns: columns,
      ...this.planeListComponent.getLazyLoadMetadata(),
    };
    this.planeDas.getFile(columnsAndFilter).subscribe(data => {
      saveAs(data, this.translateService.instant('plane.listOf') + '.csv');
    });
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }

  private initTableConfiguration() {
    //this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
    this.tableConfiguration = {
      columns: [
        Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
          isRequired: true,
        }),
        Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
          isSearchable: false,
          isSortable: false,
          type: PropType.Boolean,
        }),
        Object.assign(
          new BiaFieldConfig('lastFlightDate', 'plane.lastFlightDate'),
          {
            type: PropType.DateTime,
            //formatDate: dateFormat.dateTimeFormat
          }
        ),
        Object.assign(
          new BiaFieldConfig('deliveryDate', 'plane.deliveryDate'),
          {
            type: PropType.Date,
            //formatDate: dateFormat.dateFormat
          }
        ),
        Object.assign(new BiaFieldConfig('syncTime', 'plane.syncTime'), {
          type: PropType.TimeSecOnly,
          //formatDate: dateFormat.timeFormatSec
        }),
        Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
          type: PropType.Number,
          filterMode: PrimeNGFiltering.Equals,
          isRequired: true,
        }),
        Object.assign(new BiaFieldConfig('planeType', 'plane.planeType'), {
          type: PropType.OneToMany,
        }),
        Object.assign(
          new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'),
          {
            type: PropType.ManyToMany,
          }
        ),
      ],
    };

    this.columns = this.tableConfiguration.columns.map(
      col => <KeyValuePair>{ key: col.field, value: col.header }
    );
    this.displayedColumns = [...this.columns];
    //}));
  }
}
