import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { FilterMetadata } from 'primeng/api';
import { Table } from 'primeng/table';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaFieldConfig, PropType} from 'src/app/shared/bia-shared/model/bia-field-config';

@Component({
  selector: 'bia-table-filter',
  templateUrl: './bia-table-filter.component.html',
  styleUrls: ['./bia-table-filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class BiaTableFilterComponent implements OnInit, OnDestroy {
  @Input() col: BiaFieldConfig;
  @Input() table: Table;

  @Output() valueChange = new EventEmitter();
  @Output() complexInput = new EventEmitter<boolean>();

  protected sub = new Subscription();
  
  constructor(
    public biaTranslationService: BiaTranslationService
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
        if (element.value != undefined)
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
      if (this.table.filters && this.table.filters[col.field] && 'value' in this.table.filters[col.field])
      {
        return (this.table.filters[col.field] as FilterMetadata)['value'] != undefined
      }
    }
    return false; 
  }

  setSimpleFilter(value:any, col: BiaFieldConfig)
  {
    this.table.filter(value, col.field, col.filterMode)
  }

  private initFieldConfiguration() {
    if (
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
  }
}
