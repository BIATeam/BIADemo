import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';

@Pipe({ name: 'translateField' })
export class TranslateFieldPipe extends TranslatePipe implements PipeTransform {
  transform(
    input: any,
    key: string,
    translationKey: string,
    languageId: string
  ): string {
    return input !== undefined && input !== null
      ? translationKey
        ? this.translateValue(input, key, translationKey, languageId)
        : input[key]
      : '';
  }

  protected translateValue(
    input: any,
    key: string,
    translationkey: string,
    languageId: string
  ): string {
    const translation: any = (input[translationkey] as any[]).filter(
      t => t.languageId === languageId
    )[0];
    if (translation === null || translation === undefined) {
      return input[key];
    }
    return translation[key];
  }
}
