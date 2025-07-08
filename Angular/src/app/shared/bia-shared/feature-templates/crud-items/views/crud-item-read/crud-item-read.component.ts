import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { CrudItemFormComponent } from '../../components/crud-item-form/crud-item-form.component';
import { FormReadOnlyMode } from '../../model/crud-config';
import { CrudItemSingleService } from '../../services/crud-item-single.service';
import { CrudItemEditComponent } from '../crud-item-edit/crud-item-edit.component';

@Component({
  selector: 'bia-crud-item-read',
  templateUrl: './crud-item-read.component.html',
  styleUrls: ['./crud-item-read.component.scss'],
  imports: [NgIf, CrudItemFormComponent, SpinnerComponent, AsyncPipe],
})
export class CrudItemReadComponent<CrudItem extends BaseDto>
  extends CrudItemEditComponent<CrudItem>
  implements OnInit
{
  public canEdit: boolean;

  protected set formReadOnlyMode(value: FormReadOnlyMode) {
    super.formReadOnlyMode = value;
    this.onFormReadOnlySet(value);
  }

  public get formReadOnlyMode() {
    return super.formReadOnlyMode;
  }

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>
  ) {
    super(injector, crudItemService);
  }

  protected setPermissions(): void {
    super.setPermissions();
    this.canEdit = true;
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

  protected onFormReadOnlySet(value: FormReadOnlyMode): void {
    if (this.canEdit === undefined) {
      return;
    }

    if (value === FormReadOnlyMode.off) {
      if (!this.canEdit) {
        this.formReadOnlyMode = FormReadOnlyMode.on;
        return;
      }

      this.router.navigate(['../edit'], {
        relativeTo: this.activatedRoute,
      });
    }
  }
}
