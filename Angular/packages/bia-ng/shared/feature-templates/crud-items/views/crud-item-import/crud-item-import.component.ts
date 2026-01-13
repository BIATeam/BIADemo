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
    CrudItem extends BaseDto<string | number>,
  >
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
