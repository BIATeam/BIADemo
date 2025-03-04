import { Component, Injector } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudItemSingleService } from '../../services/crud-item-single.service';
import { CrudItemEditComponent } from '../crud-item-edit/crud-item-edit.component';

@Component({
  selector: 'bia-crud-item-read',
  templateUrl: './crud-item-read.component.html',
  styleUrls: ['./crud-item-read.component.scss'],
})
export class CrudItemReadComponent<
  CrudItem extends BaseDto,
> extends CrudItemEditComponent<CrudItem> {
  public canEdit: boolean;

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>,
    protected authService: AuthService
  ) {
    super(injector, crudItemService);
  }

  protected setPermissions(): void {
    this.canFix = false;
    this.canEdit = true;
  }
}
