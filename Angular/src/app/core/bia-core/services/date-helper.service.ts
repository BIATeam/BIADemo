import { Injectable } from '@angular/core';
import { parse } from 'date-fns';

@Injectable({
  providedIn: 'root',
})
export class DateHelperService {
  public static isDate(value: any): boolean {
    const regex = /(\d{4})-(\d{2})-(\d{2})T/;
    if (typeof value === 'string' && value.match(regex)) {
      const date = Date.parse(value);
      return !isNaN(date);
    }
    return false;
  }

  public static fillDate<TOut>(data: TOut) {
    if (data) {
      Object.keys(data).forEach((key: string) => {
        const value = (data as any)[key];
        if (value instanceof Date === true) {
          (data as any)[key] = DateHelperService.toUtc(value);
        } else if (DateHelperService.isDate(value)) {
          (data as any)[key] = new Date(value);
        } else if (value instanceof Object === true) {
          this.fillDate(value);
        }
      });
    }
  }

  public static toUtc(date: Date): Date {
    return new Date(
      Date.UTC(
        date.getFullYear(),
        date.getMonth(),
        date.getDate(),
        date.getHours(),
        date.getMinutes(),
        date.getSeconds()
      )
    );
  }

  public static isValidDate(d: Date): boolean {
    return !isNaN(d.getTime());
  }

  public static parseDate(
    dateString: string,
    dateFormat: string | null = null,
    timeFormat: string | null = null
  ): Date {
    const timePattern = /:/;

    dateString = dateString.replace('  ', ' ').trim();

    // Handle custom format if provided
    if (dateFormat != null) {
      let format = dateFormat;
      if (timePattern.test(dateString)) {
        format = dateFormat + ' ' + timeFormat;
      }
      return parse(dateString, format, new Date());
    } else {
      // Attempt to parse the date directly
      let parsedDate = new Date(dateString);
      if (!isNaN(parsedDate.getTime())) {
        // If there is no time, add it to avoid delay in the conversion
        if (!timePattern.test(dateString)) {
          dateString += ' 00:00';
          parsedDate = new Date(dateString);
        }
        return parsedDate;
      }
    }

    return <Date>{};
  }
}
