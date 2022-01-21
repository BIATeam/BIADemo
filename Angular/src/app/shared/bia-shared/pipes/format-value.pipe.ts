import { DatePipe, Time } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { PrimeTableColumn, PropType } from '../components/table/bia-table/bia-table-config';

@Pipe({
  name: 'formatValue',
})
export class FormatValuePipe implements PipeTransform {
  constructor(private datePipe: DatePipe){
  }
  transform(value: any, col: PrimeTableColumn ): string | null {
    if (value === null || value === undefined)
    {
      return null;
    }
    if (col.isDate)
    {
      if (value instanceof Date)
      {
        return this.datePipe.transform(value, col.formatDate);
      }
      else if (this.isTime(value))
      {
        return this.datePipe.transform('1990-10-10 '+ value.hours + ":" + value.minutes , col.formatDate);
      }
      else if (col.type === PropType.Time)
      {
        return this.datePipe.transform('1990-10-10 '+ value, col.formatDate);
      }
      return this.datePipe.transform(value, col.formatDate);
    }
    return value
  }

  isTime(value: Time | any): value is Time {
    return (<Time>value).hours !== undefined && (<Time>value).minutes !== undefined;
 }
}
