import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';

@Pipe({
  name: 'translateField',
})
export class TranslateFieldPipe extends TranslatePipe implements PipeTransform {

  transform(input: any, key: string, translateKey: string ): any {
    return input !== undefined ? (translateKey ? super.transform(translateKey + input[key]) : input[key]) : '';
  }
}
