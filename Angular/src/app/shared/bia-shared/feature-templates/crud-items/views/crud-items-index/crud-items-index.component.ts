import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { HttpStatusCode } from '@angular/common/http';
import {
  Component,
  HostBinding,
  Injector,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router, Routes } from '@angular/router';
import { Actions } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { saveAs } from 'file-saver';
import { PrimeTemplate } from 'primeng/api';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable, Subscription, combineLatest } from 'rxjs';
import { filter, first, skip, take, tap } from 'rxjs/operators';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOnlineOfflineService } from 'src/app/core/bia-core/services/bia-online-offline.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaButtonGroupItem } from 'src/app/shared/bia-shared/components/bia-button-group/bia-button-group.component';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';
import { BiaFieldConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaTableState } from 'src/app/shared/bia-shared/model/bia-table-state';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { FixableDto } from 'src/app/shared/bia-shared/model/dto/fixable-dto';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { TableHelperService } from 'src/app/shared/bia-shared/services/table-helper.service';
import { clone } from 'src/app/shared/bia-shared/utils';
import { DEFAULT_PAGE_SIZE, TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { BiaTableBehaviorControllerComponent } from '../../../../components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableHeaderComponent } from '../../../../components/table/bia-table-header/bia-table-header.component';
import { CrudItemTableComponent } from '../../components/crud-item-table/crud-item-table.component';
import { CrudConfig, FormReadOnlyMode } from '../../model/crud-config';
import { CrudItemService } from '../../services/crud-item.service';

@Component({
  selector: 'bia-crud-items-index',
  templateUrl: './crud-items-index.component.html',
  styleUrls: ['./crud-items-index.component.scss'],
  imports: [
    NgClass,
    BiaTableHeaderComponent,
    PrimeTemplate,
    BiaTableBehaviorControllerComponent,
    NgIf,
    CrudItemTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableControllerComponent,
    BiaTableComponent,
  ],
})
export class CrudItemsIndexComponent<
    ListCrudItem extends BaseDto,
    CrudItem extends BaseDto = ListCrudItem,
  >
  implements OnInit, OnDestroy
{
  public crudConfiguration: CrudConfig<ListCrudItem>;
  useRefreshAtLanguageChange = false;

  @HostBinding('class') classes = 'bia-flex';
  @ViewChild(BiaTableComponent, { static: false })
  biaTableComponent: BiaTableComponent<ListCrudItem>;
  @ViewChild(BiaTableControllerComponent, { static: false })
  biaTableControllerComponent: BiaTableControllerComponent;
  @ViewChild(CrudItemTableComponent, { static: false })
  crudItemTableComponent: CrudItemTableComponent<ListCrudItem>;
  protected parentDisplayItemName$: Observable<string>;

  _showTableController = true;

  get showTableController(): boolean {
    return this._showTableController;
  }
  set showTableController(value: boolean) {
    this._showTableController = value;
  }

  public get crudItemListComponent() {
    if (!this.crudConfiguration.useCalcMode) {
      return this.biaTableComponent;
    }
    return this.crudItemTableComponent;
  }

  protected sub = new Subscription();
  protected permissionSub = new Subscription();
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  crudItems$: Observable<ListCrudItem[]>;
  lastLazyLoadEvent$: Observable<TableLazyLoadEvent>;
  virtualCrudItems: ListCrudItem[];
  selectedCrudItems: ListCrudItem[] = [];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  canSave = false;
  canSelect = false;
  canFix = false;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  reorderableColumns = true;
  viewPreference: string;
  popupTitle: string;
  tableStateKey: string | undefined;
  tableState: string;
  sortFieldValue = '';
  useViewTeamWithTypeId: TeamTypeId | null;
  defaultViewPref: BiaTableState;
  hasColumnFilter = false;
  isParentFixed = false;

  protected store: Store<AppState>;
  protected router: Router;
  public activatedRoute: ActivatedRoute;
  protected translateService: TranslateService;
  protected biaTranslationService: BiaTranslationService;
  protected authService: AuthService;
  protected tableHelperService: TableHelperService;
  protected layoutService: BiaLayoutService;
  protected actions: Actions;
  protected messageService: BiaMessageService;
  protected selectedButtonGroup: BiaButtonGroupItem[];
  protected listButtonGroup: BiaButtonGroupItem[];
  protected customButtonGroup: BiaButtonGroupItem[];

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemService<ListCrudItem, CrudItem>
  ) {
    this.store = this.injector.get<Store<AppState>>(Store);
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.translateService =
      this.injector.get<TranslateService>(TranslateService);
    this.biaTranslationService = this.injector.get<BiaTranslationService>(
      BiaTranslationService
    );
    this.authService = this.injector.get<AuthService>(AuthService);
    this.tableHelperService =
      this.injector.get<TableHelperService>(TableHelperService);
    this.layoutService = this.injector.get<BiaLayoutService>(BiaLayoutService);
    this.actions = this.injector.get<Actions>(Actions);
    this.messageService =
      this.injector.get<BiaMessageService>(BiaMessageService);
  }

  toggleTableControllerVisibility() {
    this.showTableController = !this.showTableController;
  }

  getFillScrollHeightValue(offset?: string) {
    return this.tableHelperService.getFillScrollHeightValue(
      this.layoutService,
      this.crudConfiguration.useCompactMode ?? false,
      this.showTableController ?? true,
      offset
    );
  }

  useViewChange(e: boolean) {
    this.crudConfiguration.useView = e;
    this.useViewConfig(true);
  }

  useCalcModeChange(e: boolean) {
    this.crudConfiguration.useCalcMode = e;
  }

  useSignalRChange(e: boolean) {
    this.crudConfiguration.useSignalR = e;
    this.useSignalRConfig(true);
  }

  usePopupChange(e: boolean) {
    this.crudConfiguration.usePopup = e;
    this.usePopupConfig(true);
  }

  useSplitChange(e: boolean) {
    this.crudConfiguration.useSplit = e;
    this.useSplitConfig(true);
  }

  useCompactModeChange(e: boolean) {
    this.crudConfiguration.useCompactMode = e;
  }

  useVirtualScrollChange(e: boolean) {
    this.crudConfiguration.useVirtualScroll = e;
    this.initVirtualScroll();
  }

  useResizableColumnChange(e: boolean) {
    this.crudConfiguration.useResizableColumn = e;
  }

  protected useViewConfig(manualChange: boolean) {
    this.tableStateKey = this.crudConfiguration.useView
      ? this.crudConfiguration.tableStateKey
      : undefined;
    this.useViewTeamWithTypeId = this.crudConfiguration.useView
      ? this.crudConfiguration.useViewTeamWithTypeId
      : null;
    if (this.crudConfiguration.useView) {
      if (manualChange) {
        setTimeout(() => {
          if (this.crudItemListComponent?.table) {
            this.crudItemListComponent.table.saveState();
          }
        });
      }
      this.store.dispatch(loadAllView());
    }
  }

  protected usePopupConfig(manualChange: boolean) {
    if (manualChange) {
      this.applyDynamicComponent(this.activatedRoute.routeConfig?.children);
    }
  }

  protected useSplitConfig(manualChange: boolean) {
    if (manualChange) {
      this.applyDynamicComponent(this.activatedRoute.routeConfig?.children);
    }
  }

  protected applyDynamicComponent(routes: Routes | undefined) {
    if (routes) {
      routes.forEach(route => {
        if (route.data && (route.data as any).dynamicComponent) {
          route.component = (route.data as any).dynamicComponent();
        }
        this.applyDynamicComponent(route.children);
      });
    }
  }

  protected useSignalRConfig(manualChange: boolean) {
    if (this.crudConfiguration.useSignalR) {
      this.crudItemService.signalRService.initialize(this.crudItemService);
      if (this.crudItemListComponent) {
        this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata());
      }
    } else {
      if (manualChange) {
        this.crudItemService.signalRService.destroy(this.crudItemService);
      }
    }
  }

  ngOnInit() {
    /*    this.tableStateKey = this.crudConfiguration.useView ? this.crudConfiguration.tableStateKey : undefined;
        this.useViewTeamWithTypeId = this.crudConfiguration.useView ? this.crudConfiguration.useViewTeamWithTypeId : null;
    */
    this.sub = new Subscription();

    this.initButtonGroups();
    this.initTableConfiguration();

    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.crudItemService.optionsService.loadAllOptions(
          this.crudConfiguration.optionFilter
        );
      })
    );

    this.sub.add(
      this.authService.authInfo$.subscribe((authInfo: AuthInfo) => {
        if (authInfo && authInfo.token !== '') {
          this.setPermissions();
          this.initButtonGroups();
        }
      })
    );

    this.crudItemService.clearAll();
    this.crudItems$ = this.crudItemService.crudItems$;
    this.totalCount$ = this.crudItemService.totalCount$;
    this.lastLazyLoadEvent$ = this.crudItemService.lastLazyLoadEvent$;
    this.loading$ = this.crudItemService.loadingGetAll$;
    this.onDisplay();

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

    this.initVirtualScroll();

    if (this.crudConfiguration.useOfflineMode) {
      if (BiaOnlineOfflineService.isModeEnabled) {
        this.sub.add(
          this.injector
            .get<BiaOnlineOfflineService>(BiaOnlineOfflineService)
            .syncCompleted$.pipe(
              skip(1),
              filter(x => x === true)
            )
            .subscribe(() => {
              this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata());
            })
        );
      }
    }
  }

  protected initVirtualScroll() {
    if (this.crudConfiguration.useVirtualScroll) {
      this.sub.add(
        combineLatest([this.crudItems$, this.totalCount$]).subscribe(data => {
          this.lastLazyLoadEvent$
            .pipe(
              take(1),
              tap(lastLazyLoadEvent => {
                if (lastLazyLoadEvent.first !== undefined) {
                  if (
                    data[1] > 0 &&
                    (!this.virtualCrudItems ||
                      data[1] !== this.virtualCrudItems.length)
                  ) {
                    this.virtualCrudItems = Array.from({ length: data[1] });
                  }

                  if (
                    this.virtualCrudItems &&
                    this.virtualCrudItems.length > 0
                  ) {
                    this.virtualCrudItems.splice(
                      lastLazyLoadEvent.first ?? 0,
                      lastLazyLoadEvent.rows ?? 0,
                      ...data[0]
                    );
                  }
                }
              })
            )
            .subscribe();
        })
      );
    }
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
    this.permissionSub.unsubscribe();
    this.onHide();
  }

  onDisplay() {
    this.checkhasAdvancedFilter();
    this.useViewConfig(false);
    this.useSignalRConfig(false);
    this.usePopupConfig(false);
  }

  onHide() {
    if (this.crudConfiguration.useSignalR) {
      this.crudItemService.signalRService.destroy(this.crudItemService);
    }
  }

  onCreate() {
    if (!this.crudConfiguration.useCalcMode) {
      this.router.navigate(['create'], { relativeTo: this.activatedRoute });
    }
  }

  onClone() {
    if (!this.crudConfiguration.useCalcMode) {
      this.router.navigate(['create'], {
        relativeTo: this.activatedRoute,
        state: { itemTemplateId: this.selectedCrudItems[0].id },
      });
    } else {
      this.crudItemTableComponent.initEditableRow({
        ...clone(this.selectedCrudItems[0]),
        id: 0,
      });
    }
  }

  onImport() {
    this.router.navigate(['import'], { relativeTo: this.activatedRoute });
  }

  onClickRow(crudItemId: any) {
    this.onEdit(crudItemId);
  }

  onEdit(crudItemId: any) {
    if (!this.crudConfiguration.useCalcMode) {
      const target =
        this.crudConfiguration.formEditReadOnlyMode !== FormReadOnlyMode.off
          ? 'read'
          : 'edit';
      this.router.navigate([crudItemId, target], {
        relativeTo: this.activatedRoute,
      });
    }
  }

  onChange() {
    if (this.crudConfiguration.useCalcMode) {
      this.crudItemTableComponent.onChange();
    }
  }

  onComplexInput(isIn: boolean) {
    if (this.crudConfiguration.useCalcMode) {
      this.crudItemTableComponent.onComplexInput(isIn);
    }
  }

  onSave(crudItem: CrudItem) {
    if (!this.crudConfiguration.useCalcMode) {
      return;
    }

    if (crudItem.id > 0 && this.canEdit) {
      this.handleCrudOperation(
        crudItem,
        this.crudItemService.updateSuccessActionType,
        this.crudItemService.updateFailureActionType,
        this.crudItemService.update.bind(this.crudItemService)
      );
    }

    if (crudItem.id === 0 && this.canAdd) {
      this.handleCrudOperation(
        crudItem,
        this.crudItemService.createSuccessActionType,
        undefined,
        this.crudItemService.create.bind(this.crudItemService)
      );
    }
  }

  protected handleCrudOperation(
    crudItem: CrudItem,
    successActionType: string | undefined,
    failureActionType: string | undefined,
    crudOperation: (item: CrudItem) => void
  ) {
    if (successActionType) {
      this.actions
        .pipe(
          filter((action: any) => action.type === successActionType),
          first()
        )
        .subscribe(() => {
          this.resetEditableRow();
        });
    }

    if (failureActionType) {
      this.actions
        .pipe(
          filter((action: any) => action.type === failureActionType),
          first()
        )
        .subscribe(action => {
          if (action.error?.status === HttpStatusCode.Conflict) {
            this.messageService.showWarning(
              this.translateService.instant('bia.outdatedData')
            );
          }
        });
    }

    crudOperation(crudItem);

    if (!successActionType) {
      this.resetEditableRow();
    }
  }

  protected resetEditableRow() {
    this.crudItemTableComponent.resetEditableRow();
  }

  onDelete() {
    if (this.canDelete) {
      const itemsToDelete = this.crudConfiguration.isFixable
        ? this.selectedCrudItems.filter(
            x => (x as unknown as FixableDto).isFixed === false
          )
        : this.selectedCrudItems;

      this.crudItemService.multiRemove(itemsToDelete.map(x => x.id));
    }
  }

  onSelectedElementsChanged(crudItems: ListCrudItem[]) {
    this.selectedCrudItems = crudItems;
    this.initButtonGroups();
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: TableLazyLoadEvent) {
    const pagingAndFilter: PagingFilterFormatDto = {
      advancedFilter: this.crudConfiguration.fieldsConfig.advancedFilter,
      parentIds: this.crudItemService.getParentIds().map(id => id.toString()),
      ...lazyLoadEvent,
    };
    this.crudItemService.loadAllByPost(pagingAndFilter);
    this.hasColumnFilter =
      this.tableHelperService.hasFilter(this.biaTableComponent) ||
      this.tableHelperService.hasFilter(this.crudItemTableComponent);
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
    this.updateAdvancedFilterByView(viewPreference);
  }

  onStateSave(tableState: string) {
    this.viewPreference = tableState;
    this.tableState = tableState;
  }

  onExportCSV(fileName = 'bia.crud.listOf', useAllColumn = false) {
    fileName = this.translateService.instant(fileName);

    const selectedViewName =
      this.biaTableControllerComponent.getSelectedViewName();
    if (selectedViewName && selectedViewName.length > 0) {
      fileName = `${fileName}-${selectedViewName}`;
    }

    const columns: { [key: string]: string } = {};

    if (useAllColumn === true) {
      const allColumns = [
        ...this.crudConfiguration.fieldsConfig.columns.filter(
          col => col.isVisibleInTable
        ),
      ];
      const columnIdExists = allColumns.some(column => column.field === 'id');

      if (columnIdExists !== true) {
        allColumns.unshift(new BiaFieldConfig<ListCrudItem>('id', 'bia.id'));
      }

      allColumns?.map(
        (x: BiaFieldConfig<ListCrudItem>) =>
          (columns[x.field.toString()] = this.translateService.instant(
            x.header
          ))
      );
    } else {
      this.crudItemListComponent
        .getPrimeNgTable()
        ?.columns?.map(
          (x: BiaFieldConfig<ListCrudItem>) =>
            (columns[x.field.toString()] = this.translateService.instant(
              x.header
            ))
        );
    }

    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.crudItemService.getParentIds().map(id => id.toString()),
      columns: columns,
      advancedFilter: this.crudConfiguration.fieldsConfig.advancedFilter,
      ...this.crudItemListComponent.getLazyLoadMetadata(),
    };
    this.crudItemService.getFile(columnsAndFilter).subscribe(data => {
      saveAs(data, fileName + '.csv');
    });
  }

  protected setPermissions() {
    this.permissionSub.unsubscribe();
    this.permissionSub = new Subscription();

    this.canEdit = true;
    this.canDelete = true;
    this.canAdd = true;
    this.canFix = false;
  }

  protected initTableConfiguration() {
    this.columns = this.crudConfiguration.fieldsConfig.columns
      .filter(col => col.isVisibleInTable)
      .map(col => <KeyValuePair>{ key: col.field, value: col.header });
    this.displayedColumns = this.crudConfiguration.fieldsConfig.columns
      .filter(col => col.isVisibleInTable && !col.isHideByDefault)
      .map(col => <KeyValuePair>{ key: col.field, value: col.header });
    this.sortFieldValue = this.columns[0].key;

    this.defaultViewPref = <BiaTableState>{
      first: 0,
      rows: this.defaultPageSize,
      sortField: this.sortFieldValue,
      sortOrder: 1,
      filters: {},
      columnOrder: this.crudConfiguration.fieldsConfig.columns
        .filter(col => col.isVisibleInTable && !col.isHideByDefault)
        .map(x => x.field),
      advancedFilter: undefined,
    };
  }

  // Advanced Filter;
  showAdvancedFilter = false;
  hasAdvancedFilter = false;

  onFilter(advancedFilter: any) {
    this.crudConfiguration.fieldsConfig.advancedFilter = advancedFilter;
    this.crudItemListComponent.table?.saveState();
    this.checkhasAdvancedFilter();
    this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata());
  }

  checkhasAdvancedFilter() {
    this.hasAdvancedFilter = false;
  }

  protected updateAdvancedFilterByView(viewPreference: string) {
    if (viewPreference) {
      const state = JSON.parse(viewPreference);
      if (state) {
        this.crudConfiguration.fieldsConfig.advancedFilter =
          state.advancedFilter;
        this.checkhasAdvancedFilter();
      }
    } else {
      this.crudConfiguration.fieldsConfig.advancedFilter = {};
      this.checkhasAdvancedFilter();
    }
  }

  onCloseFilter() {
    this.showAdvancedFilter = false;
  }

  onOpenFilter() {
    this.showAdvancedFilter = true;
  }

  get defaultRowHeight(): number {
    if (this.crudConfiguration.useCompactMode) {
      return this.layoutService.config().scale * 1.679 + 2;
    } else {
      return this.layoutService.config().scale * 2.257 + 2;
    }
  }

  onClearFilters() {
    const table = this.crudItemListComponent.getPrimeNgTable();
    if (table) {
      Object.keys(table.filters).forEach(key =>
        this.tableHelperService.clearFilterMetaData(table.filters[key])
      );
      table.onLazyLoad.emit(table.createLazyLoadMetadata());
    }
  }

  protected initButtonGroups() {
    this.initSelectedButtonGroup();
    this.initListButtonGroup();
    this.initCustomButtonGroup();
  }

  protected initSelectedButtonGroup() {
    this.selectedButtonGroup = [];
  }

  protected initListButtonGroup() {
    this.listButtonGroup = [];
  }

  protected initCustomButtonGroup() {
    this.customButtonGroup = [];
  }
}
