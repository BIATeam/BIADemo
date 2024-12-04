import {
  AfterViewInit,
  Component,
  Injector,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, take } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import {
  BulkData,
  BulkParam,
  CrudItemBulkService,
} from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { clone } from 'src/app/shared/bia-shared/utils';
import { CrudConfig } from '../../model/crud-config';
import { CrudItemService } from '../../services/crud-item.service';

@Component({
  template: '',
})
export abstract class CrudItemBulkComponent<CrudItem extends BaseDto>
  implements OnInit, AfterViewInit, OnDestroy
{
  protected sub = new Subscription();
  protected crudConfiguration: CrudConfig<CrudItem>;
  protected bulkData: BulkData<CrudItem>;
  protected crudItemBulkService: CrudItemBulkService<CrudItem>;
  protected authService: AuthService;
  protected biaTranslationService: BiaTranslationService;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected canEdit = true;
  protected canDelete = true;
  protected canAdd = true;

  @ViewChild(BiaFormComponent) biaFormComponent: BiaFormComponent<CrudItem>;

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
    this.biaTranslationService = this.injector.get<BiaTranslationService>(
      BiaTranslationService
    );
  }

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.crudItemService.optionsService.loadAllOptions(
          this.crudConfiguration.optionFilter
        );
      })
    );
  }

  ngAfterViewInit() {
    this.crudItemBulkService.init(
      this.getForm(),
      this.addColumnId(this.crudConfiguration),
      this.crudItemService
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  protected addColumnId(crudConfig: CrudConfig<CrudItem>) {
    const columnIdExists = crudConfig.fieldsConfig.columns.some(
      column => column.field === 'id'
    );

    if (columnIdExists !== true) {
      const crudConfigCopy = clone(crudConfig, false);
      crudConfigCopy.fieldsConfig.columns.unshift(this.getColumnId());
      return crudConfigCopy;
    } else {
      return crudConfig;
    }
  }

  protected getColumnId(): BiaFieldConfig<CrudItem> {
    return Object.assign(new BiaFieldConfig<CrudItem>('id', 'bia.id'), {
      isEditable: false,
      type: PropType.Number,
    });
  }

  protected onFileSelected(event: any) {
    this.crudItemBulkService
      .uploadCsv(event.files)
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

  protected getForm(): BiaFormComponent<CrudItem> {
    return this.biaFormComponent;
  }

  protected onChangeBulkParam(bulkParam: BulkParam) {
    this.crudItemBulkService.bulkParam = bulkParam;
  }
}
