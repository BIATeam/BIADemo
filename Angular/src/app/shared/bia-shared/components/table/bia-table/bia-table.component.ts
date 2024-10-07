import {
  AfterContentInit,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  QueryList,
  SimpleChanges,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PrimeTemplate, TableState } from 'primeng/api';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { Observable, of, timer } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { TABLE_FILTER_GLOBAL } from 'src/app/shared/constants';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from '../../../model/bia-field-config';
import { BiaTableState } from '../../../model/bia-table-state';
import { KeyValuePair } from '../../../model/key-value-pair';
import { TableHelperService } from '../../../services/table-helper.service';

const objectsEqual = (o1: any, o2: any) =>
  Object.keys(o1).length === Object.keys(o2).length &&
  Object.keys(o1).every(p => o1[p] === o2[p]);
const arraysEqual = (a1: any, a2: any) =>
  (!a1 && !a2) ||
  (a1 &&
    a2 &&
    a1.length === a2.length &&
    a1.every((o: any, idx: any) => objectsEqual(o, a2[idx])));

@Component({
  selector: 'bia-table',
  templateUrl: './bia-table.component.html',
  styleUrls: ['./bia-table.component.scss'],
})
export class BiaTableComponent implements OnChanges, AfterContentInit {
  @Input() pageSize: number;
  @Input() totalRecord: number;
  @Input() paginator = true;
  @Input() elements: any[];
  @Input() columnToDisplays: KeyValuePair[];
  @Input() reorderableColumns = true;
  @Input() sortFieldValue = '';
  @Input() sortOrderValue = 1;
  @Input() showColSearch = false;
  @Input() globalSearchValue = '';
  @Input() canClickRow = true;
  @Input() canSelectElement = true;
  @Input() loading = false;
  @Input() tableStateKey: string | undefined;
  @Input() viewPreference: string;
  @Input() actionColumnLabel = 'bia.actions';
  @Input() showLoadingAfter = 100;
  @Input() scrollHeightValue = 'calc( 100vh - 450px)';
  @Input() isScrollable = true;
  @Input() frozeSelectColumn = false;
  @Input() canSelectMultipleElement = true;

  protected isSelectFrozen = false;
  protected widthSelect: string;
  protected alignFrozenSelect = 'left';
  protected _configuration: BiaFieldsConfig;
  get configuration(): BiaFieldsConfig {
    return this._configuration;
  }
  @Input() set configuration(value: BiaFieldsConfig) {
    this._configuration = value;
    this.manageSelectFrozen(value);
  }

  @Output() clickRowId = new EventEmitter<number>();
  @Output() clickRowData = new EventEmitter<any>();
  @Output() filter = new EventEmitter<number>();
  @Output() loadLazy = new EventEmitter<TableLazyLoadEvent>();
  @Output() selectedElementsChanged = new EventEmitter<any[]>();
  @Output() stateSave = new EventEmitter<string>();

  @ViewChild('dt', { static: false }) table: Table | undefined;

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  // specificInputTemplate: TemplateRef<any>;
  specificOutputTemplate: TemplateRef<any>;

  protected _selectedElements: any[] = [];
  get selectedElements(): any[] {
    return this._selectedElements;
  }
  set selectedElements(value: any[]) {
    this._selectedElements = value;
  }

  displayedColumns: BiaFieldConfig[];
  showLoading$: Observable<any>;

  protected defaultSortField: string;
  protected defaultSortOrder: number;
  protected defaultPageSize: number;
  protected defaultColumns: string[];

  constructor(
    public authService: AuthService,
    public translateService: TranslateService
  ) {}

  ngAfterContentInit() {
    this.templates.forEach(item => {
      switch (item.getType()) {
        /*case 'specificInput':
          this.specificInputTemplate = item.template;
        break;*/
        case 'specificOutput':
          this.specificOutputTemplate = item.template;
          break;
      }
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    this.onElementsChange(changes);
    this.onLoadingChange(changes);
    this.onConfigurationChange(changes);
    this.onColumnToDisplayChange(changes);
    this.onPageSizeChange(changes);
    this.onSearchGlobalChange(changes);
    this.onViewPreferenceChange(changes);
    this.onAdvancedFilterChange(changes);
  }

  protected onElementsChange(changes: SimpleChanges) {
    if (changes.elements && this.table) {
      if (
        this.selectedElements &&
        this.selectedElements.length > 0 &&
        this.elements
      ) {
        this.selectedElements = this.elements.filter(x =>
          this.selectedElements.some(y => x.id === y.id)
        );
      } else {
        this.selectedElements = [];
      }
      this.onSelectionChange();
    }
  }

  protected onLoadingChange(changes: SimpleChanges) {
    if (changes.loading) {
      if (this.loading === true) {
        this.showLoading$ = timer(this.showLoadingAfter);
      } else {
        this.showLoading$ = of(undefined);
      }
    }
  }

  protected onAdvancedFilterChange(changes: SimpleChanges) {
    if (changes.advancedFilter) {
      this.saveTableState();
    }
  }

  protected onConfigurationChange(changes: SimpleChanges) {
    if (this.configuration && changes.configuration) {
      this.updateDisplayedColumns(true);
    }
  }

  protected onColumnToDisplayChange(changes: SimpleChanges) {
    if (
      this.displayedColumns &&
      this.columnToDisplays &&
      changes.columnToDisplays
    ) {
      if (changes.columnToDisplays.isFirstChange()) {
        this.initDefaultSort();
        this.defaultPageSize = this.pageSize;
        this.defaultColumns = this.displayedColumns.map(x => x.field);
      }
      this.updateDisplayedColumns(true);
    }
  }

  protected initDefaultSort() {
    if (
      this.sortFieldValue.length < 1 &&
      this.displayedColumns &&
      this.displayedColumns.length > 0
    ) {
      this.sortFieldValue = this.displayedColumns[0].field;
    }
    this.defaultSortField = this.sortFieldValue;
    this.defaultSortOrder = this.sortOrderValue;
  }

  protected updateDisplayedColumns(saveTableState: boolean) {
    //setTimeout(() =>{
    const columns: BiaFieldConfig[] = this.getColumns();
    let displayedColumns;
    if (this.columnToDisplays) {
      displayedColumns = columns.filter(
        col => this.columnToDisplays.map(x => x.key).indexOf(col?.field) > -1
      );
    } else {
      displayedColumns = columns.slice();
    }

    if (arraysEqual(displayedColumns, this.displayedColumns) !== true) {
      this.displayedColumns = displayedColumns;
      if (saveTableState === true) {
        this.saveTableState();
      }
    }
    //});
  }

  saveStateNoEmit() {
    if (this.table?.stateKey !== undefined && this.table.stateKey !== '') {
      // Copy of the PrimeNG funcion (replace this by this.table) and comment emit and add custom
      const storage = this.table.getStorage();
      const state: any = {};
      if (this.table.paginator) {
        state.first = this.table.first;
        state.rows = this.table.rows;
      }
      if (this.table.sortField) {
        state.sortField = this.table.sortField;
        state.sortOrder = this.table.sortOrder;
      }
      if (this.table.multiSortMeta) {
        state.multiSortMeta = this.table.multiSortMeta;
      }
      if (this.table.hasFilter()) {
        state.filters = this.table.filters;
      }
      if (this.table.resizableColumns) {
        this.table.saveColumnWidths(state);
      }
      //if (this.table.reorderableColumns) {
      this.table.saveColumnOrder(state);
      //}
      if (this.table.selection) {
        state.selection = this.table.selection;
      }
      if (Object.keys(this.table.expandedRowKeys).length) {
        state.expandedRowKeys = this.table.expandedRowKeys;
      }

      const customState: any = this.configuration.advancedFilter
        ? { advancedFilter: this.configuration.advancedFilter, ...state }
        : state;

      storage.setItem(this.table.stateKey, JSON.stringify(customState));
      //this.table.onStateSave.emit(state);
    }
  }

  protected getColumns(): BiaFieldConfig[] {
    const tableState: BiaTableState | null = this.getTableState();
    let columns: BiaFieldConfig[] = [];
    let columnOrder: string[] | undefined = [];
    if (tableState && tableState.columnOrder) {
      columnOrder = tableState.columnOrder;
    } else if (this.table) {
      columnOrder = this.table.columns?.map(x => String(x.field));
    }

    if (columnOrder && columnOrder?.length > 0) {
      // The primeTableColumns are sorted by columnOrder.
      columnOrder.forEach(colName => {
        const column: BiaFieldConfig = this.configuration.columns.filter(
          col => col.field === colName
        )[0];
        columns.push(column);
      });

      // A hide then show column does not appear in sorted fields,
      // We start by finding them (missingColumns)
      const missingColumns = this.configuration.columns.filter(
        col => columnOrder?.indexOf(col.field) === -1
      );

      missingColumns.forEach(missingColumn => {
        // Then, missing columns are added in relation to their initial position
        const index: number = this.configuration.columns.indexOf(missingColumn);
        columns.splice(index, 0, missingColumn);
      });
    } else {
      columns = this.configuration.columns;
    }

    return columns;
  }

  protected onPageSizeChange(changes: SimpleChanges) {
    if (changes.pageSize && this.table) {
      this.table.onPageChange({ first: 0, rows: Number(this.pageSize) });
    }
  }

  protected onSearchGlobalChange(changes: SimpleChanges) {
    if (changes.globalSearchValue) {
      this.searchGlobalChanged(this.globalSearchValue);
    }
  }

  protected firstViewPreferenceApply = false;

  protected onViewPreferenceChange(changes: SimpleChanges) {
    if (
      this.table &&
      this.table.isStateful() &&
      changes.viewPreference &&
      this.tableStateKey
    ) {
      const viewPreference: BiaTableState = JSON.parse(
        changes.viewPreference.currentValue
      );

      // compatibility switch sort multiple to single
      if (this.table.sortMode === 'multiple') {
        if (
          viewPreference.multiSortMeta === undefined &&
          viewPreference.sortField != null &&
          viewPreference.sortField != '' &&
          viewPreference.sortOrder != null
        ) {
          viewPreference.multiSortMeta = [
            {
              field: viewPreference.sortField,
              order: viewPreference.sortOrder,
            },
          ];
        }
      } else {
        if (
          viewPreference.multiSortMeta !== undefined &&
          viewPreference.multiSortMeta.length > 0
        ) {
          viewPreference.sortField = viewPreference.multiSortMeta[0].field;
          viewPreference.sortOrder = viewPreference.multiSortMeta[0].order;
        }
      }

      const sViewPreference = JSON.stringify(viewPreference);

      if (
        !this.firstViewPreferenceApply ||
        sessionStorage.getItem(this.tableStateKey) !== sViewPreference
      ) {
        this.firstViewPreferenceApply = true;
        sessionStorage.setItem(this.tableStateKey, sViewPreference);
        this.restoreStateTable();
      }
    }
  }

  /** @deprecated use clickElementId instead */
  clickElement(itemId: number) {
    this.clickElementId(itemId);
  }

  clickElementId(itemId: number) {
    if (this.canClickRow === true) {
      this.clickRowId.emit(itemId);
    }
  }

  clickElementData(rowData: any) {
    if (this.canClickRow === true) {
      this.clickRowData.emit(rowData);
      if (
        rowData &&
        Object.prototype.hasOwnProperty.call(rowData, 'id') === true
      ) {
        this.clickElementId(rowData.id);
      }

      if (this.canSelectElement && !this.canSelectMultipleElement) {
        this.selectedElements = [];
      }
    }
  }

  searchGlobalChanged(value: string) {
    if (this.table) {
      if (this.table.lazy === true) {
        this.configuration.columns.forEach(col => {
          if (col.isSearchable === true && col.type !== PropType.Boolean) {
            this.table?.filter(
              value,
              TABLE_FILTER_GLOBAL + col.field,
              col.filterMode
            );
          }
        });
      } else {
        this.table.filterGlobal(value, 'contains');
      }
    }
  }

  onFilter() {
    if (this.table) {
      this.filter.emit(this.table.totalRecords);
      this.table.saveState();
    }
  }

  onLoadLazy(event: TableLazyLoadEvent) {
    this.saveTableState();
    const tableLazyLoadCopy: TableLazyLoadEvent =
      TableHelperService.copyTableLazyLoadEvent(event);
    setTimeout(() => this.loadLazy.emit(tableLazyLoadCopy), 0);
  }

  onSelectionChange() {
    setTimeout(() => {
      let selectedElements = this.selectedElements;
      if (
        this.canSelectMultipleElement === false &&
        !(selectedElements instanceof Array)
      ) {
        selectedElements = selectedElements ? [selectedElements] : [];
      }
      this.selectedElementsChanged.next(selectedElements);
    }, 0);
  }

  onStateSave(state: TableState) {
    if (this.table && Object.keys(state).length) {
      const customState: BiaTableState = {
        advancedFilter: this.configuration.advancedFilter,
        ...state,
      };
      if (this.table.stateKey !== undefined && this.table.stateKey !== '') {
        const storage = this.table.getStorage();
        const jsonCustomState: string = JSON.stringify(customState);
        storage.setItem(this.table.stateKey, jsonCustomState);
        setTimeout(() => this.stateSave.emit(jsonCustomState), 0);
      }
    }
  }

  protected saveTableState() {
    if (this.table && this.table.isStateful()) {
      this.table.columns = this.displayedColumns;
      //this.table.saveState();
      this.saveStateNoEmit();
    }
  }

  protected restoreStateTable() {
    if (this.table) {
      const tableState: BiaTableState | null = this.getTableState();
      if (tableState?.columnOrder) {
        this.updateDisplayedColumns(false);
        this.table.restoreState();

        if (this.table.sortMode === 'multiple') {
          this.table.sortMultiple();
        } else {
          this.table.sortSingle();
        }

        this.showColSearch = false;
        if (this.table.hasFilter()) {
          for (const key in this.table.filters) {
            if (!key.startsWith(TABLE_FILTER_GLOBAL)) {
              this.showColSearch = true;
              break;
            }
          }
        }
      }
    }
  }

  protected getTableState(): BiaTableState | null {
    if (this.tableStateKey) {
      const stateString: string | null = sessionStorage.getItem(
        this.tableStateKey
      );
      if (stateString && stateString?.length > 0) {
        const state: BiaTableState = JSON.parse(stateString);
        return state;
      }
    }

    return null;
  }

  /**
   * If in the configuration, we have at least one frozen column, we freeze the select column.
   * @param biaFieldsConfig
   */
  protected manageSelectFrozen(biaFieldsConfig: BiaFieldsConfig) {
    if (!this.frozeSelectColumn && biaFieldsConfig) {
      if (biaFieldsConfig?.columns?.some(x => x.isFrozen === true) === true) {
        this.isSelectFrozen = true;
        this.widthSelect = '50px';
      } else {
        this.isSelectFrozen = false;
        this.widthSelect = '';
      }
    } else {
      this.isSelectFrozen = true;
      this.widthSelect = '50px';
    }
  }

  hasPermission(permission: string): boolean {
    return this.authService.hasPermission(permission);
  }

  getLazyLoadMetadata(): TableLazyLoadEvent {
    const lazyLoadEvent = this.table?.createLazyLoadMetadata();
    if (lazyLoadEvent) {
      return TableHelperService.copyTableLazyLoadEvent(lazyLoadEvent);
    }
    return {};
  }

  getLazyLoadOnInit(): boolean {
    return !this.tableStateKey;
  }

  getPrimeNgTable(): Table | undefined {
    return this.table;
  }

  getCellData(rowData: any, col: any): any {
    const nestedProperties: string[] = col.field.split('.');
    let value: any = rowData;
    for (const prop of nestedProperties) {
      if (value == null) {
        return null;
      }

      value = value[prop];
    }

    return value;
  }
}
