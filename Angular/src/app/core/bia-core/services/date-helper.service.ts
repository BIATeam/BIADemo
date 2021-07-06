import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateHelperService {
  constructor() {}

  public static isDate(value: any): boolean {
    const regex = /(\d{4})-(\d{2})-(\d{2})T/;
    if (typeof value === 'string' && value.match(regex)) {
      const date = Date.parse(value);
      return !isNaN(date);
    }
    return false;
  }

  public static fillDate<TOut>(data: TOut) {
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
}
