import { animate, style, transition, trigger } from '@angular/animations';
import {
  AfterContentInit,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  SimpleChanges,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { FilterMetadata, PrimeTemplate, SelectItem } from 'primeng/api';
import { KeyValuePair } from '../../../model/key-value-pair';
import {
  DEFAULT_PAGE_SIZE,
  TABLE_FILTER_GLOBAL,
  TeamTypeId,
} from 'src/app/shared/constants';
import { BiaTableState } from '../../../model/bia-table-state';
import { ViewListComponent } from '../../../features/view/views/view-list/view-list.component';

@Component({
  selector: 'bia-table-controller',
  templateUrl: './bia-table-controller.component.html',
  styleUrls: ['./bia-table-controller.component.scss'],
  animations: [
    trigger('options', [
      transition(':enter', [
        style({ height: 0 }),
        animate('200ms ease-out', style({ height: '*' })),
      ]),
      transition(':leave', [
        style({ height: '*' }),
        animate('200ms ease-out', style({ height: 0 })),
      ]),
    ]),
  ],
})
export class BiaTableControllerComponent
  implements OnChanges, OnInit, OnDestroy, AfterContentInit
{
  @Input() pageSizeOptions: number[] = [10, 25, 50, 100];
  @Input() defaultPageSize: number;
  @Input() length: number;
  @Input() columns: KeyValuePair[];
  @Input() columnToDisplays: KeyValuePair[];
  @Input() tableStateKey: string;
  @Input() tableState: string;
  @Input() useViewTeamWithTypeId: TeamTypeId | null;
  @Input() defaultViewPref: BiaTableState;
  @Input() hasColumnFilter = false;

  @Output() displayedColumnsChange = new EventEmitter<KeyValuePair[]>();
  @Output() filter = new EventEmitter<string>();
  @Output() pageSizeChange = new EventEmitter<number>();
  @Output() toggleSearch = new EventEmitter<void>();
  @Output() viewChange = new EventEmitter<string>();

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  @ViewChild(ViewListComponent, { static: false })
  viewListComponent: ViewListComponent;

  customControlTemplate: TemplateRef<any>;
  selectedViewName: string | null;
  pageSize: number;
  pageSizes: SelectItem[];
  resultMessageMapping = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    '=0': 'bia.noResult',
    // eslint-disable-next-line @typescript-eslint/naming-convention
    '=1': 'bia.result',
    other: 'bia.results',
  };
  listedColumns: SelectItem[];
  filterCtrl = new UntypedFormControl();
  globalFilter = '';
  displayedColumns: string[];
  defaultDisplayedColumns: string[];
  firstChange = true;

  private sub = new Subscription();

  constructor(public translateService: TranslateService) {}

  ngAfterContentInit() {
    this.templates.forEach(item => {
      switch (item.getType()) {
        case 'customControl':
          this.customControlTemplate = item.template;
          break;
      }
    });
  }

  ngOnInit() {
    this.initPageSize();
    this.updateDisplayedPageSizeOptions();
    this.initFilterCtrl();
    if (this.defaultViewPref === undefined) {
      // compatibility with old system
      this.defaultViewPref = <BiaTableState>{
        first: 0,
        rows: this.defaultPageSize,
        sortField: this.columns[0].key,
        sortOrder: 1,
        filters: {},
        columnOrder: this.columns.map(x => x.key),
        advancedFilter: undefined,
      };
    }
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
    const cols = this.columns.filter(
      x => this.displayedColumns.indexOf(x.key) > -1
    );
    setTimeout(() => this.displayedColumnsChange.emit(cols));
  }

  onToggleSearch() {
    this.toggleSearch.emit();
  }

  onViewChange(event: string) {
    this.setControlByViewState(event);
    setTimeout(() => this.viewChange.emit(event));
  }

  public getSelectedViewName(): string | null {
    return this.viewListComponent?.getCurrentViewName();
  }

  private onColumnsChange(changes: SimpleChanges) {
    if (
      this.firstChange === true &&
      this.columns &&
      this.columnToDisplays &&
      (changes.columns || changes.columnToDisplays)
    ) {
      this.firstChange = false;
      const cols = this.columns.map(x => x.value);
      this.defaultDisplayedColumns = this.columnToDisplays.map(x => x.key);
      this.displayedColumns = this.defaultDisplayedColumns;
      this.sub.add(
        this.translateService.stream(cols).subscribe(results => {
          this.listedColumns = new Array<SelectItem>();
          this.columns.forEach(col => {
            this.listedColumns.push({
              label: results[col.value],
              value: col.key,
            });
          });
        })
      );
    }
  }

  private initFilterCtrl() {
    this.sub.add(
      this.filterCtrl.valueChanges.subscribe(filterValue => {
        this.filter.emit(filterValue.trim().toLowerCase());
      })
    );
  }

  private initPageSize() {
    this.pageSize = this.defaultPageSize;
  }

  private updateDisplayedPageSizeOptions() {
    if (this.pageSizeOptions) {
      const displayedPageSizeOptions = this.pageSizeOptions.sort(
        (a, b) => a - b
      );

      this.pageSizes = new Array<SelectItem>();
      displayedPageSizeOptions.forEach(displayedPageSizeOption => {
        this.pageSizes.push({
          label: displayedPageSizeOption.toString(),
          value: displayedPageSizeOption,
        });
      });
    }
  }

  private setControlByViewState(stateString: string) {
    const state: BiaTableState = <BiaTableState>JSON.parse(stateString);
    this.pageSize = state.rows ? state.rows : DEFAULT_PAGE_SIZE;
    const newDisplayColumns = state.columnOrder ? state.columnOrder : [];
    if (this.displayedColumns !== newDisplayColumns) {
      this.displayedColumns = newDisplayColumns;
      this.onChangeSelectColumn();
    }
    for (const key in state.filters) {
      if (key.startsWith(TABLE_FILTER_GLOBAL)) {
        this.globalFilter = (state.filters[key] as FilterMetadata).value;
        break;
      }
    }
  }
}
