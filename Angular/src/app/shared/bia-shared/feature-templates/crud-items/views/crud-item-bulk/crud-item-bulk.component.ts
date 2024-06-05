import { Component, Injector, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription, take } from 'rxjs';
import {
  BulkData,
  CrudItemBulkService,
} from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudConfig } from '../../model/crud-config';
import { ActivatedRoute, Router } from '@angular/router';
import { CrudItemService } from '../../services/crud-item.service';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';

@Component({
  template: '', // le template pour ce composant abstrait doit être vide
})
export abstract class CrudItemBulkComponent<CrudItem extends BaseDto> implements OnInit, OnDestroy {
  protected sub = new Subscription();
  protected crudConfiguration: CrudConfig;
  protected bulkData: BulkData<CrudItem>;
  protected crudItemBulkService: CrudItemBulkService<CrudItem>;
  protected authService: AuthService;
  protected biaTranslationService: BiaTranslationService;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected canEdit = true;
  protected canDelete = true;
  protected canAdd = true;
  @ViewChild(BiaFormComponent) biaFormComponent: BiaFormComponent;

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
    this.biaTranslationService = this.injector.get<BiaTranslationService>(BiaTranslationService);
  }

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
          this.crudItemService.optionsService.loadAllOptions(this.crudConfiguration.optionFilter);
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
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

  protected getForm(): BiaFormComponent {
    return this.biaFormComponent;
  }
}
