import { DatePipe } from '@angular/common';
import { DestroyRef, Pipe, PipeTransform, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
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
  private readonly destroyRef = inject(DestroyRef);

  constructor(
    private readonly datePipe: DatePipe,
    private readonly biaTranslationService: BiaTranslationService
  ) {
    this.biaTranslationService.currentCultureDateFormat$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(dateFormat => {
        this.dateFormat = dateFormat;
      });
  }

  transform(value: Date, withTime: boolean = false): string | null {
    const format = this.dateFormat
      ? withTime
        ? this.dateFormat.dateTimeFormat
        : this.dateFormat.dateFormat
      : undefined;
    return this.datePipe.transform(value, format);
  }
}
