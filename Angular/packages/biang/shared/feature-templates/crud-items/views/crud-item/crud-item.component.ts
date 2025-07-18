import { Injector } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Actions } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { BiaTranslationService } from 'biang/core';
import { BaseDto } from 'biang/models';
import { BiaAppState } from 'biang/store';
import { Subscription } from 'rxjs';
import { LayoutMode } from '../../../../components/layout/dynamic-layout/dynamic-layout.component';
import { CrudConfig } from '../../model/crud-config';
import { CrudItemSingleService } from '../../services/crud-item-single.service';

export abstract class CrudItemComponent<CrudItem extends BaseDto> {
  protected sub = new Subscription();
  public crudConfiguration: CrudConfig<CrudItem>;

  protected store: Store<BiaAppState>;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected biaTranslationService: BiaTranslationService;
  protected actions: Actions;

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>
  ) {
    this.store = this.injector.get<Store<BiaAppState>>(Store);
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.biaTranslationService = this.injector.get<BiaTranslationService>(
      BiaTranslationService
    );
    this.actions = this.injector.get<Actions>(Actions);
  }

  abstract onSubmitted(crudItemToUpdate: CrudItem): void;

  onCancelled() {
    this.navigateBack();
  }

  protected abstract navigateBack(): void;

  get showPopupButton(): boolean {
    return (
      this.crudConfiguration.showIcons.showPopup &&
      !this.crudConfiguration.usePopup
    );
  }

  get showSplitButton(): boolean {
    return (
      this.crudConfiguration.showIcons.showSplit &&
      !this.crudConfiguration.useSplit
    );
  }

  get showFullPageButton(): boolean {
    return (
      ((this.crudConfiguration.showIcons.showPopup ||
        this.crudConfiguration.showIcons.showSplit) &&
        this.crudConfiguration.usePopup) ||
      this.crudConfiguration.useSplit
    );
  }

  protected notifyLayoutChange(layoutMode: LayoutMode) {
    switch (layoutMode) {
      case LayoutMode.popup:
        this.crudConfiguration.useCalcMode = false;
        this.crudConfiguration.useSplit = false;
        this.crudConfiguration.usePopup = true;
        break;
      case LayoutMode.splitPage:
        this.crudConfiguration.useCalcMode = false;
        this.crudConfiguration.useSplit = true;
        this.crudConfiguration.usePopup = false;
        break;
      case LayoutMode.fullPage:
        this.crudConfiguration.useCalcMode = false;
        this.crudConfiguration.useSplit = false;
        this.crudConfiguration.usePopup = false;
        break;
    }

    this.crudItemService.notifyConfigChange();
  }
}
