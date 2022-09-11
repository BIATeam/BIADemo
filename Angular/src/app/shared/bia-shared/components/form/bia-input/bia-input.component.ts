import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  Input,
  OnDestroy,
  OnInit,
  QueryList,
  TemplateRef
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaFieldConfig, PropType} from 'src/app/shared/bia-shared/model/bia-field-config';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';

@Component({
  selector: 'bia-input',
  templateUrl: './bia-input.component.html',
  styleUrls: ['./bia-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class BiaInputComponent implements OnInit, OnDestroy, AfterContentInit {
  @Input() field: BiaFieldConfig;
  @Input() form: FormGroup;
  @Input() dictOptionDtos: DictOptionDto[];

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  // specificInputTemplate: TemplateRef<any>;
  specificInputTemplate: TemplateRef<any>;
  protected sub = new Subscription();
  
  constructor(
    public biaTranslationService: BiaTranslationService
      // protected authService: AuthService
    ) {
    
  }
  ngOnInit() {
    this.initFieldConfiguration()
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
  ngAfterContentInit() {
    this.templates.forEach((item) => {
        switch(item.getType()) {
          /*case 'specificInput':
            this.specificInputTemplate = item.template;
          break;*/
          case 'specificInput':
            this.specificInputTemplate = item.template;
          break;
        }
    });
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos.filter((x) => x.key === key)[0]?.value;
  }

  private initFieldConfiguration() {
    if (
      this.field.type == PropType.DateTime
      ||
      this.field.type == PropType.Date
      ||
      this.field.type == PropType.Time
      ||
      this.field.type == PropType.TimeOnly
      ||
      this.field.type == PropType.TimeSecOnly
    )
    {
      this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
        let field = this.field.clone();
        switch (field.type)
        {
          case PropType.DateTime :
            field.primeDateFormat = dateFormat.primeDateFormat;
            field.hourFormat = dateFormat.hourFormat;
            break;
          case PropType.Date :
            field.primeDateFormat = dateFormat.primeDateFormat;
            break;
          case PropType.Time :
            field.primeDateFormat = dateFormat.timeFormat;
            field.hourFormat = dateFormat.hourFormat;
            break;
          case PropType.TimeOnly :
            field.primeDateFormat = dateFormat.timeFormat;
            field.hourFormat = dateFormat.hourFormat;
            break;
          case PropType.TimeSecOnly :
            field.primeDateFormat = dateFormat.timeFormatSec;
            field.hourFormat = dateFormat.hourFormat;
            break;
        }
        this.field = field;
      }));
    }

  }
}
