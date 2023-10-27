import { Component, OnChanges } from '@angular/core';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { PropType } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

@Component({
  selector: 'bia-crud-item-table',
  templateUrl: '../../../../components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: ['../../../../components/table/bia-calc-table/bia-calc-table.component.scss']
})
export class CrudItemTableComponent<CrudItem extends BaseDto> extends BiaCalcTableComponent implements OnChanges {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }

  public initForm() {
    this.form = this.formBuilder.group(this.formFields());
  }
  protected formFields() {
      let fields : {[key:string]: any} = {id: [this.element.id]};
      for (let col of this.configuration.columns) {
        if (col.validators && col.validators.length > 0) {
          fields[col.field] = [this.element[col.field as keyof CrudItem], col.validators];
        } else if (col.isRequired) {
          fields[col.field] = [this.element[col.field as keyof CrudItem], Validators.required];
        } else {
          fields[col.field] = [this.element[col.field as keyof CrudItem]];
        }
      }
      return fields;
  }

  onSubmit() {
    if (this.form.valid) {
      const crudItem: CrudItem = <CrudItem>this.form.value;
      crudItem.id = crudItem.id > 0 ? crudItem.id : 0;
      for (let col of this.configuration.columns) {
        switch(col.type)
        {
          case PropType.Boolean:
            Reflect.set(crudItem, col.field, crudItem[col.field as keyof CrudItem] ? crudItem[col.field as keyof CrudItem] : false);
            break;
          case PropType.ManyToMany:
            Reflect.set(crudItem, col.field, BiaOptionService.Differential(
              Reflect.get(crudItem, col.field) as BaseDto [], 
              this.element?Reflect.get(this.element, col.field):undefined));
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
