import {
  AfterViewInit,
  Component,
  Injector,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AuthService,
  BiaTranslationService,
  clone,
} from '@bia-team/bia-ng/core';
import { BaseDto, BiaFieldConfig } from '@bia-team/bia-ng/models';
import { PropType } from '@bia-team/bia-ng/models/enum';
import { Subscription, take } from 'rxjs';
import { BiaFieldHelperService } from '../../../../components/form/bia-field-base/bia-field-helper.service';
import { BiaFormComponent } from '../../../../components/form/bia-form/bia-form.component';
import { CrudConfig } from '../../model/crud-config';
import {
  CrudItemImportService,
  ImportData,
  ImportParam,
} from '../../services/crud-item-import.service';
import { CrudItemService } from '../../services/crud-item.service';

@Component({
  selector: 'bia-crud-item-import',
  template: '',
})
export abstract class CrudItemImportComponent<
  TListCrudItem extends BaseDto<string | number>,
  TFormCrudItem extends BaseDto<string | number> = TListCrudItem,
>
  implements OnInit, AfterViewInit, OnDestroy
{
  protected sub = new Subscription();
  protected crudConfiguration: CrudConfig<TFormCrudItem>;
  protected importData: ImportData<TFormCrudItem>;
  protected crudItemImportService: CrudItemImportService<
    TListCrudItem,
    TFormCrudItem
  >;
  protected authService: AuthService;
  protected biaTranslationService: BiaTranslationService;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected canEdit = true;
  protected canDelete = true;
  protected canAdd = true;

  @ViewChild(BiaFormComponent)
  biaFormComponent: BiaFormComponent<TFormCrudItem>;

  constructor(
    protected injector: Injector,
    protected crudItemService: CrudItemService<TListCrudItem, TFormCrudItem>
  ) {
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.crudItemImportService = this.injector.get<
      CrudItemImportService<TListCrudItem, TFormCrudItem>
    >(CrudItemImportService<TListCrudItem, TFormCrudItem>);
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

  protected addColumnId(crudConfig: CrudConfig<TFormCrudItem>) {
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

  protected getColumnId(): BiaFieldConfig<TFormCrudItem> {
    return Object.assign(new BiaFieldConfig<TFormCrudItem>('id', 'bia.id'), {
      isEditable: false,
      type: PropType.Number,
    });
  }

  protected onFileSelected(file: File) {
    this.crudItemImportService
      .uploadCsv(file)
      .pipe(take(1)) // auto unsubscribe
      .subscribe(
        (importData: ImportData<TFormCrudItem>) =>
          (this.importData = importData)
      );
  }

  protected onCancel() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  protected onSave(toSaves: TFormCrudItem[]) {
    if (toSaves.length > 0) {
      this.save(toSaves);
    }

    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  abstract setPermissions(): void;

  abstract save(toSaves: TFormCrudItem[]): void;

  protected getForm(): BiaFormComponent<TFormCrudItem> {
    return this.biaFormComponent;
  }

  protected onChangeImportParam(importParam: ImportParam) {
    this.crudItemImportService.importParam = importParam;
  }
}
