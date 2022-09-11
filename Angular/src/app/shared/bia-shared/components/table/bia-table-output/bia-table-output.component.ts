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
import { PrimeTemplate } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaFieldConfig, PropType} from 'src/app/shared/bia-shared/model/bia-field-config';

@Component({
  selector: 'bia-table-output',
  templateUrl: './bia-table-output.component.html',
  styleUrls: ['./bia-table-output.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class BiaTableOutputComponent implements OnInit, OnDestroy, AfterContentInit {
  @Input() field: BiaFieldConfig;
  @Input() data: any;

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;

  specificOutputTemplate: TemplateRef<any>;
  protected sub = new Subscription();
  
  constructor(
    public biaTranslationService: BiaTranslationService
    ) {
    
  }
  ngOnInit() {
    this.initFieldConfiguration();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
  ngAfterContentInit() {
    this.templates.forEach((item) => {
        switch(item.getType()) {
          case 'specificOutput':
            this.specificOutputTemplate = item.template;
          break;
        }
    });
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
