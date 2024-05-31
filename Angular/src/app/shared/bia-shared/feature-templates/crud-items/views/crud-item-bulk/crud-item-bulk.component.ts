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

@Component({
  template: '',
})
export abstract class CrudItemBulkComponent<CrudItem extends BaseDto> {
  protected crudConfiguration: CrudConfig;
  protected bulkData: BulkData<CrudItem>;

  protected router: Router;
  protected activatedRoute: ActivatedRoute;

  constructor(
    protected injector: Injector,
    protected crudItemBulkService: CrudItemBulkService<CrudItem>
  ) {
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
  }

  protected onFileSelected(event: any) {
    this.crudItemBulkService
      .uploadCsv(this.getForm(), event.files, this.crudConfiguration)
      .pipe(take(1)) // auto unsubscribe
      .subscribe(
        (bulkData: BulkData<CrudItem>) =>
          (this.bulkData = bulkData)
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
