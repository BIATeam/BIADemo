import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  TemplateRef
} from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaFieldConfig, PropType} from 'src/app/shared/bia-shared/model/bia-field-config';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';

@Component({
  selector: 'bia-table-input',
  templateUrl: './bia-table-input.component.html',
  styleUrls: ['./bia-table-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class BiaTableInputComponent implements OnInit, OnDestroy, AfterContentInit {
  @Input() field: BiaFieldConfig;
  @Input() form: UntypedFormGroup;
  @Input() dictOptionDtos: DictOptionDto[];

  @Output() valueChange = new EventEmitter<void>();
  @Output() complexInput = new EventEmitter<boolean>();

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
          /*case 'specificInput':
            this.specificInputTemplate = item.template;
          break;*/
          case 'specificInput':
            this.specificInputTemplate = item.template;
          break;
        }
    });
  }

  public onChange() {
    this.valueChange.emit();
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos.filter((x) => x.key === key)[0]?.value;
  }

  public onComplexInput(isIn : boolean) {
    this.complexInput.emit(isIn);
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
