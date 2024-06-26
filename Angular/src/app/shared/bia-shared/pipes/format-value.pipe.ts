import { CurrencyPipe, DatePipe, DecimalPipe, Time } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { BiaFieldConfig, PropType } from '../model/bia-field-config';

@Pipe({
  name: 'formatValue',
})
export class FormatValuePipe implements PipeTransform {
  constructor(
    private datePipe: DatePipe,
    private currencyPipe: CurrencyPipe,
    private decimalPipe: DecimalPipe
  ) {}
  transform(value: any, col: BiaFieldConfig): string | null {
    if (value === null || value === undefined) {
      return null;
    }
    if (col.type == PropType.Currency) {
      return this.currencyPipe.transform(
        value,
        col.outputFormat[0],
        col.outputFormat[1],
        col.outputFormat[2],
        col.culture
      );
    }
    if (
      col.type == PropType.Float ||
      col.type == PropType.Double ||
      col.type == PropType.Number
    ) {
      return this.decimalPipe.transform(
        value,
        col.outputFormat[0],
        col.culture
      );
    }
    if (col.isDate) {
      if (value instanceof Date) {
        return this.datePipe.transform(value, col.formatDate);
      } else if (this.isTime(value)) {
        return this.datePipe.transform(
          '1990-10-10 ' + value.hours + ':' + value.minutes,
          col.formatDate
        );
      } else if (col.type === PropType.Time) {
        return this.datePipe.transform('1990-10-10 ' + value, col.formatDate);
      }
      return this.datePipe.transform(value, col.formatDate);
    }
    return value;
  }

  isTime(value: Time | any): value is Time {
    return (
      (<Time>value).hours !== undefined && (<Time>value).minutes !== undefined
    );
  }
}
