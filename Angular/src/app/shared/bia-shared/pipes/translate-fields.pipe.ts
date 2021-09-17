import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';

@Pipe({
  name: 'translateFields',
})
export class TranslateFieldsPipe extends TranslatePipe implements PipeTransform {
  transform(input: any, key: string, translateKey: string ): any {
    return input!= undefined? ((input as any[]).map(value =>  translateKey?  super.transform(translateKey + value[key]) : value[key]).join(", ")) : '';
  }
}
