import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Actions } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { filter, first, Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { AppState } from 'src/app/store/state';
import { CrudConfig } from '../../model/crud-config';
import { CrudItemSingleService } from '../../services/crud-item-single.service';

@Component({
  selector: 'bia-crud-item-new',
  templateUrl: './crud-item-new.component.html',
  styleUrls: ['./crud-item-new.component.scss'],
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
    if (this.crudItemService.createSuccessActionType) {
      this.actions
        .pipe(
          filter(
            (action: any) =>
              action.type === this.crudItemService.createSuccessActionType
          ),
          first()
        )
        .subscribe(() => {
          this.router.navigate(['../'], { relativeTo: this.activatedRoute });
        });
    }

    this.crudItemService.create(crudItemToCreate);

    if (!this.crudItemService.createSuccessActionType) {
      this.router.navigate(['../'], { relativeTo: this.activatedRoute });
    }
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
