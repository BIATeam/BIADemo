import { NgClass, NgTemplateOutlet } from '@angular/common';
import {
  AfterContentInit,
  AfterViewInit,
  Component,
  ContentChildren,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  Renderer2,
  SimpleChanges,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormControl,
} from '@angular/forms';
import { TABLE_FILTER_GLOBAL } from '@bia-team/bia-ng/core';
import { BiaTableState, KeyValuePair } from '@bia-team/bia-ng/models';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { FilterMetadata, PrimeTemplate, SelectItem } from 'primeng/api';
import { Badge } from 'primeng/badge';
import { Button } from 'primeng/button';
import { FloatLabel } from 'primeng/floatlabel';
import { InputText } from 'primeng/inputtext';
import { MultiSelect } from 'primeng/multiselect';
import { Tooltip } from 'primeng/tooltip';
import { Subscription } from 'rxjs';
import { View } from '../../../features/view/model/view';
import { ViewListComponent } from '../../../features/view/views/view-list/view-list.component';

@Component({
  selector: 'bia-table-controller',
  templateUrl: './bia-table-controller.component.html',
  styleUrls: ['./bia-table-controller.component.scss'],
  imports: [
    NgClass,
    ViewListComponent,
    MultiSelect,
    FormsModule,
    NgTemplateOutlet,
    InputText,
    ReactiveFormsModule,
    Button,
    Tooltip,
    TranslateModule,
    FloatLabel,
    Badge,
  ],
})
export class BiaTableControllerComponent
  implements OnChanges, OnInit, OnDestroy, AfterContentInit, AfterViewInit
{
  @Input() defaultPageSize: number;
  @Input() columns: KeyValuePair[];
  @Input() columnToDisplays: KeyValuePair[];
  @Input() tableStateKey: string | undefined;
  @Input() tableState: string;
  @Input() useViewTeamWithTypeId: number | null;
  @Input() defaultViewPref: BiaTableState;
  @Input() hasColumnFilter = false;

  @Input() hasFilter = false;
  @Input() showFilter = false;
  @Input() showBtnFilter = false;

  @Output() displayedColumnsChange = new EventEmitter<KeyValuePair[]>();
  @Output() filter = new EventEmitter<string>();
  @Output() toggleSearch = new EventEmitter<void>();
  @Output() viewChange = new EventEmitter<string>();
  @Output() clearFilters = new EventEmitter<void>();
  @Output() openFilter = new EventEmitter<void>();
  @Output() selectedViewChanged = new EventEmitter<View | null>();

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  @ViewChild(ViewListComponent, { static: false })
  viewListComponent: ViewListComponent;

  @ViewChild('contentContainer') contentContainer: ElementRef;
  @ViewChild('overflowingContent') overflowingContent: ElementRef;
  @ViewChild('overflowingContentButton', { static: false })
  overflowingContentButton: Button;

  customControlTemplate: TemplateRef<any>;
  selectedViewName: string | null;
  listedColumns: SelectItem[];
  filterCtrl = new UntypedFormControl();
  globalFilter = '';
  displayedColumns: string[];
  defaultDisplayedColumns: string[];
  firstChange = true;

  protected sub = new Subscription();
  isOverflowing: boolean;
  overflowingContentActive = false;
  resizeObserver: ResizeObserver;
  overflowingWidth: number;
  overflowingContentOutsideClickListener: any;

  constructor(
    public translateService: TranslateService,
    protected renderer: Renderer2
  ) {}

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
  }

  ngAfterViewInit() {
    this.resizeObserver = new ResizeObserver(() => {
      this.checkOverflow();
    });

    this.resizeObserver.observe(this.contentContainer.nativeElement);
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }

    if (this.resizeObserver) {
      this.resizeObserver.disconnect();
    }
  }

  get overflowingClass() {
    const styleClass: { [key: string]: any } = {
      /* eslint-disable @typescript-eslint/naming-convention */
      'bia-overflowing-content': this.isOverflowing,
      'bia-overflowing-content-active': this.overflowingContentActive,
    };
    /* eslint-enable @typescript-eslint/naming-convention */
    return styleClass;
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

  onClearFilters() {
    this.filterCtrl.setValue('');
    this.clearFilters.emit();
  }

  onViewChange(event: string) {
    this.setControlByViewState(event);
    setTimeout(() => this.viewChange.emit(event));
  }

  onSelectedViewChanged(event: View | null) {
    setTimeout(() => this.selectedViewChanged.emit(event));
  }

  public getSelectedView(): View | null {
    return this.viewListComponent?.getCurrentView();
  }

  protected onColumnsChange(changes: SimpleChanges) {
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
          const tmpListedColumns = new Array<SelectItem>();
          this.columns.forEach(col => {
            tmpListedColumns.push({
              label: results[col.value],
              value: col.key,
            });
          });
          tmpListedColumns.sort((a, b) => {
            const labelA = a.label || '';
            const labelB = b.label || '';
            return labelA.localeCompare(labelB);
          });
          this.listedColumns = [...tmpListedColumns];
        })
      );
    }
  }

  protected initFilterCtrl() {
    this.sub.add(
      this.filterCtrl.valueChanges.subscribe(filterValue => {
        this.filter.emit(filterValue.trim());
      })
    );
  }

  protected setControlByViewState(stateString: string) {
    const state: BiaTableState = <BiaTableState>JSON.parse(stateString);
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

  @HostListener('window:resize')
  onResize() {
    this.checkOverflow();
  }

  checkOverflow() {
    const container = this.contentContainer.nativeElement;
    if (!this.isOverflowing) {
      this.overflowingWidth = container.scrollWidth;
    }

    if (this.overflowingWidth > container.clientWidth) {
      this.isOverflowing = true;
    } else {
      this.isOverflowing = false;
    }
  }

  toggleOverflowingContent() {
    this.overflowingContentActive = !this.overflowingContentActive;
    if (this.overflowingContentActive) {
      this.overflowingContentSubscription();
    }
  }

  hideOverflowingContent() {
    this.overflowingContentActive = false;
    if (this.overflowingContentOutsideClickListener) {
      this.overflowingContentOutsideClickListener();
      this.overflowingContentOutsideClickListener = null;
    }
  }

  protected overflowingContentSubscription() {
    this.overflowingContentOutsideClickListener = this.renderer.listen(
      'document',
      'click',
      event => {
        const isOutsideClicked = !(
          this.overflowingContent.nativeElement.isSameNode(event.target) ||
          this.overflowingContent.nativeElement.contains(event.target) ||
          this.overflowingContentButton.el.nativeElement.isSameNode(
            event.target
          ) ||
          this.overflowingContentButton.el.nativeElement.contains(event.target)
        );
        if (isOutsideClicked) {
          this.hideOverflowingContent();
        }
      }
    );
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
    if (this.showFilter === true) {
      this.openFilter.emit();
    }
  }
}
