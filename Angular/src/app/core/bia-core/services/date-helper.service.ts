import { Injectable } from '@angular/core';

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

  public static parseDate(dateString: string): Date | null {
    if (isNaN(Date.parse(dateString)) !== true) {
      return new Date(dateString);
    }

    const formats = [
      // 'DD/MM/YYYY HH:mm'
      {
        regex: /^\d{2}\/\d{2}\/\d{4}\s\d{2}:\d{2}$/,
        fn: (d: string) => {
          const [date, time] = d.split(' ');
          const [day, month, year] = date.split('/');
          const [hours, minutes] = time.split(':');
          return new Date(`${year}-${month}-${day}T${hours}:${minutes}`);
        },
      },

      // 'DD/MM/YYYY'
      {
        regex: /^\d{2}\/\d{2}\/\d{4}$/,
        fn: (d: string) => {
          const [day, month, year] = d.split('/');
          return new Date(`${year}-${month}-${day}`);
        },
      },
    ];

    for (let i = 0; i < formats.length; i++) {
      if (formats[i].regex.test(dateString)) {
        return formats[i].fn(dateString);
      }
    }

    return null;
  }
}
