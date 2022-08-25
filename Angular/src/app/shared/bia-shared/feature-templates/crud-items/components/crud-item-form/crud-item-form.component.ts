import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { PropType } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudConfig } from '../../model/crud-config';

@Component({
  selector: 'app-crud-item-form',
  templateUrl: './crud-item-form.component.html',
  styleUrls: ['./crud-item-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class CrudItemFormComponent<CrudItem extends BaseDto> implements OnInit, OnChanges {
  @Input() crudItem: CrudItem = <CrudItem>{};
  @Input() crudConfiguration : CrudConfig;
  @Input() dictOptionDtos: DictOptionDto[];

  @Output() save = new EventEmitter<CrudItem>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(
      public formBuilder: FormBuilder,
      // protected authService: AuthService
    ) {
    
  }

  ngOnInit() {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.crudItem && this.form) {
      this.form.reset();
      if (this.crudItem) {
        this.form.patchValue({ ...this.crudItem });
      }
    }
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos.filter((x) => x.key === key)[0]?.value;
  }

  protected initForm() {
    this.form = this.formBuilder.group(this.formFields());
  }
  protected formFields() {
      let fields : {[key:string]: any} = {id: [this.crudItem.id]};
      for (let col of this.crudConfiguration.columns) {
        if (col.isRequired)
        {
          fields[col.field] = [this.crudItem[col.field as keyof CrudItem], Validators.required];
        }
        else
        {
          fields[col.field] = [this.crudItem[col.field as keyof CrudItem]];
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
      const crudItem: CrudItem = <CrudItem>this.form.value;
      crudItem.id = crudItem.id > 0 ? crudItem.id : 0;
      for (let col of this.crudConfiguration.columns) {
        switch(col.type)
        {
          case PropType.Boolean:
            Reflect.set(crudItem, col.field, crudItem[col.field as keyof CrudItem] ? crudItem[col.field as keyof CrudItem] : false);
            break;
          case PropType.ManyToMany:
            Reflect.set(crudItem, col.field, BiaOptionService.Differential(
              Reflect.get(crudItem, col.field), 
              this.crudItem?Reflect.get(this.crudItem, col.field):undefined));
            break;
          case PropType.OneToMany:
            Reflect.set(crudItem, col.field, BiaOptionService.Clone(crudItem[col.field as keyof CrudItem]));
            break;
          }        
      }
      
      this.save.emit(crudItem);
      this.form.reset();
    }
  }
}

