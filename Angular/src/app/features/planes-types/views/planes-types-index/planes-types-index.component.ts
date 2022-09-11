import { Component, HostBinding, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllPlanesTypes, getPlanesTypesTotalCount, getPlaneTypeLoadingGetAll } from '../../store/plane-type.state';
import {
  multiRemove,
  loadAllByPost,
  load,
  openDialogEdit,
  openDialogNew
} from '../../store/planes-types-actions';
import { Observable, Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { PlaneType } from '../../model/plane-type';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaFieldsConfig,
  BiaFieldConfig,
  PropType
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { PlaneTypeDas } from '../../services/plane-type-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';

@Component({
  selector: 'app-planes-types-index',
  templateUrl: './planes-types-index.component.html',
  styleUrls: ['./planes-types-index.component.scss']
})
export class PlanesTypesIndexComponent implements OnInit, OnDestroy {
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) planeTypeListComponent: BiaTableComponent;
  private sub = new Subscription();
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  planesTypes$: Observable<PlaneType[]>;
  selectedPlanesTypes: PlaneType[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  tableConfiguration: BiaFieldsConfig;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  
  constructor(
    private store: Store<AppState>,
    private authService: AuthService,
    private planeTypeDas: PlaneTypeDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService
  ) { }

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();
    this.planesTypes$ = this.store.select(getAllPlanesTypes);
    this.totalCount$ = this.store.select(getPlanesTypesTotalCount);
    this.loading$ = this.store.select(getPlaneTypeLoadingGetAll);
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onCreate() {
    this.store.dispatch(openDialogNew());
  }

  onEdit(planeTypeId: number) {
    this.store.dispatch(load({ id: planeTypeId }));
    this.store.dispatch(openDialogEdit());
  }

  onDelete() {
    if (this.selectedPlanesTypes) {
      this.store.dispatch(multiRemove({ ids: this.selectedPlanesTypes.map((x) => x.id) }));
    }
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    this.store.dispatch(loadAllByPost({ event: lazyLoadEvent }));
  }

  onSelectedElementsChanged(planesTypes: PlaneType[]) {
    this.selectedPlanesTypes = planesTypes;
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
    this.planeTypeListComponent.getPrimeNgTable().columns.map((x: BiaFieldConfig) => (columns[x.field] = this.translateService.instant(x.header)));
    const customEvent: any = { columns: columns, ...this.planeTypeListComponent.getLazyLoadMetadata() };
    this.planeTypeDas.getFile(customEvent).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.planesTypes') + '.csv');
    });
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.PlaneType_Update);
    this.canDelete = this.authService.hasPermission(Permission.PlaneType_Delete);
    this.canAdd = this.authService.hasPermission(Permission.PlaneType_Create);
  }

  private initTableConfiguration() {
    this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          new BiaFieldConfig('title', 'planeType.title'),
          Object.assign(new BiaFieldConfig('certificationDate', 'planeType.certificationDate'), {
            type: PropType.Date,
            formatDate: dateFormat.dateTimeFormat
          })
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    }));
  }
}
