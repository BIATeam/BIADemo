import { CurrencyPipe, DatePipe, DecimalPipe, Time } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
  NumberMode,
  PropType,
} from '../model/bia-field-config';

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
    if (col.type == PropType.Number) {
      if (
        col.displayFormat == null ||
        !(col.displayFormat instanceof BiaFieldNumberFormat)
      ) {
        return this.decimalPipe.transform(value);
      } else if (col.displayFormat.mode == NumberMode.Currency) {
        return this.currencyPipe.transform(
          value,
          col.displayFormat.currency,
          col.displayFormat.currencyDisplay,
          col.displayFormat.outputFormat,
          col.displayFormat.autoLocale
        );
      } else {
        // Integer or Decimal
        return this.decimalPipe.transform(
          value,
          col.displayFormat.outputFormat,
          col.displayFormat.autoLocale
        );
      }
    }
    if (col.isDate && col.displayFormat instanceof BiaFieldDateFormat) {
      if (value instanceof Date) {
        return this.datePipe.transform(value, col.displayFormat.autoFormatDate);
      } else if (this.isTime(value)) {
        return this.datePipe.transform(
          '1990-10-10 ' + value.hours + ':' + value.minutes,
          col.displayFormat.autoFormatDate
        );
      } else if (col.type === PropType.Time) {
        return this.datePipe.transform(
          '1990-10-10 ' + value,
          col.displayFormat.autoFormatDate
        );
      }
      return this.datePipe.transform(value, col.displayFormat.autoFormatDate);
    }
    return value;
  }

  isTime(value: Time | any): value is Time {
    return (
      (<Time>value).hours !== undefined && (<Time>value).minutes !== undefined
    );
  }
}
