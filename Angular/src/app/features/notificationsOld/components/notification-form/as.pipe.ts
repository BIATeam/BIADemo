import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'as',
  pure: true,
})
export class AsPipe implements PipeTransform {

  transform<T>(value: any, clss: new (...args: any[]) => T): T {
    return value as T;
  }

}
