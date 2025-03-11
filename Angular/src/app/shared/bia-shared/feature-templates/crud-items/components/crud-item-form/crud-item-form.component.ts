import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { ValidatorFn } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { BiaFieldConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaFormLayoutConfig } from 'src/app/shared/bia-shared/model/bia-form-layout-config';
import { FormReadOnlyMode } from '../../model/crud-config';

@Component({
    selector: 'bia-crud-item-form',
    templateUrl: './crud-item-form.component.html',
    styleUrls: ['./crud-item-form.component.scss'],
    changeDetection: ChangeDetectionStrategy.Default,
    standalone: false
})
export class CrudItemFormComponent<CrudItem extends BaseDto> {
  @Input() crudItem?: CrudItem;
  @Input() fields: BiaFieldConfig<CrudItem>[];
  @Input() formLayoutConfig?: BiaFormLayoutConfig<CrudItem>;
  @Input() formValidators?: ValidatorFn[];
  @Input() formReadOnlyMode: FormReadOnlyMode = FormReadOnlyMode.off;
  @Input() dictOptionDtos: DictOptionDto[];
  @Input() isAdd?: boolean;
  @Input() isCrudItemOutdated = false;
  @Input() showSubmitButton = true;
  @Input() showFixableState?: boolean;
  @Input() canFix = false;

  @Output() save = new EventEmitter<CrudItem>();
  @Output() cancelled = new EventEmitter<void>();
  @Output() fixedChanged = new EventEmitter<boolean>();

  @ViewChild(BiaFormComponent) biaFormComponent: BiaFormComponent<CrudItem>;

  constructor(
    protected router: Router,
    protected activatedRoute: ActivatedRoute
  ) {}

  onCancel() {
    this.cancelled.next();
  }

  onSave(crudItem: CrudItem) {
    this.save.emit(crudItem);
  }

  onReadOnlyChanged(readOnly: boolean) {
    if (
      this.formReadOnlyMode === FormReadOnlyMode.clickToEdit &&
      readOnly === false
    ) {
      this.router.navigate(['../edit'], {
        relativeTo: this.activatedRoute,
      });
    }
  }

  onFixableStateChanged(fixed: boolean) {
    this.fixedChanged.emit(fixed);
  }
}
