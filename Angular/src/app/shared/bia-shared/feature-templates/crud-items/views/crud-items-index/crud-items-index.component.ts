import { Component, HostBinding, Injector, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  PrimeTableColumn,
  PropType
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE, TeamTypeId } from 'src/app/shared/constants';
import { ActivatedRoute, Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { CrudItemTableComponent } from '../../components/crud-item-table/crud-item-table.component';
import { filter, skip } from 'rxjs/operators';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudItemService } from '../../services/crud-item.service';
import { CrudConfig } from '../../model/crud-config';
import { BiaOnlineOfflineService } from 'src/app/core/bia-core/services/bia-online-offline.service';

@Component({
  selector: 'app-crud-items-index',
  templateUrl: './crud-items-index.component.html',
  styleUrls: ['./crud-items-index.component.scss']
})
export class CrudItemsIndexComponent<CrudItem extends BaseDto> implements OnInit, OnDestroy {
  public crudConfiguration : CrudConfig;
  useRefreshAtLanguageChange = false;

  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) biaTableComponent: BiaTableComponent;
  @ViewChild(CrudItemTableComponent, { static: false }) crudItemTableComponent: CrudItemTableComponent<CrudItem>;
  protected get crudItemListComponent() {
    if (!this.crudConfiguration.useCalcMode) {
      return this.biaTableComponent;
    }
    return this.crudItemTableComponent;
  }

  protected sub = new Subscription();
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  crudItems$: Observable<CrudItem[]>;
  selectedCrudItems: CrudItem[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  tableConfiguration: BiaListConfig = { columns : []};
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  popupTitle: string;
  tableStateKey: string | undefined;
  tableState: string;
  sortFieldValue = 'msn';
  useViewTeamWithTypeId: TeamTypeId | null;
  parentIds: string[];

  protected store: Store<AppState>;
  protected router: Router;
  public activatedRoute: ActivatedRoute;
  protected translateService: TranslateService;
  protected biaTranslationService: BiaTranslationService;

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemService<CrudItem>, 
  ) {
    this.store = this.injector.get<Store<AppState>>(Store);
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.translateService = this.injector.get<TranslateService>(TranslateService);
    this.biaTranslationService = this.injector.get<BiaTranslationService>(BiaTranslationService);
  }

  useViewChange(e: boolean) {
    this.crudConfiguration.useView = e;
    this.useViewConfig(true);
  }

  useClacModeChange(e: boolean) {
    this.crudConfiguration.useCalcMode = e;
    this.useClacModeConfig(true);
  }

  useSignalRChange(e: boolean) {
    this.crudConfiguration.useSignalR = e;
    this.useSignalRConfig(true);
  }

  private useViewConfig(manualChange: boolean) {
    this.tableStateKey = this.crudConfiguration.useView ? this.crudConfiguration.tableStateKey : undefined;
    this.useViewTeamWithTypeId = this.crudConfiguration.useView ? this.crudConfiguration.useViewTeamWithTypeId : null;
    if (this.crudConfiguration.useView) {
      if (manualChange)
      {
        setTimeout(() => {
          if (this.crudItemListComponent?.table) 
          {
            this.crudItemListComponent.table.saveState();
          }
        });
      }
      this.store.dispatch(loadAllView());
    }
  }

  isLoadAllOptionsSubsribe = false;
  private useClacModeConfig(manualChange: boolean) {
    if (this.crudConfiguration.useCalcMode && ! this.isLoadAllOptionsSubsribe) {
      this.isLoadAllOptionsSubsribe = true;
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(event => {
          this.crudItemService.optionsService.loadAllOptions();
        })
      );
    }
  }

  private useSignalRConfig(manualChange: boolean) {
    if (this.crudConfiguration.useSignalR) {
      this.crudItemService.signalRService.initialize(this.crudItemService);
      this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata());
    }
    else
    {
      this.crudItemService.signalRService.destroy();
    }
  }

  ngOnInit() {
/*    this.tableStateKey = this.crudConfiguration.useView ? this.crudConfiguration.tableStateKey : undefined;
    this.useViewTeamWithTypeId = this.crudConfiguration.useView ? this.crudConfiguration.useViewTeamWithTypeId : null;
*/
    this.parentIds = [];
    this.sub = new Subscription();

    this.initTableConfiguration();
    this.setPermissions();
    this.crudItems$ = this.crudItemService.crudItems$;
    this.totalCount$ = this.crudItemService.totalCount$;
    this.loading$ = this.crudItemService.loadingGetAll$;
    this.OnDisplay();

    if (this.useRefreshAtLanguageChange) {
      // Reload data if language change.
      this.sub.add(
        this.biaTranslationService.currentCulture$.pipe(skip(1)).subscribe(event => {
          this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata());
        })
      );
    }

    if (this.crudConfiguration.useOfflineMode)
    {
      if (BiaOnlineOfflineService.isModeEnabled) {
        this.sub.add(
          this.injector.get<BiaOnlineOfflineService>(BiaOnlineOfflineService).syncCompleted$.pipe(skip(1), filter(x => x === true)).subscribe(() => {
            this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata());
          })
        );
      }
    }
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
    this.OnHide();
  }

  OnDisplay() {
    this.useViewConfig(false);
    this.useClacModeConfig(false);
    this.useSignalRConfig(false);    
    /*if (this.crudConfiguration.useView) {
      this.store.dispatch(loadAllView());
    }*/
  }

  OnHide() {
    if (this.crudConfiguration.useSignalR) {
      this.crudItemService.signalRService.destroy();
    }
  }

  onCreate() {
    if (!this.crudConfiguration.useCalcMode) {
      this.router.navigate(['create'], { relativeTo: this.activatedRoute });
    }
  }

  onEdit(crudItemId: number) {
    if (!this.crudConfiguration.useCalcMode) {
      this.router.navigate([crudItemId, 'edit'], { relativeTo: this.activatedRoute });
    }
  }
  
  onChange() {
    if (this.crudConfiguration.useCalcMode) {
      this.crudItemTableComponent.onChange();
    }
  }

  onSave(crudItem: CrudItem) {
    if (this.crudConfiguration.useCalcMode) {
      if (crudItem.id > 0) {
        if (this.canEdit) {
          this.crudItemService.update(crudItem);
        }
      } else {
        if (this.canAdd) {
          this.crudItemService.create(crudItem);
        }
      }
    }
  }

  onDelete() {
    if (this.selectedCrudItems && this.canDelete) {
      this.crudItemService.multiRemove(this.selectedCrudItems.map((x) => x.id));
    }
  }

  onSelectedElementsChanged(crudItems: CrudItem[]) {
    this.selectedCrudItems = crudItems;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    const pagingAndFilter: PagingFilterFormatDto = { parentIds: this.parentIds, ...lazyLoadEvent };
    this.crudItemService.loadAllByPost(pagingAndFilter);
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
    this.crudItemListComponent.getPrimeNgTable().columns.map((x: PrimeTableColumn) => (columns[x.field] = this.translateService.instant(x.header)));
    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds, columns: columns, ...this.crudItemListComponent.getLazyLoadMetadata()
    };
    this.crudItemService.dasService.getFile(columnsAndFilter).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.crudItems') + '.csv');
    });
  }

  protected setPermissions() {
    // TODO redefine in plane
    this.canEdit = true;
    this.canDelete = true;
    this.canAdd = true;
  }
  protected initTableConfiguration() {
    this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: this.crudConfiguration.columns.map<PrimeTableColumn>(object => object.clone())}
 
      this.tableConfiguration.columns.forEach(column => {
        switch (column.type)
        {
          case PropType.DateTime :
            column.formatDate = dateFormat.dateTimeFormat;
            break;
          case PropType.Date :
            column.formatDate = dateFormat.dateFormat;
            break;
          case PropType.Time :
            column.formatDate = dateFormat.timeFormat;
            break;
          case PropType.TimeOnly :
            column.formatDate = dateFormat.timeFormat;
            break;
          case PropType.TimeSecOnly :
            column.formatDate = dateFormat.timeFormatSec;
            break;
        }
      });
      
      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    }));
  }
}