import {
  NgClass,
  NgFor,
  NgIf,
  NgSwitch,
  NgSwitchCase,
  NgSwitchDefault,
} from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  // EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'biang/core';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
  OptionDto,
} from 'biang/models';
import { PrimeNGFiltering, PropType } from 'biang/models/enum';
import {
  FilterMatchMode,
  FilterMetadata,
  PrimeTemplate,
  SelectItem,
} from 'primeng/api';
import { FloatLabel } from 'primeng/floatlabel';
import { InputText } from 'primeng/inputtext';
import { MultiSelect } from 'primeng/multiselect';
import { Table, TableModule } from 'primeng/table';
import { Subscription } from 'rxjs';
import { FormatValuePipe } from '../../../pipes/format-value.pipe';
import { TableHelperService } from '../../../services/table-helper.service';
import { BiaFieldBaseComponent } from '../../form/bia-field-base/bia-field-base.component';

@Component({
  selector: 'bia-table-filter',
  templateUrl: './bia-table-filter.component.html',
  styleUrls: ['./bia-table-filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  encapsulation: ViewEncapsulation.None,
  imports: [
    NgIf,
    TableModule,
    PrimeTemplate,
    MultiSelect,
    FormsModule,
    NgFor,
    NgSwitch,
    NgSwitchCase,
    NgSwitchDefault,
    InputText,
    NgClass,
    TranslateModule,
    FormatValuePipe,
    FloatLabel,
  ],
})
export class BiaTableFilterComponent<CrudItem>
  extends BiaFieldBaseComponent<CrudItem>
  implements OnInit, OnDestroy
{
  @Input() table: Table;
  @Input() options?: OptionDto[];

  propType = PropType;

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

  getOptionsLabels(value: number[]): string {
    return value
      .map(v => this.options?.find(o => o.id === v)?.display)
      .join(',');
  }

  ngOnInit() {
    this.initFiterConfiguration();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  isArrayFilter(col: BiaFieldConfig<CrudItem>): FilterMetadata[] | null {
    if (
      this.table &&
      this.table.filters &&
      Array.isArray(this.table.filters[col.field])
    ) {
      if (
        (this.table.filters[col.field] as FilterMetadata[]).some(
          element => !TableHelperService.isEmptyFilter(element)
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

  isArraySimple(col: BiaFieldConfig<CrudItem>) {
    if (this.table) {
      if (this.table.filters) {
        const filter: FilterMetadata = this.table.filters[
          col.field
        ] as FilterMetadata;
        if (filter) {
          return !TableHelperService.isEmptyFilter(filter);
        }
      }
    }
    return false;
  }

  setSimpleFilter(event: any, col: BiaFieldConfig<CrudItem>) {
    const separator = ',';
    if (
      col.type === PropType.ManyToMany &&
      event?.value?.toString().indexOf(separator) > -1
    ) {
      if (
        event?.value?.slice(-1) !== ' ' &&
        event?.value?.slice(-1) !== separator
      ) {
        const valuesArray: string[] = event?.value
          .split(separator)
          .map((value: string) => value.trim());
        this.table.filter(
          valuesArray,
          col.field.toString(),
          PrimeNGFiltering.In
        );
      }
    } else {
      this.table.filter(event?.value, col.field.toString(), col.filterMode);
    }
  }

  protected initFiterConfiguration() {
    this.initFieldConfiguration();
    if (this.field.type === PropType.Number) {
      this.columnFilterType = 'numeric';
      this.generateMatchModeOptions(this.filterMatchModeOptions.numeric);
    } else if (this.field.type === PropType.Boolean) {
      this.columnFilterType = 'boolean';
    } else if (
      this.field.type === PropType.DateTime ||
      this.field.type === PropType.Date
    ) {
      this.generateMatchModeOptions(this.filterMatchModeOptions.date);
      this.columnFilterType = 'date';
    } else if (
      this.field.type === PropType.Time ||
      this.field.type === PropType.TimeOnly ||
      this.field.type === PropType.TimeSecOnly
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

  setFilterConstraint(filterConstraint: FilterMetadata, value: OptionDto[]) {
    filterConstraint.value = value.map(x => x.display);
  }

  onMultiSelectChange(filterConstraint: any) {
    if (!filterConstraint.value || filterConstraint.value.length === 0) {
      filterConstraint.value = null;
    }
  }
}
