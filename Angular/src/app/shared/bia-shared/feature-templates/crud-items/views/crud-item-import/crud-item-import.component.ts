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
import { BiaFieldHelperService } from 'src/app/shared/bia-shared/components/form/bia-field-base/bia-field-helper.service';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import {
  CrudItemImportService,
  ImportData,
  ImportParam,
} from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-import.service';
import {
  BiaFieldConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { clone } from 'src/app/shared/bia-shared/utils';
import { CrudConfig } from '../../model/crud-config';
import { CrudItemService } from '../../services/crud-item.service';

@Component({
  template: '',
})
export abstract class CrudItemImportComponent<CrudItem extends BaseDto>
  implements OnInit, AfterViewInit, OnDestroy
{
  protected sub = new Subscription();
  protected crudConfiguration: CrudConfig<CrudItem>;
  protected importData: ImportData<CrudItem>;
  protected crudItemImportService: CrudItemImportService<CrudItem>;
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
    this.crudItemImportService = this.injector.get<
      CrudItemImportService<CrudItem>
    >(CrudItemImportService<CrudItem>);
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
    this.crudItemImportService.init(
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
      crudConfigCopy.fieldsConfig.columns = crudConfig.fieldsConfig.columns.map(
        c => {
          const field = c.clone();
          this.sub.add(
            this.biaTranslationService.currentCultureDateFormat$
              .pipe(take(1))
              .subscribe(dateFormat => {
                if (field instanceof BiaFieldConfig) {
                  BiaFieldHelperService.setDateFormat(field, dateFormat);
                }
              })
          );
          return field;
        }
      );
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

  protected onFileSelected(file: File) {
    this.crudItemImportService
      .uploadCsv(file)
      .pipe(take(1)) // auto unsubscribe
      .subscribe(
        (importData: ImportData<CrudItem>) => (this.importData = importData)
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

  abstract setPermissions(): void;

  abstract save(toSaves: CrudItem[]): void;

  protected getForm(): BiaFormComponent<CrudItem> {
    return this.biaFormComponent;
  }

  protected onChangeImportParam(importParam: ImportParam) {
    this.crudItemImportService.importParam = importParam;
  }
}
