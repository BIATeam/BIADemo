import {
  ChangeDetectionStrategy,
  Component,
  // EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
  // Output,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FilterMatchMode, FilterMetadata, SelectItem } from 'primeng/api';
import { Table } from 'primeng/table';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaFieldConfig, PropType} from 'src/app/shared/bia-shared/model/bia-field-config';

@Component({
  selector: 'bia-table-filter',
  templateUrl: './bia-table-filter.component.html',
  styleUrls: ['./bia-table-filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  encapsulation: ViewEncapsulation.None
})

export class BiaTableFilterComponent implements OnInit, OnDestroy {
  @Input() col: BiaFieldConfig;
  @Input() table: Table;

  // @Output() valueChange = new EventEmitter<void>();
  // @Output() complexInput = new EventEmitter<boolean>();

  public columnFilterType : string = '';
  protected matchModeOptions : SelectItem[] | undefined= undefined;
  protected sub = new Subscription();

  constructor(
    public biaTranslationService: BiaTranslationService,
    private translateService: TranslateService
    ) {
    
  }
  
  ngOnInit() {
    this.initFieldConfiguration();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  isArrayFilter(col: BiaFieldConfig)
  {
    let valueInArray = false;
    if (this.table && this.table.filters && Array.isArray(this.table.filters[col.field]))
    {
      (this.table.filters[col.field] as FilterMetadata[]).forEach(element => {
        if (element.value != undefined || element.matchMode === 'empty' || element.matchMode === 'notEmpty' )
        {
          valueInArray =true;
        }
      });
    }
    return valueInArray;
  }

  isArraySimple(col: BiaFieldConfig)
  {
    if (this.table)
    {
      if (this.table.filters)
      {
        let filter : FilterMetadata = this.table.filters[col.field] as FilterMetadata;
        if (filter)
        {
            return filter['value'] !== undefined || filter['matchMode'] === 'empty' || filter['matchMode'] === 'notEmpty';
        }
      }
    }
    return false; 
  }

  setSimpleFilter(value:any, col: BiaFieldConfig)
  {
    this.table.filter(value, col.field, col.filterMode)
  }

  private initFieldConfiguration() {
    if (this.col.type == PropType.Number)
    {
      this.columnFilterType = 'numeric';
      this.generateMatchModeOptions(this.filterMatchModeOptions.numeric);
    }
    else if (this.col.type == PropType.Boolean)
    {
      this.columnFilterType = 'boolean';
    }
    else if (
      this.col.type == PropType.DateTime
      ||
      this.col.type == PropType.Date
      ||
      this.col.type == PropType.Time
      ||
      this.col.type == PropType.TimeOnly
      ||
      this.col.type == PropType.TimeSecOnly
    )
    {
      this.generateMatchModeOptions(this.filterMatchModeOptions.date);
      this.columnFilterType = 'date';
      this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
        let field = this.col.clone();
        switch (field.type)
        {
          case PropType.DateTime :
            field.formatDate = dateFormat.dateTimeFormat;
            break;
          case PropType.Date :
            field.formatDate = dateFormat.dateFormat;
            break;
          case PropType.Time :
            field.formatDate = dateFormat.timeFormat;
            break;
          case PropType.TimeOnly :
            field.formatDate = dateFormat.timeFormat;
            break;
          case PropType.TimeSecOnly :
            field.formatDate = dateFormat.timeFormatSec;
            break;
        }
        this.col = field;
      }));
    }
    else{
      this.generateMatchModeOptions(this.filterMatchModeOptions.text);
      this.columnFilterType = 'text';
    }
  }
  filterMatchModeOptions = {
    text: [FilterMatchMode.STARTS_WITH, "notStartsWith", FilterMatchMode.CONTAINS, FilterMatchMode.NOT_CONTAINS, FilterMatchMode.ENDS_WITH, "notEndsWith", FilterMatchMode.EQUALS, FilterMatchMode.NOT_EQUALS, "empty", "notEmpty"],
    numeric: [FilterMatchMode.EQUALS, FilterMatchMode.NOT_EQUALS, FilterMatchMode.LESS_THAN, FilterMatchMode.LESS_THAN_OR_EQUAL_TO, FilterMatchMode.GREATER_THAN, FilterMatchMode.GREATER_THAN_OR_EQUAL_TO, "empty", "notEmpty"],
    date: [FilterMatchMode.DATE_IS, FilterMatchMode.DATE_IS_NOT, FilterMatchMode.DATE_BEFORE, FilterMatchMode.DATE_AFTER, "empty", "notEmpty"]
  };
  generateMatchModeOptions(option: string[]) {
    this.sub.add(this.biaTranslationService.currentCulture$.subscribe(() => {
      this.matchModeOptions =
          option?.map((key: string) => {
              return { label: this.translateService.instant("primeng." + key), value: key };
          });
      this.resetColumnFilter()
    }));
  }

  // use to force the refresh du to langage conflict. PrimeNg issue #14273
  showColumnFilter:boolean = true
  resetColumnFilter(){
    this.showColumnFilter = false;
 
    setTimeout(() => {
       this.showColumnFilter = true
     });
 }
}
