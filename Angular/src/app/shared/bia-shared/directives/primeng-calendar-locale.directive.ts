import { Directive, OnDestroy } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { PrimeLocale } from '../model/prime-locale';
import { TranslateService } from '@ngx-translate/core';
import { map } from 'rxjs/operators';
import { Calendar } from 'primeng';

@Directive({
  selector: '[biaLocale]'
})
export class PrimengCalendarLocaleDirective implements OnDestroy {
  locale$: Observable<PrimeLocale>;
  subscription: Subscription;

  constructor(pCalendar: Calendar, private translate: TranslateService) {
    this.locale$ = this.getPrimeLocale();
    this.subscription = this.locale$.subscribe((primeLocale: PrimeLocale) => (pCalendar.locale = primeLocale));
  }

  private getPrimeLocale(): Observable<PrimeLocale> {
    const translationKeys = [
      'bia.locale.dayNames',
      'bia.locale.dayNamesShort',
      'bia.locale.dayNamesMin',
      'bia.locale.monthNames',
      'bia.locale.monthNamesShort',
      'bia.locale.today',
      'bia.locale.clear'
    ];

    return this.translate.stream(translationKeys).pipe(
      map((translations) => {
        const locale: PrimeLocale = {
          firstDayOfWeek: 0,
          dayNames: [],
          dayNamesShort: [],
          dayNamesMin: [],
          monthNames: [],
          monthNamesShort: [],
          today: '',
          clear: '',
          dateFormat: 'dd/mm/yy',
          weekHeader: 'wk'
        };

        locale.dayNames = translations['bia.locale.dayNames'].split(',');
        locale.dayNamesShort = translations['bia.locale.dayNamesShort'].split(',');
        locale.dayNamesMin = translations['bia.locale.dayNamesMin'].split(',');
        locale.monthNames = translations['bia.locale.monthNames'].split(',');
        locale.monthNamesShort = translations['bia.locale.monthNamesShort'].split(',');
        locale.today = translations['bia.locale.today'];
        locale.clear = translations['bia.locale.clear'];

        return locale;
      })
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
