import { Component, Injector } from '@angular/core';
import { take } from 'rxjs';
import {
  BulkSaveData,
  CrudItemBulkSaveService,
} from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk-save.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudItemFormComponent } from '../../components/crud-item-form/crud-item-form.component';
import { CrudConfig } from '../../model/crud-config';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  template: '',
})
export abstract class CrudItemBulkComponent<CrudItem extends BaseDto> {
  protected crudConfiguration: CrudConfig;
  protected bulkSaveData: BulkSaveData<CrudItem>;

  protected router: Router;
  protected activatedRoute: ActivatedRoute;

  constructor(
    protected injector: Injector,
    protected crudItemBulkSaveService: CrudItemBulkSaveService<CrudItem>
  ) {
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
  }

  protected onFileSelected(event: any) {
    this.crudItemBulkSaveService
      .uploadCsv(this.getForm(), event.files, this.crudConfiguration)
      .pipe(take(1)) // auto unsubscribe
      .subscribe(
        (bulkSaveData: BulkSaveData<CrudItem>) =>
          (this.bulkSaveData = bulkSaveData)
      );
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

  abstract save(toSaves: CrudItem[]): void;

  abstract getForm(): CrudItemFormComponent<CrudItem>;
}
