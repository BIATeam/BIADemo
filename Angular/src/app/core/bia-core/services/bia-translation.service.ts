import { Inject, Injectable, LOCALE_ID } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNGConfig } from 'primeng/api';
// import * as deepmerge from 'deepmerge';
import { forkJoin, Observable, of, BehaviorSubject, combineLatest } from 'rxjs';
import { tap, map, distinctUntilChanged, skip } from 'rxjs/operators';
import { AppSettings } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import { getAppSettings } from 'src/app/domains/bia-domains/app-settings/store/app-settings.state';
import { AppState } from 'src/app/store/state';

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
  timeFormatSec: string
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
  public currentCulture$: Observable<string | null> = this.cultureSubject.asObservable();
  public appSettings$: Observable<AppSettings | null> = this.store.select(getAppSettings);
  public currentCultureDateFormat$: Observable<DateFormat> = combineLatest([this.currentCulture$, this.appSettings$])
    .pipe(map(([currentCulture, appSettings]) => this.getDateFormatByCulture(currentCulture, appSettings)));
  public languageId$: Observable<number> = combineLatest([this.currentCulture$, this.appSettings$])
    .pipe(map(([currentCulture, appSettings]) => this.getLanguageId(currentCulture, appSettings))).pipe(distinctUntilChanged()).pipe(skip(1));

  constructor(
    private translate: TranslateService,
    @Inject(LOCALE_ID) localeId: string,
    private store: Store<AppState>,
    private primeNgConfig: PrimeNGConfig
  ) {}

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
      this.cultureSubject.next(culture);
    });
    this.translate.get('primeng').subscribe(res => this.primeNgConfig.setTranslation(res));
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

  private getDateFormatByCulture(code: string | null, appSettings: AppSettings| null): DateFormat {
    let dateFormat = 'yyyy-MM-dd';
    let timeFormat = 'HH:mm';
    let timeFormatSec ="HH:mm:ss"
    if (appSettings != null) {
      let culture;

      if (code == null) {
        culture = appSettings.cultures.filter(c => c.acceptedCodes.indexOf('default') > -1)[0];
      }
      if (culture == null) {
        culture = appSettings.cultures.filter(c => c.code === code)[0];
      }

      if (culture) {
        dateFormat = culture.dateFormat;
        timeFormat = culture.timeFormat;
        timeFormatSec = culture.timeFormatSec;
      }
    }
    return { dateFormat: dateFormat, dateTimeFormat: `${dateFormat} ${timeFormat}`, timeFormat: timeFormat, timeFormatSec: timeFormatSec };
  }
  private getLanguageId(code: string | null, appSettings: AppSettings| null): number {
    let LanguageId = 0;
    if (appSettings != null) {
      let culture;

      if (code == null) {
        culture = appSettings.cultures.filter(c => c.acceptedCodes.indexOf('default') > -1)[0];
      }

      if (culture == null) {
        culture = appSettings.cultures.filter(c => c.code === code)[0];
      }

      if (culture) {
        LanguageId = culture.languageId;
      }
    }
    return LanguageId;
  }
}
