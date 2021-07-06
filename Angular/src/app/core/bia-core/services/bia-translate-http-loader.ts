import { HttpClient } from '@angular/common/http';
import { TranslateLoader,  TranslateStore } from '@ngx-translate/core';
import * as deepmerge from 'deepmerge';
import { map } from 'rxjs/operators';

export class BiaTranslateHttpLoader implements TranslateLoader {
  constructor(
    private http: HttpClient,
    private store: TranslateStore,
    public prefix: string,
    public suffix: string = '.json'
  ) {}

  public getTranslation(lang: string) {
    return this.http.get(`${this.prefix}${lang}${this.suffix}`).pipe(
      map((translation) => {
        const previousTranslations = this.store.translations[lang];
        if (previousTranslations) {
          return deepmerge.default(previousTranslations, translation);
        }
        return translation;
      })
    );
  }
}
