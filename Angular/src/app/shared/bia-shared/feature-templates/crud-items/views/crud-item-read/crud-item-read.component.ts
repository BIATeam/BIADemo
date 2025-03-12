import { Component, Injector, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { FormReadOnlyMode } from '../../model/crud-config';
import { CrudItemSingleService } from '../../services/crud-item-single.service';
import { CrudItemEditComponent } from '../crud-item-edit/crud-item-edit.component';
import { NgIf, AsyncPipe } from '@angular/common';
import { CrudItemFormComponent } from '../../components/crud-item-form/crud-item-form.component';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';

@Component({
    selector: 'bia-crud-item-read',
    templateUrl: './crud-item-read.component.html',
    styleUrls: ['./crud-item-read.component.scss'],
    imports: [NgIf, CrudItemFormComponent, SpinnerComponent, AsyncPipe]
})
export class CrudItemReadComponent<CrudItem extends BaseDto>
  extends CrudItemEditComponent<CrudItem>
  implements OnInit
{
  public canEdit: boolean;
  protected initialFormReadOnlyMode: FormReadOnlyMode;

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>,
    protected authService: AuthService
  ) {
    super(injector, crudItemService);
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.initialFormReadOnlyMode = this.formReadOnlyMode;
  }

  protected setPermissions(): void {
    super.setPermissions();
    this.canEdit = true;
  }
}
