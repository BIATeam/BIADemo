import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import {
  BiaFieldConfig,
  BiaFieldNumberFormat,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaFieldHelperService } from './bia-field-helper.service';

@Component({
  selector: 'bia-field-base',
  template: '',
})
export class BiaFieldBaseComponent<CrudItem> implements OnInit, OnDestroy {
  @Input() field: BiaFieldConfig<CrudItem>;
  protected sub = new Subscription();

  constructor(
    public biaTranslationService: BiaTranslationService
    // protected authService: AuthService
  ) {}

  ngOnInit() {
    this.initFieldConfiguration();
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
    if (this.field.type === PropType.Number) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(culture => {
          if (culture) {
            if (this.field instanceof BiaFieldConfig) {
              const field = this.field.clone();
              field.displayFormat ||= new BiaFieldNumberFormat();
              if (field.displayFormat instanceof BiaFieldNumberFormat) {
                field.displayFormat.autoLocale = culture;
              }
              this.field = field;
            }
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
