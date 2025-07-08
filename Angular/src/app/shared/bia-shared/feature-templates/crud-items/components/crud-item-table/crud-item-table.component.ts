import {
  AsyncPipe,
  NgClass,
  NgFor,
  NgIf,
  NgStyle,
  NgSwitch,
  NgTemplateOutlet,
} from '@angular/common';
import { Component, OnChanges } from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { Skeleton } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { BiaFrozenColumnDirective } from 'src/app/shared/bia-shared/components/table/bia-frozen-column/bia-frozen-column.directive';
import { PropType } from 'src/app/shared/bia-shared/model/bia-field-config';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { BiaTableFilterComponent } from '../../../../components/table/bia-table-filter/bia-table-filter.component';
import { BiaTableFooterControllerComponent } from '../../../../components/table/bia-table-footer-controller/bia-table-footer-controller.component';
import { BiaTableInputComponent } from '../../../../components/table/bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from '../../../../components/table/bia-table-output/bia-table-output.component';

@Component({
  selector: 'bia-crud-item-table',
  templateUrl:
    '../../../../components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../components/table/bia-calc-table/bia-calc-table.component.scss',
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    TableModule,
    PrimeTemplate,
    NgFor,
    Tooltip,
    NgSwitch,
    BiaTableFilterComponent,
    NgClass,
    BiaTableInputComponent,
    NgTemplateOutlet,
    BiaTableOutputComponent,
    Skeleton,
    NgStyle,
    BiaTableFooterControllerComponent,
    AsyncPipe,
    TranslateModule,
    BiaFrozenColumnDirective,
  ],
})
export class CrudItemTableComponent<CrudItem extends BaseDto>
  extends BiaCalcTableComponent<CrudItem>
  implements OnChanges
{
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
    if (this.configuration.formValidators) {
      this.form.addValidators(this.configuration.formValidators);
    }
  }

  protected formFields() {
    const fields: { [key: string]: any } = { id: [this.element.id] };
    for (const col of this.configuration.columns) {
      const validators: ValidatorFn[] = [];
      if (col.validators && col.validators.length > 0) {
        validators.push(...col.validators);
      }
      if (col.isRequired) {
        validators.push(Validators.required);
      }
      if (validators)
        fields[col.field.toString()] = [this.element[col.field], validators];
      else {
        fields[col.field.toString()] = [this.element[col.field]];
      }
    }
    return fields;
  }

  onSubmit() {
    if (this.form.valid) {
      const crudItem: CrudItem = <CrudItem>this.form.value;
      crudItem.id = crudItem.id ?? 0;
      for (const col of this.configuration.columns) {
        switch (col.type) {
          case PropType.Boolean:
            Reflect.set(
              crudItem,
              col.field,
              crudItem[col.field] ? crudItem[col.field] : false
            );
            break;
          case PropType.ManyToMany:
            Reflect.set(
              crudItem,
              col.field,
              BiaOptionService.differential(
                Reflect.get(crudItem, col.field) as BaseDto[],
                (this.element
                  ? (Reflect.get(this.element, col.field) ?? [])
                  : []) as BaseDto[]
              )
            );
            break;
          case PropType.OneToMany:
            if (
              col.isEditableChoice &&
              typeof crudItem[col.field as keyof CrudItem] === 'string'
            ) {
              Reflect.set(
                crudItem,
                col.field,
                new OptionDto(
                  0,
                  crudItem[col.field as keyof CrudItem] as string,
                  DtoState.AddedNewChoice
                )
              );
            } else {
              Reflect.set(
                crudItem,
                col.field,
                BiaOptionService.clone(crudItem[col.field as keyof CrudItem])
              );
            }
            break;
        }
      }

      this.save.emit(crudItem);
    }
  }
}
