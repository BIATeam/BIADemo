import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'sortByProperty' })
export class SortByPropertyPipe implements PipeTransform {
  transform<T>(items: T[], property: keyof T, ascending: boolean = true): T[] {
    if (!items || !property) return items || [];
    return [...items].sort((a, b) => {
      const valueA: number = Number(a[property]) ?? 0;
      const valueB: number = Number(b[property]) ?? 0;
      return ascending ? valueA - valueB : valueB - valueA;
    });
  }
}
