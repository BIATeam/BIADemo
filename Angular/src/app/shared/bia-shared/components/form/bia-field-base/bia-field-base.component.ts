import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';

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
              if (
                !(field.displayFormat instanceof BiaFieldDateFormat) ||
                !field.customDisplayFormat
              ) {
                field.customDisplayFormat = false;
                field.displayFormat = new BiaFieldDateFormat();
                switch (field.type) {
                  case PropType.DateTime:
                    field.displayFormat.autoPrimeDateFormat =
                      dateFormat.primeDateFormat;
                    field.displayFormat.autoHourFormat = dateFormat.hourFormat;
                    field.displayFormat.autoFormatDate =
                      dateFormat.dateTimeFormat;
                    break;
                  case PropType.Date:
                    field.displayFormat.autoPrimeDateFormat =
                      dateFormat.primeDateFormat;
                    field.displayFormat.autoFormatDate = dateFormat.dateFormat;
                    break;
                  case PropType.Time:
                    field.displayFormat.autoPrimeDateFormat =
                      dateFormat.timeFormat;
                    field.displayFormat.autoHourFormat = dateFormat.hourFormat;
                    field.displayFormat.autoFormatDate = dateFormat.timeFormat;
                    break;
                  case PropType.TimeOnly:
                    field.displayFormat.autoPrimeDateFormat =
                      dateFormat.timeFormat;
                    field.displayFormat.autoHourFormat = dateFormat.hourFormat;
                    field.displayFormat.autoFormatDate = dateFormat.timeFormat;
                    break;
                  case PropType.TimeSecOnly:
                    field.displayFormat.autoPrimeDateFormat =
                      dateFormat.timeFormatSec;
                    field.displayFormat.autoHourFormat = dateFormat.hourFormat;
                    field.displayFormat.autoFormatDate =
                      dateFormat.timeFormatSec;
                    break;
                }
              }
              this.field = field;
            }
          }
        )
      );
    }
  }
}
