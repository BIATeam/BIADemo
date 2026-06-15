import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { BiaTranslationService } from 'packages/bia-ng/core/public-api';
import { PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  BiaFieldConfig,
  BiaFieldNumberFormat,
} from 'packages/bia-ng/models/public-api';
import { Subscription, take } from 'rxjs';
import { BiaFieldHelperService } from './bia-field-helper.service';

@Component({
  selector: 'bia-field-base',
  template: '',
})
export class BiaFieldBaseComponent<CrudItem>
  implements OnInit, OnChanges, OnDestroy
{
  @Input() field: BiaFieldConfig<CrudItem>;
  protected sub = new Subscription();
  protected fieldSubscriptionsInitialized = false;

  constructor(public biaTranslationService: BiaTranslationService) {}

  ngOnInit() {
    this.initFieldConfiguration();
    this.initFieldSubscriptions();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.field && !changes.field.isFirstChange()) {
      this.initFieldConfiguration();
    }
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  protected getDateView(dateFormat: string) {
    const hasY = /[yY]/.test(dateFormat);
    const hasM = /[mM]/.test(dateFormat);
    const hasD = /[dD]/.test(dateFormat);
    if (hasY && !hasM && !hasD) {
      return 'year';
    } else if (hasM && !hasD) {
      return 'month';
    } else {
      return 'date';
    }
  }

  protected showSeconds(dateFormat: string) {
    return /[s]/.test(dateFormat);
  }

  protected initFieldConfiguration() {
    if (!(this.field instanceof BiaFieldConfig)) {
      return;
    }

    if (this.field.type === PropType.Number) {
      const field = this.field.clone();
      field.displayFormat ||= new BiaFieldNumberFormat();
      if (field.displayFormat instanceof BiaFieldNumberFormat) {
        field.displayFormat.autoLocale =
          this.biaTranslationService.currentCultureValue || '';
      }
      this.field = field;
    }

    if (
      this.field.type === PropType.DateTime ||
      this.field.type === PropType.Date ||
      this.field.type === PropType.Time ||
      this.field.type === PropType.TimeOnly ||
      this.field.type === PropType.TimeSecOnly
    ) {
      this.biaTranslationService.currentCultureDateFormat$
        .pipe(take(1))
        .subscribe(dateFormat => {
          if (this.field instanceof BiaFieldConfig) {
            const field = this.field.clone();
            BiaFieldHelperService.setDateFormat(field, dateFormat);
            this.field = field;
          }
        });
    }
  }

  protected initFieldSubscriptions() {
    if (this.fieldSubscriptionsInitialized) {
      return;
    }

    this.fieldSubscriptionsInitialized = true;

    if (this.field.type === PropType.Number) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(culture => {
          if (culture && this.field instanceof BiaFieldConfig) {
            const field = this.field.clone();
            field.displayFormat ||= new BiaFieldNumberFormat();
            if (field.displayFormat instanceof BiaFieldNumberFormat) {
              field.displayFormat.autoLocale = culture;
            }
            this.field = field;
          }
        })
      );
    }

    if (
      this.field.type === PropType.DateTime ||
      this.field.type === PropType.Date ||
      this.field.type === PropType.Time ||
      this.field.type === PropType.TimeOnly ||
      this.field.type === PropType.TimeSecOnly
    ) {
      this.sub.add(
        this.biaTranslationService.currentCultureDateFormat$.subscribe(
          dateFormat => {
            if (this.field instanceof BiaFieldConfig) {
              const field = this.field.clone();
              BiaFieldHelperService.setDateFormat(field, dateFormat);
              this.field = field;
            }
          }
        )
      );
    }
  }
}
