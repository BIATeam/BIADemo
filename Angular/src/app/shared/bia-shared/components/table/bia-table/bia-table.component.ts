import { Component, Input, ViewChild, Output, EventEmitter, OnChanges, SimpleChanges, ContentChildren, QueryList, TemplateRef, AfterContentInit } from '@angular/core';
import { Table } from 'primeng/table';
import { LazyLoadEvent, PrimeTemplate, TableState } from 'primeng/api';
import { BiaFieldsConfig, PropType, BiaFieldConfig } from '../../../model/bia-field-config';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { TABLE_FILTER_GLOBAL } from 'src/app/shared/constants';
import { KeyValuePair } from '../../../model/key-value-pair';
import { Observable, timer, of } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { BiaTableState } from '../../../model/bia-table-state';


const objectsEqual = (o1: any, o2: any) =>
  Object.keys(o1).length === Object.keys(o2).length
  && Object.keys(o1).every(p => o1[p] === o2[p]);
const arraysEqual = (a1: any, a2: any) =>
  (!a1 && !a2) || (a1 && a2 && a1.length === a2.length && a1.every((o: any, idx: any) => objectsEqual(o, a2[idx])));

@Component({
  selector: 'bia-table',
  templateUrl: './bia-table.component.html',
  styleUrls: ['./bia-table.component.scss']
})
export class BiaTableComponent implements OnChanges, AfterContentInit {
  @Input() pageSize: number;
  @Input() totalRecord: number;
  @Input() paginator = true;
  @Input() elements: any[];
  @Input() columnToDisplays: KeyValuePair[];
  @Input() sortFieldValue = '';
  @Input() sortOrderValue = 1;
  @Input() configuration: BiaFieldsConfig;
  @Input() showColSearch = false;
  @Input() globalSearchValue = '';
  @Input() canClickRow = true;

  @Input() canSelectElement = true;
  @Input() loading = false;
  @Input() tableStateKey: string;
  @Input() viewPreference: string;
  @Input() actionColumnLabel = 'bia.actions';
  @Input() showLoadingAfter = 100;

  @Output() clickRowId = new EventEmitter<number>();
  @Output() clickRowData = new EventEmitter<any>();
  @Output() filter = new EventEmitter<number>();
  @Output() loadLazy = new EventEmitter<LazyLoadEvent>();
  @Output() selectedElementsChanged = new EventEmitter<any[]>();
  @Output() stateSave = new EventEmitter<string>();

  @ViewChild('dt', { static: false }) table: Table;

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  // specificInputTemplate: TemplateRef<any>;
  specificOutputTemplate: TemplateRef<any>;

  get selectedElements(): any[] {
    if (this.table) {
      return this.table.selection as any[];
    }
    return {} as any[];
  }
  set selectedElements(value: any[]) {
    if (this.table) {
      this.table.selection = value;
    }
  }

  displayedColumns: BiaFieldConfig[];
  showLoading$: Observable<any>;

  protected defaultSortField: string;
  protected defaultSortOrder: number;
  protected defaultPageSize: number;
  protected defaultColumns: string[];

  constructor(public authService: AuthService, public translateService: TranslateService) { }

  ngAfterContentInit() {
    this.templates.forEach((item) => {
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
      if (this.selectedElements && this.selectedElements.length > 0 && this.elements) {
        this.selectedElements = this.elements.filter((x) => this.selectedElements.some((y) => x.id === y.id));
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
    if (this.displayedColumns && this.columnToDisplays && changes.columnToDisplays) {
      if (changes.columnToDisplays.isFirstChange()) {
        this.initDefaultSort();
        this.defaultPageSize = this.pageSize;
        this.defaultColumns = this.displayedColumns.map((x) => x.field);
      }
      this.updateDisplayedColumns(true);
    }
  }


  protected initDefaultSort() {
    if (this.sortFieldValue.length < 1 && this.displayedColumns && this.displayedColumns.length > 0) {
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
        (col) => this.columnToDisplays.map((x) => x.key).indexOf(col?.field) > -1
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
    if (this.table.stateKey !== undefined && this.table.stateKey !== '') {
      // Copy of the PrimeNG funcion (replace this by this.table) and comment emit and add custom
      const storage = this.table.getStorage();
      let state: any = {};
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
      if (this.table.reorderableColumns) {
        this.table.saveColumnOrder(state);
      }
      if (this.table.selection) {
        state.selection = this.table.selection;
      }
      if (Object.keys(this.table.expandedRowKeys).length) {
        state.expandedRowKeys = this.table.expandedRowKeys;
      }

      const customState: any = this.configuration.advancedFilter ? { advancedFilter: this.configuration.advancedFilter, ...state } : state;

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
        const column: BiaFieldConfig = this.configuration.columns.filter((col) => col.field === colName)[0];
        columns.push(column);
      });

      // A hide then show column does not appear in sorted fields,
      // We start by finding them (missingColumns)
      const missingColumns = this.configuration.columns.filter(
        (col) => columnOrder?.indexOf(col.field) === -1
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

  private firstViewPreferenceApply: boolean = false;

  protected onViewPreferenceChange(changes: SimpleChanges) {
    if (this.table && this.table.isStateful() && changes.viewPreference) {
      let viewPreference = changes.viewPreference.currentValue;
      if (!this.firstViewPreferenceApply || sessionStorage.getItem(this.tableStateKey) !== viewPreference) {
        this.firstViewPreferenceApply = true;
        sessionStorage.setItem(this.tableStateKey, viewPreference);
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
      if (rowData && rowData.hasOwnProperty('id') === true) {
        this.clickElementId(rowData.id);
      }
    }
  }

  searchGlobalChanged(value: string) {
    if (this.table) {
      if (this.table.lazy === true) {
        this.configuration.columns.forEach((col) => {
          if (col.isSearchable === true && col.type !== PropType.Boolean) {
            this.table.filter(value, TABLE_FILTER_GLOBAL + col.field, col.filterMode);
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

  onLoadLazy(event: LazyLoadEvent) {
    this.saveTableState();
    setTimeout(() =>
      this.loadLazy.emit(event)
      , 0);
  }

  onSelectionChange() {
    setTimeout(() => this.selectedElementsChanged.next(this.selectedElements), 0);
  }

  onStateSave(state: TableState) {
    if (this.table && Object.keys(state).length) {
      const customState: BiaTableState = { advancedFilter: this.configuration.advancedFilter, ...state };
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
        this.table.sortSingle();

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
    const stateString: string | null = sessionStorage.getItem(this.tableStateKey);
    if (stateString && stateString?.length > 0) {
      const state: BiaTableState = JSON.parse(stateString);
      return state;
    }

    return null;
  }

  hasPermission(permission: string): boolean {
    return this.authService.hasPermission(permission);
  }

  getLazyLoadMetadata(): LazyLoadEvent {
    return this.table.createLazyLoadMetadata();
  }

  getLazyLoadOnInit(): boolean {
    return !this.tableStateKey;
  }

  getPrimeNgTable(): Table {
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
