import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';
import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
  NumberMode,
  PropType,
} from '../model/bia-field-config';

@Pipe({ name: 'formatValue' })
@Injectable({ providedIn: 'root' })
export class FormatValuePipe implements PipeTransform {
  constructor(
    protected datePipe: DatePipe,
    protected currencyPipe: CurrencyPipe,
    protected decimalPipe: DecimalPipe,
    protected translateService: TranslateService
  ) {}

  transform<TDto>(value: any, col: BiaFieldConfig<TDto>): string | null {
    if (value === null || value === undefined) {
      return null;
    }
    if (col.type === PropType.Number) {
      if (
        col.displayFormat === null ||
        !(col.displayFormat instanceof BiaFieldNumberFormat)
      ) {
        return this.decimalPipe.transform(value);
      } else if (col.displayFormat.mode === NumberMode.Currency) {
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
      } else if (this.isTime(value) || col.type === PropType.Time) {
        return this.datePipe.transform(
          '1990-10-10 ' + value,
          col.displayFormat.autoFormatDate
        );
      }
      try {
        return this.datePipe.transform(value, col.displayFormat.autoFormatDate);
      } catch {
        return this.translateService.instant('bia.errorDateFormat');
      }
    }
    return value;
  }

  isTime(value: any) {
    if (!value) {
      return false;
    }

    return /^\d{1,2}:\d{2}$/.test(value.toString());
  }
}
