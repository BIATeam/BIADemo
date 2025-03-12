import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Actions } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { Subscription, filter, first } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { AppState } from 'src/app/store/state';
import { CrudConfig } from '../../model/crud-config';
import { CrudItemSingleService } from '../../services/crud-item-single.service';

@Component({
    selector: 'bia-crud-item-new',
    templateUrl: './crud-item-new.component.html',
    styleUrls: ['./crud-item-new.component.scss'],
    standalone: false
})
export class CrudItemNewComponent<CrudItem extends BaseDto>
  implements OnInit, OnDestroy
{
  protected sub = new Subscription();
  public crudConfiguration: CrudConfig<CrudItem>;

  protected store: Store<AppState>;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected biaTranslationService: BiaTranslationService;
  protected actions: Actions;
  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>
  ) {
    this.store = this.injector.get<Store<AppState>>(Store);
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.biaTranslationService = this.injector.get<BiaTranslationService>(
      BiaTranslationService
    );
    this.actions = this.injector.get<Actions>(Actions);
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

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(crudItemToCreate: CrudItem) {
    const successActionType = this.crudItemService.createSuccessActionType;

    if (successActionType) {
      this.actions
        .pipe(
          filter((action: any) => action.type === successActionType),
          first()
        )
        .subscribe(() => {
          this.navigateBack();
        });
    }

    this.crudItemService.create(crudItemToCreate);

    if (!successActionType) {
      this.navigateBack();
    }
  }

  onCancelled() {
    this.navigateBack();
  }

  private navigateBack() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
