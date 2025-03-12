import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'join',
    standalone: false
})
export class JoinPipe implements PipeTransform {
  transform(input: Array<any>, sep = ','): string {
    return input.join(sep);
  }
}
