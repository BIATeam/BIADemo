import { Component, Input, ViewChild, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { Table } from 'primeng/table';
import { LazyLoadEvent, TableState } from 'primeng/api';
import { BiaListConfig, PropType, PrimeTableColumn } from './bia-table-config';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { DEFAULT_VIEW, TABLE_FILTER_GLOBAL } from 'src/app/shared/constants';
import { KeyValuePair } from '../../../model/key-value-pair';
import { Observable, timer, of } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'bia-table',
  templateUrl: './bia-table.component.html',
  styleUrls: ['./bia-table.component.scss']
})
export class BiaTableComponent implements OnChanges {
  @Input() pageSize: number;
  @Input() totalRecord: number;
  @Input() paginator = true;
  @Input() elements: any[];
  @Input() columnToDisplays: KeyValuePair[];
  @Input() sortFieldValue = '';
  @Input() sortOrderValue = 1;
  @Input() configuration: BiaListConfig;
  @Input() showColSearch = false;
  @Input() globalSearchValue = '';
  @Input() canClickRow = true;

  @Input() canSelectElement = true;
  @Input() loading = false;
  @Input() tableStateKey: string;
  @Input() viewPreference: string;
  @Input() advancedFilter: any;
  @Input() actionColumnLabel = 'bia.actions';
  @Input() showLoadingAfter = 100;

  @Output() clickRow = new EventEmitter<number>();
  @Output() filter = new EventEmitter<number>();
  @Output() loadLazy = new EventEmitter<LazyLoadEvent>();
  @Output() selectedElementsChanged = new EventEmitter<any[]>();
  @Output() colReorder = new EventEmitter<KeyValuePair[]>();

  @ViewChild('dt', { static: false }) table: Table;

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

  displayedColumns: PrimeTableColumn[];
  showLoading$: Observable<any>;

  protected defaultSortField: string;
  protected defaultSortOrder: number;
  protected defaultPageSize: number;
  protected defaultColumns: string[];

  constructor(public authService: AuthService, public translateService: TranslateService) { }

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
    if (this.columnToDisplays && changes.columnToDisplays) {
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
    const columns: PrimeTableColumn[] = this.getColumns();
    if (this.columnToDisplays) {
      this.displayedColumns = columns.filter(
        (col) => this.columnToDisplays.map((x) => x.key).indexOf(col.field) > -1
      );
    } else {
      this.displayedColumns = columns.slice();
    }
    if (saveTableState === true) {
      this.saveTableState();
    }
  }

  protected getColumns(): PrimeTableColumn[] {
    const tableState: TableState | null = this.getTableState();
    let columns: PrimeTableColumn[] = [];
    let columnOrder: string[] = [];
    if (this.table) {
      columnOrder = this.table.columns.map(x => x.field);
    } else if (tableState && tableState.columnOrder) {
      columnOrder = tableState.columnOrder;
    }

    if (columnOrder && columnOrder?.length > 0) {

      // The primeTableColumns are sorted by columnOrder.
      columnOrder.forEach(colName => {
        const column: PrimeTableColumn = this.configuration.columns.filter((col) => col.field === colName)[0];
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

  protected onViewPreferenceChange(changes: SimpleChanges) {
    if (this.table && this.table.isStateful() && changes.viewPreference) {
      let viewPreference = changes.viewPreference.currentValue;
      if (viewPreference === DEFAULT_VIEW) {
        const defaultState: TableState = this.createDefaultTableState();
        viewPreference = JSON.stringify(defaultState);
      }
      sessionStorage.setItem(this.tableStateKey, viewPreference);
      this.restoreStateTable();
    }
  }

  protected createDefaultTableState(): TableState {
    return <TableState>{
      first: 0,
      rows: this.defaultPageSize,
      sortField: this.defaultSortField,
      sortOrder: this.defaultSortOrder,
      filters: {},
      columnOrder: this.defaultColumns
    };
  }

  clickElement(itemId: number) {
    if (this.canClickRow) {
      this.clickRow.emit(itemId);
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
    setTimeout(() => this.loadLazy.emit(event), 0);
  }

  onSelectionChange() {
    setTimeout(() => this.selectedElementsChanged.next(this.selectedElements), 0);
  }

  onStateSave(state: TableState) {
    if (this.table && Object.keys(state).length) {
      const customState: any = this.advancedFilter ? { advancedFilter: this.advancedFilter, ...state } : state;
      if (this.table.stateKey !== undefined && this.table.stateKey !== '') {
        const storage = this.table.getStorage();
        storage.setItem(this.table.stateKey, JSON.stringify(customState));
      }
    }
  }

  onColReorder(event: { dragIndex: number, dropIndex: number, columns: PrimeTableColumn[] }) {
    const displayedColumns: KeyValuePair[] = event.columns.map(x => <KeyValuePair>{ key: x.field, value: x.header });
    this.colReorder.emit(displayedColumns);
  }

  protected saveTableState() {
    if (this.table && this.table.isStateful()) {
      this.table.columns = this.displayedColumns;
      this.table.saveState();
    }
  }

  protected restoreStateTable() {
    if (this.table) {
      const tableState: TableState | null = this.getTableState();
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

  protected getTableState(): TableState | null {
    const stateString: string | null = sessionStorage.getItem(this.tableStateKey);
    if (stateString && stateString?.length > 0) {
      const state: TableState = JSON.parse(stateString);
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
}
