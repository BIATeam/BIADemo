import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import {
  BiaFieldConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';

@Component({
  selector: 'bia-field-base',
  template: '',
})
export class BiaFieldBaseComponent implements OnInit, OnDestroy {
  @Input() field: BiaFieldConfig;
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
    if (this.field.type == PropType.Number) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(culture => {
          if (culture != null) {
            const field = this.field.clone();
            field.culture = culture;
            this.field = field;
          }
        })
      );
    }
    if (
      this.field.type == PropType.DateTime ||
      this.field.type == PropType.Date ||
      this.field.type == PropType.Time ||
      this.field.type == PropType.TimeOnly ||
      this.field.type == PropType.TimeSecOnly
    ) {
      this.sub.add(
        this.biaTranslationService.currentCultureDateFormat$.subscribe(
          dateFormat => {
            const field = this.field.clone();
            switch (field.type) {
              case PropType.DateTime:
                field.primeDateFormat = dateFormat.primeDateFormat;
                field.hourFormat = dateFormat.hourFormat;
                field.formatDate = dateFormat.dateTimeFormat;
                break;
              case PropType.Date:
                field.primeDateFormat = dateFormat.primeDateFormat;
                field.formatDate = dateFormat.dateFormat;
                break;
              case PropType.Time:
                field.primeDateFormat = dateFormat.timeFormat;
                field.hourFormat = dateFormat.hourFormat;
                field.formatDate = dateFormat.timeFormat;
                break;
              case PropType.TimeOnly:
                field.primeDateFormat = dateFormat.timeFormat;
                field.hourFormat = dateFormat.hourFormat;
                field.formatDate = dateFormat.timeFormat;
                break;
              case PropType.TimeSecOnly:
                field.primeDateFormat = dateFormat.timeFormatSec;
                field.hourFormat = dateFormat.hourFormat;
                field.formatDate = dateFormat.timeFormatSec;
                break;
            }
            this.field = field;
          }
        )
      );
    }
  }
}
