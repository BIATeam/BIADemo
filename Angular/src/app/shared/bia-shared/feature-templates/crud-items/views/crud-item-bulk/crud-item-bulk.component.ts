import { Component, Injector } from '@angular/core';
import { take } from 'rxjs';
import {
  BulkData,
  CrudItemBulkService,
} from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudItemFormComponent } from '../../components/crud-item-form/crud-item-form.component';
import { CrudConfig } from '../../model/crud-config';
import { ActivatedRoute, Router } from '@angular/router';
import { CrudItemService } from '../../services/crud-item.service';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';

@Component({
  template: '',
})
export abstract class CrudItemBulkComponent<CrudItem extends BaseDto> {
  protected crudConfiguration: CrudConfig;
  protected bulkData: BulkData<CrudItem>;
  protected crudItemBulkService: CrudItemBulkService<CrudItem>;
  protected authService: AuthService;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected canEdit = true;
  protected canDelete = true;
  protected canAdd = true;

  constructor(
    protected injector: Injector,
    protected crudItemService: CrudItemService<CrudItem>
  ) {
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.crudItemBulkService = this.injector.get<CrudItemBulkService<CrudItem>>(
      CrudItemBulkService<CrudItem>
    );
    this.authService = this.injector.get<AuthService>(AuthService);
  }

  protected onFileSelected(event: any) {
    this.crudItemBulkService
      .uploadCsv(
        this.getForm(),
        event.files,
        this.crudConfiguration,
        this.crudItemService
      )
      .pipe(take(1)) // auto unsubscribe
      .subscribe((bulkData: BulkData<CrudItem>) => (this.bulkData = bulkData));
  }

  protected onCancel() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  protected onSave(toSaves: CrudItem[]) {
    if (toSaves.length > 0) {
      this.save(toSaves);
    }

    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  abstract setPermissions(): void;

  abstract save(toSaves: CrudItem[]): void;

  abstract getForm(): CrudItemFormComponent<CrudItem>;
}
