import { animate, style, transition, trigger } from '@angular/animations';
import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { SelectItem, TableState } from 'primeng/primeng';
import { KeyValuePair } from '../../../model/key-value-pair';
import { DEFAULT_VIEW, DEFAULT_PAGE_SIZE, TABLE_FILTER_GLOBAL } from 'src/app/shared/constants';

@Component({
  selector: 'bia-table-controller',
  templateUrl: './bia-table-controller.component.html',
  styleUrls: ['./bia-table-controller.component.scss'],
  animations: [
    trigger('options', [
      transition(':enter', [style({ height: 0 }), animate('200ms ease-out', style({ height: '*' }))]),
      transition(':leave', [style({ height: '*' }), animate('200ms ease-out', style({ height: 0 }))])
    ])
  ]
})
export class BiaTableControllerComponent implements OnChanges, OnInit, OnDestroy {
  @Input() pageSizeOptions: number[] = [10, 25, 50, 100];
  @Input() defaultPageSize: number;
  @Input() length: number;
  @Input() columns: KeyValuePair[];
  @Input() columnToDisplays: KeyValuePair[];
  @Input() tableStateKey: string;

  @Output() displayedColumnsChange = new EventEmitter<KeyValuePair[]>();
  @Output() filter = new EventEmitter<string>();
  @Output() pageSizeChange = new EventEmitter<number>();
  @Output() toggleSearch = new EventEmitter();
  @Output() viewChange = new EventEmitter<string>();

  pageSize: number;
  pageSizes: SelectItem[];
  resultMessageMapping = {
    '=0': 'bia.noResult',
    '=1': 'bia.result',
    other: 'bia.results'
  };
  listedColumns: SelectItem[];
  filterCtrl = new FormControl();
  globalFilter = '';
  displayedColumns: string[];
  defaultDisplayedColumns: string[];
  firstChange = true;

  private sub = new Subscription();

  constructor(public translateService: TranslateService) {}

  ngOnInit() {
    this.initPageSize();
    this.updateDisplayedPageSizeOptions();
    this.initFilterCtrl();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.onColumnsChange(changes);

    if (changes.pageSizeOptions) {
      this.updateDisplayedPageSizeOptions();
    }
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onPageSizeChange() {
    this.pageSizeChange.emit(Number(this.pageSize));
  }

  onChangeSelectColumn() {
    const cols = this.columns.filter((x) => this.displayedColumns.indexOf(x.key) > -1);
    this.displayedColumnsChange.emit(cols);
  }

  onToggleSearch() {
    this.toggleSearch.emit();
  }

  onViewChange(event: string) {
    this.setControlByViewState(event);
    setTimeout(() => this.viewChange.emit(event));
  }

  private onColumnsChange(changes: SimpleChanges) {
    if (
      this.firstChange === true &&
      this.columns &&
      this.columnToDisplays &&
      (changes.columns || changes.columnToDisplays)
    ) {
      this.firstChange = false;
      const cols = this.columns.map((x) => x.value);
      this.defaultDisplayedColumns = this.columnToDisplays.map((x) => x.key);
      this.displayedColumns = this.defaultDisplayedColumns;
      this.sub.add(
        this.translateService.stream(cols).subscribe((results) => {
          this.listedColumns = new Array<SelectItem>();
          this.columns.forEach((col) => {
            this.listedColumns.push({ label: results[col.value], value: col.key });
          });
        })
      );
    }
  }

  private initFilterCtrl() {
    this.sub.add(
      this.filterCtrl.valueChanges.subscribe((filterValue) => {
        this.filter.emit(filterValue.trim().toLowerCase());
      })
    );
  }

  private initPageSize() {
    this.pageSize = this.defaultPageSize;
  }

  private updateDisplayedPageSizeOptions() {
    if (this.pageSizeOptions) {
      const displayedPageSizeOptions = this.pageSizeOptions.sort((a, b) => a - b);

      this.pageSizes = new Array<SelectItem>();
      displayedPageSizeOptions.forEach((displayedPageSizeOption) => {
        this.pageSizes.push({ label: displayedPageSizeOption.toString(), value: displayedPageSizeOption });
      });
    }
  }

  private setControlByViewState(stateString: string) {
    if (stateString === DEFAULT_VIEW) {
      this.pageSize = this.defaultPageSize;
      this.displayedColumns = this.defaultDisplayedColumns;
      this.globalFilter = '';
    } else {
      const state: TableState = <TableState>JSON.parse(stateString);
      this.pageSize = state.rows ? state.rows : DEFAULT_PAGE_SIZE;
      this.displayedColumns = state.columnOrder ? state.columnOrder : [];
      for (const key in state.filters) {
        if (key.startsWith(TABLE_FILTER_GLOBAL)) {
          this.globalFilter = state.filters[key].value;
          break;
        }
      }
    }
  }
}
