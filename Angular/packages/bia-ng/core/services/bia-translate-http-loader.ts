import { HttpClient } from '@angular/common/http';
import { TranslateLoader, TranslationObject } from '@ngx-translate/core';

export class BiaTranslateHttpLoader implements TranslateLoader {
  constructor(
    private http: HttpClient,
    public prefix: string,
    public suffix: string = '.json'
  ) {}

  public getTranslation(lang: string) {
    return this.http.get<TranslationObject>(
      `${this.prefix}${lang}${this.suffix}`
    );
  }
}
