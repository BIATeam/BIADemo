import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { FilterMetadata } from 'primeng/api';
import { Table } from 'primeng/table';
import { BiaFieldConfig} from 'src/app/shared/bia-shared/model/bia-field-config';

@Component({
  selector: 'bia-table-filter',
  templateUrl: './bia-table-filter.component.html',
  styleUrls: ['./bia-table-filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class BiaTableFilterComponent  {
  @Input() col: BiaFieldConfig;
  @Input() table: Table;

  @Output() valueChange = new EventEmitter();
  @Output() complexInput = new EventEmitter<boolean>();
  
  constructor(

    ) {
    
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
}
