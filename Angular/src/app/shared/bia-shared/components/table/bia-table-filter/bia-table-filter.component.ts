import {
  ChangeDetectionStrategy,
  Component,
  // EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FilterMatchMode, FilterMetadata, SelectItem } from 'primeng/api';
import { Table } from 'primeng/table';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { TableHelperService } from '../../../services/table-helper.service';
import { BiaFieldBaseComponent } from '../../form/bia-field-base/bia-field-base.component';

@Component({
  selector: 'bia-table-filter',
  templateUrl: './bia-table-filter.component.html',
  styleUrls: ['./bia-table-filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  encapsulation: ViewEncapsulation.None,
})
export class BiaTableFilterComponent
  extends BiaFieldBaseComponent
  implements OnInit, OnDestroy
{
  @Input() table: Table;

  // @Output() valueChange = new EventEmitter<void>();
  // @Output() complexInput = new EventEmitter<boolean>();

  public columnFilterType = '';
  protected matchModeOptions: SelectItem[] | undefined = undefined;
  protected sub = new Subscription();

  constructor(
    public biaTranslationService: BiaTranslationService,
    protected translateService: TranslateService,
    protected tableHelperService: TableHelperService
  ) {
    super(biaTranslationService);
  }

  getDisplayNumberFormat(
    displayFormat: BiaFieldNumberFormat | BiaFieldDateFormat | null
  ): BiaFieldNumberFormat | null {
    return displayFormat && displayFormat instanceof BiaFieldNumberFormat
      ? displayFormat
      : null;
  }

  ngOnInit() {
    this.initFiterConfiguration();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  isArrayFilter(col: BiaFieldConfig): FilterMetadata[] | null {
    if (
      this.table &&
      this.table.filters &&
      Array.isArray(this.table.filters[col.field])
    ) {
      if (
        (this.table.filters[col.field] as FilterMetadata[]).some(
          element => !this.tableHelperService.isEmptyFilter(element)
        )
      ) {
        return this.table.filters[col.field] as FilterMetadata[];
      }
    }
    return null;
  }

  isSimpleFilter(
    filter: FilterMetadata | FilterMetadata[] | undefined
  ): FilterMetadata | null {
    return filter && !Array.isArray(filter) ? filter : null;
  }

  isArraySimple(col: BiaFieldConfig) {
    if (this.table) {
      if (this.table.filters) {
        const filter: FilterMetadata = this.table.filters[
          col.field
        ] as FilterMetadata;
        if (filter) {
          return !this.tableHelperService.isEmptyFilter(filter);
        }
      }
    }
    return false;
  }

  setSimpleFilter(event: any, col: BiaFieldConfig) {
    this.table.filter(event?.value, col.field, col.filterMode);
  }

  protected initFiterConfiguration() {
    this.initFieldConfiguration();
    if (this.field.type == PropType.Number) {
      this.columnFilterType = 'numeric';
      this.generateMatchModeOptions(this.filterMatchModeOptions.numeric);
    } else if (this.field.type == PropType.Boolean) {
      this.columnFilterType = 'boolean';
    } else if (
      this.field.type == PropType.DateTime ||
      this.field.type == PropType.Date
    ) {
      this.generateMatchModeOptions(this.filterMatchModeOptions.date);
      this.columnFilterType = 'date';
    } else if (
      this.field.type == PropType.Time ||
      this.field.type == PropType.TimeOnly ||
      this.field.type == PropType.TimeSecOnly
    ) {
      this.generateMatchModeOptions(this.filterMatchModeOptions.date);
      this.columnFilterType = 'text';
    } else {
      this.generateMatchModeOptions(this.filterMatchModeOptions.text);
      this.columnFilterType = 'text';
    }
  }
  filterMatchModeOptions = {
    text: [
      FilterMatchMode.STARTS_WITH,
      'notStartsWith',
      FilterMatchMode.CONTAINS,
      FilterMatchMode.NOT_CONTAINS,
      FilterMatchMode.ENDS_WITH,
      'notEndsWith',
      FilterMatchMode.EQUALS,
      FilterMatchMode.NOT_EQUALS,
    ],
    numeric: [
      FilterMatchMode.EQUALS,
      FilterMatchMode.NOT_EQUALS,
      FilterMatchMode.LESS_THAN,
      FilterMatchMode.LESS_THAN_OR_EQUAL_TO,
      FilterMatchMode.GREATER_THAN,
      FilterMatchMode.GREATER_THAN_OR_EQUAL_TO,
    ],
    date: [
      FilterMatchMode.DATE_IS,
      FilterMatchMode.DATE_IS_NOT,
      FilterMatchMode.DATE_BEFORE,
      FilterMatchMode.DATE_AFTER,
    ],
  };
  generateMatchModeOptions(option: string[]) {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.matchModeOptions = option?.map((key: string) => {
          return {
            label: this.translateService.instant('primeng.' + key),
            value: key,
          };
        });
        if (this.field.isRequired === false) {
          this.matchModeOptions.push(
            {
              label: this.translateService.instant('primeng.empty'),
              value: 'empty',
            },
            {
              label: this.translateService.instant('primeng.notEmpty'),
              value: 'notEmpty',
            }
          );
        }
        this.resetColumnFilter();
      })
    );
  }

  // use to force the refresh du to langage conflict. PrimeNg issue #14273
  showColumnFilter = true;
  resetColumnFilter() {
    this.showColumnFilter = false;

    setTimeout(() => {
      this.showColumnFilter = true;
    });
  }
}
