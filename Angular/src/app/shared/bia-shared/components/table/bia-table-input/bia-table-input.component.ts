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
import { FormGroup } from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { PrimeTableColumn, PropType} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';

@Component({
  selector: 'bia-table-input',
  templateUrl: './bia-table-input.component.html',
  styleUrls: ['./bia-table-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class BiaTableInputComponent implements OnInit, OnDestroy, AfterContentInit {
  @Input() field: PrimeTableColumn;
  @Input() form: FormGroup;
  @Input() dictOptionDtos: DictOptionDto[];

  @Output() valueChange = new EventEmitter();
  @Output() complexInput = new EventEmitter<boolean>();
  
  protected currentRow: HTMLElement;
  protected mandatoryFields: string[] = [];

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

  public onChange() {
    this.valueChange.emit();
  }

  protected fillMandatoryFields() {
    Object.keys(this.form.controls).forEach(key => {
      if (this.form.controls[key]?.validator?.name === 'required') {
        this.mandatoryFields.push(key);
      }
    });
  }

  public onShowCalendar() {
    this.currentRow = this.getParentComponent(document.activeElement, 'p-selectable-row') as HTMLElement;
  }

  public onBlurCalendar() {
    this.currentRow?.focus();
  }

  public getParentComponent(el: Element | null, parentClassName: string): HTMLElement | null {
    if (el) {
      while (el.parentElement) {
        if (el.parentElement.classList.contains(parentClassName)) {
          return el.parentElement;
        } else {
          el = el.parentElement;
        }
      }
    }
    return null;
  }

  public onCloseCalendar() {
    this.currentRow?.focus();
  }

  public isRequired(field: string): boolean {
    return this.mandatoryFields.includes(field);
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
