import { Inject, Injectable, LOCALE_ID } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
// import * as deepmerge from 'deepmerge';
import { forkJoin, Observable, of, BehaviorSubject } from 'rxjs';
import { tap, map } from 'rxjs/operators';

// export const BIA_DEFAULT_LOCALE_ID = new InjectionToken('biaDefaultLocaleId');
const TRANSLATION_LANG_KEY = '@@lang';
export const STORAGE_LANG_KEY = 'lang';

// Return last choosed lang or browser lang.
export const getInitialLang = (supportedLangs: string[]) => {
  let lang;
  try {
    lang = localStorage.getItem(STORAGE_LANG_KEY);
  } catch {}
  if (!lang) {
    lang = navigator.language;
  }
  if (supportedLangs.indexOf(lang) !== -1) {
    return lang;
  }
  return 'en-US';
};

export interface DateFormat {
  dateFormat: string;
  dateTimeFormat: string;
  timeFormat: string;
}

// Same as @ngx-translate/http-loader but keep the previous translations (usefull for lazy loading of translation)

// Workaround for https://github.com/ngx-translate/core/issues/425
// Also force ngx-translate to load translations
@Injectable({
  providedIn: 'root'
})
export class BiaTranslationService {
  private translationsLoaded: { [lang: string]: boolean } = {};
  private lazyTranslateServices: TranslateService[] = [];
  private cultureSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(this.getLangSelected());
  public culture$: Observable<DateFormat> = this.cultureSubject
    .asObservable()
    .pipe(map((x) => this.getDateFormatByCulture(x)));

  constructor(private translate: TranslateService, @Inject(LOCALE_ID) localeId: string) {}

  getLangSelected(): string | null {
    return localStorage.getItem(STORAGE_LANG_KEY);
  }

  // Translation for bia are not loaded with http client (arguably unnecessary)
  registerLocaleData(data: { [k: string]: any }) {
    if (!data[TRANSLATION_LANG_KEY]) {
      throw new Error('invalid translation file');
    }
    this.translate.setTranslation(data[TRANSLATION_LANG_KEY], data, true);
  }

  // Because we add some translations (registerLocaleData), ngx-translate doesn't modules translations
  // So we need to call getTranslation manually
  // NOTE: Check if it's still usefull
  loadAndChangeLanguage(lang: string, defaultLang?: string) {
    const culture = lang;
    this.cultureSubject.next(culture);
    lang = lang.split('-')[0];
    const translationLoaders$ = [];
    const translateServices = [this.translate, ...this.lazyTranslateServices];
    if (!this.translationsLoaded[lang]) {
      for (const translateService of translateServices) {
        translationLoaders$.push(translateService.getTranslation(lang));
      }
    }
    if (defaultLang && defaultLang !== lang && !this.translationsLoaded[defaultLang]) {
      for (const translateService2 of translateServices) {
        translationLoaders$.push(translateService2.getTranslation(defaultLang));
      }
    }
    let lang$: Observable<any> = of(undefined);
    if (translationLoaders$.length) {
      lang$ = this.loadTranslations(translationLoaders$, lang, defaultLang);
    }
    lang$.subscribe(() => {
      this.translate.use(lang);
      if (defaultLang) {
        this.translate.setDefaultLang(defaultLang);
      }
      try {
        localStorage.setItem(STORAGE_LANG_KEY, culture);
      } catch {}
    });
  }

  registerLazyTranslateService(translateService: TranslateService) {
    if (this.translate.defaultLang !== this.translate.currentLang) {
      translateService.getTranslation(this.translate.defaultLang);
    }
    translateService.getTranslation(this.translate.currentLang);
    this.lazyTranslateServices.push(translateService);
  }

  private loadTranslations(translationLoaders$: Observable<any>[], lang: string, defaultLang?: string) {
    return forkJoin(translationLoaders$).pipe(
      tap(() => {
        this.translationsLoaded[lang] = true;
        if (defaultLang) {
          this.translationsLoaded[defaultLang] = true;
        }
      })
    );
  }

  private getDateFormatByCulture(culture: string | null): DateFormat {
    let dateFormat = '';
    let timeFormat = '';
    switch (culture) {
      case 'de-DE':
        dateFormat = 'dd.MM.yyyy';
        timeFormat = 'HH:mm';
        break;
      case 'es-ES':
        dateFormat = 'dd/MM/yyyy';
        timeFormat = 'H:mm';
        break;
      case 'fr-FR':
        dateFormat = 'dd/MM/yyyy';
        timeFormat = 'HH:mm';
        break;
      case 'en-GB':
        dateFormat = 'dd/MM/yyyy';
        timeFormat = 'HH:mm';
        break;
      case 'es-MX':
        dateFormat = 'dd/MM/yyyy';
        timeFormat = 'hh:mm a';
        break;
      case 'en-US':
        dateFormat = 'MM/dd/yyyy';
        timeFormat = 'h:mm a';
        break;
      default:
        dateFormat = 'yyyy-MM-dd';
        timeFormat = 'HH:mm';
        break;
    }
    return { dateFormat: dateFormat, dateTimeFormat: `${dateFormat} ${timeFormat}`, timeFormat: timeFormat };
  }
}
