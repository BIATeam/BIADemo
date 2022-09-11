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
import { PrimeTableColumn, PropType} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';

@Component({
  selector: 'bia-input',
  templateUrl: './bia-input.component.html',
  styleUrls: ['./bia-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class BiaInputComponent implements OnInit, OnDestroy, AfterContentInit {
  @Input() field: PrimeTableColumn;
  @Input() form: FormGroup;
  @Input() dictOptionDtos: DictOptionDto[];

  date8: Date = new Date(2022,12,24,15,15);

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
        //this.fields = this.crudConfiguration.columns.map<PrimeTableColumn>(object => object.clone())
          switch (this.field.type)
          {
            case PropType.DateTime :
              this.field.primeDateFormat = dateFormat.primeDateFormat;
              this.field.hourFormat = dateFormat.hourFormat;
              break;
            case PropType.Date :
              this.field.primeDateFormat = dateFormat.primeDateFormat;
              break;
            case PropType.Time :
              this.field.primeDateFormat = dateFormat.timeFormat;
              this.field.hourFormat = dateFormat.hourFormat;
              break;
            case PropType.TimeOnly :
              this.field.primeDateFormat = dateFormat.timeFormat;
              this.field.hourFormat = dateFormat.hourFormat;
              break;
            case PropType.TimeSecOnly :
              //this.field.primeDateFormat = dateFormat.timeFormatSec;
              this.field.hourFormat = dateFormat.hourFormat;
              break;
          }
      }));
    }

  }
}
