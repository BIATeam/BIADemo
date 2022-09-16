import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  SimpleChanges,
  TemplateRef
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { BiaFieldConfig, PropType } from 'src/app/shared/bia-shared/model/bia-field-config';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';

@Component({
  selector: 'bia-form',
  templateUrl: './bia-form.component.html',
  styleUrls: ['./bia-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class BiaFormComponent implements OnInit, OnDestroy, OnChanges, AfterContentInit {
  @Input() element: any = {};
  @Input() fields: BiaFieldConfig[];
  @Input() dictOptionDtos: DictOptionDto[];
  @Output() save = new EventEmitter<any>();
  @Output() cancel = new EventEmitter();

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  // specificInputTemplate: TemplateRef<any>;
  specificInputTemplate: TemplateRef<any>;
  form: FormGroup;
  protected sub = new Subscription();

  constructor(
      public formBuilder: FormBuilder,
      // protected authService: AuthService
    ) {
    
  }

  ngOnInit() {
    this.initForm();
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

  ngOnChanges(changes: SimpleChanges) {
    if (this.element && this.form) {
      this.form.reset();
      if (this.element) {
        this.form.patchValue({ ...this.element });
      }
    }
  }

  protected initForm() {
    this.form = this.formBuilder.group(this.formFields());
  }
  protected formFields() {
      let fields : {[key:string]: any} = {id: [this.element.id]};
      for (let col of this.fields) {
        if (col.isRequired)
        {
          fields[col.field] = [this.element[col.field], Validators.required];
        }
        else
        {
          fields[col.field] = [this.element[col.field]];
        }
      }
      return fields;
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const element: any = this.form.value;
      element.id = element.id > 0 ? element.id : 0;
      for (let col of this.fields) {
        switch(col.type)
        {
          case PropType.Boolean:
            Reflect.set(element, col.field, element[col.field] ? element[col.field] : false);
            break;
          case PropType.ManyToMany:
            Reflect.set(element, col.field, BiaOptionService.Differential(
              Reflect.get(element, col.field), 
              this.element?Reflect.get(this.element, col.field):undefined));
            break;
          case PropType.OneToMany:
            Reflect.set(element, col.field, BiaOptionService.Clone(element[col.field]));
            break;
          }        
      }
      
      this.save.emit(element);
      this.form.reset();
    }
  }
  getCellData(field: any): any {
    const nestedProperties: string[] = field.field.split('.');
    let value: any = this.element;
    for (const prop of nestedProperties) {
      if(value == null)
      {
        return null;
      }
      
      value = value[prop];
    }
 
    return value;
  }
}

