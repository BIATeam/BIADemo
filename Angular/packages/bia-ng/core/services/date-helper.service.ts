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
          (data as any)[key] = value.toISOString();
        } else if (DateHelperService.isDate(value)) {
          (data as any)[key] = new Date(value);
        } else if (value instanceof Object === true) {
          this.fillDate(value);
        }
      });
    }
  }

  /**
   * Sérialise les dates en tenant compte des champs UTC picker.
   * Pour les champs UTC, utilise toISOStringFromUtcPickerDate
   * Pour les autres, utilise toISOString standard
   *
   * @param data L'objet à sérialiser
   * @param utcFields Liste des champs qui sont des UTC pickers
   */
  public static fillDateWithUtcFields<TOut>(
    data: TOut,
    utcFields: string[] = []
  ): void {
    if (!data) return;

    const utcFieldsSet = new Set(utcFields);

    Object.keys(data).forEach((key: string) => {
      const value = (data as any)[key];
      if (value instanceof Date === true) {
        if (utcFieldsSet.has(key)) {
          (data as any)[key] =
            DateHelperService.toISOStringFromUtcPickerDate(value);
        } else {
          (data as any)[key] = value.toISOString();
        }
      } else if (DateHelperService.isDate(value)) {
        (data as any)[key] = new Date(value);
      } else if (value instanceof Object === true) {
        this.fillDateWithUtcFields(value, utcFields);
      }
    });
  }

  public static fillDateISO<TOut>(data: TOut): TOut {
    if (!data) return data;

    Object.keys(data).forEach((key: string) => {
      const value = (data as any)[key];
      if (value instanceof Date) {
        (data as any)[key] = value.toISOString();
      }
    });

    return data;
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

  public static toUtcPickerDate(d: Date): Date {
    return new Date(
      d.getUTCFullYear(),
      d.getUTCMonth(),
      d.getUTCDate(),
      d.getUTCHours(),
      d.getUTCMinutes(),
      d.getUTCSeconds(),
      d.getUTCMilliseconds()
    );
  }

  /**
   * Reconvertit une date UTC picker vers UTC ISO string.
   * Une date UTC picker est une date locale qui représente visuellement l'heure UTC.
   * Exemple: Date(2024,0,15,14,30) représente 2024-01-15T14:30:00Z
   * Cette fonction retourne le string ISO correspondant.
   */
  public static toISOStringFromUtcPickerDate(d: Date): string {
    return new Date(
      Date.UTC(
        d.getFullYear(),
        d.getMonth(),
        d.getDate(),
        d.getHours(),
        d.getMinutes(),
        d.getSeconds(),
        d.getMilliseconds()
      )
    ).toISOString();
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
    if (dateFormat !== null) {
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
