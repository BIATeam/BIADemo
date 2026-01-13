import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import {
  BiaTranslationService,
  DateFormat,
} from 'packages/bia-ng/core/public-api';

@Pipe({
  name: 'localeDate',
  pure: false,
})
export class LocaleDatePipe implements PipeTransform {
  dateFormat: DateFormat;

  constructor(
    private readonly datePipe: DatePipe,
    private readonly biaTranslationService: BiaTranslationService
  ) {
    this.biaTranslationService.currentCultureDateFormat$.subscribe(
      dateFormat => {
        this.dateFormat = dateFormat;
      }
    );
  }

  transform(value: Date, withTime: boolean = false): string | null {
    return this.datePipe.transform(
      value,
      withTime ? this.dateFormat.dateTimeFormat : this.dateFormat.dateFormat
    );
  }
}
