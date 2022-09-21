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
        let field = this.field.clone();
        switch (field.type)
        {
          case PropType.DateTime :
            field.formatDate = dateFormat.dateTimeFormat;
            break;
          case PropType.Date :
            field.formatDate = dateFormat.dateFormat;
            break;
          case PropType.Time :
            field.formatDate = dateFormat.timeFormat;
            break;
          case PropType.TimeOnly :
            field.formatDate = dateFormat.timeFormat;
            break;
          case PropType.TimeSecOnly :
            field.formatDate = dateFormat.timeFormatSec;
            break;
        }
        this.field = field;
      }));
    }
  }
}
