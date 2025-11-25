import { CommonModule } from '@angular/common';
import { Component, Inject, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import { BaseDto, BiaTableState } from 'packages/bia-ng/models/public-api';
import { CrudConfig, CrudItemService } from 'packages/bia-ng/shared/public-api';
import { ButtonDirective } from 'primeng/button';
import { Observable, map } from 'rxjs';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { CrudItemEditComponent } from '../../../../feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { ViewSaveFormComponent } from '../../components/view-save-form/view-save-form.component';
import { ViewStateRecapComponent } from '../../components/view-state-recap/view-state-recap.component';
import { View } from '../../model/view';
import { viewCRUDConfiguration } from '../../model/view.constants';
import { ViewService } from '../../services/view.service';
import { ViewsStore } from '../../store/view.state';

@Component({
  selector: 'bia-view-save',
  templateUrl: './view-save.component.html',
  styleUrls: ['./view-save.component.scss'],
  imports: [
    CommonModule,
    ViewSaveFormComponent,
    ViewStateRecapComponent,
    SpinnerComponent,
    TranslateModule,
    ButtonDirective,
  ],
  providers: [
    {
      provide: 'FEATURE_SERVICE',
      useFactory: (injector: Injector, route: ActivatedRoute) => {
        const serviceType =
          route.parent?.parent?.snapshot.data['featureServiceType'];
        if (serviceType) {
          return injector.get(serviceType);
        } else {
          return null;
        }
      },
      deps: [Injector, ActivatedRoute],
    },
  ],
})
export class ViewSaveComponent<
  TDto extends BaseDto,
> extends CrudItemEditComponent<View> {
  tableStateKey: string;
  featureConfiguration?: CrudConfig<TDto>;
  currentView: Observable<View> = this.store.select(ViewsStore.getCurrentView);
  viewPreference$: Observable<BiaTableState | undefined>;
  title: string;

  constructor(
    protected injector: Injector,
    protected viewService: ViewService,
    protected readonly store: Store,
    protected readonly activatedRoute: ActivatedRoute,
    @Inject('FEATURE_SERVICE')
    protected readonly featureService: CrudItemService<TDto>
  ) {
    super(injector, viewService);
    this.crudConfiguration = viewCRUDConfiguration;
    this.setTableStateKey();
    this.viewPreference$ = this.store
      .select(ViewsStore.getViewCurrentPreferences)
      .pipe(map((p: string | null) => this.getViewPreference(p)));
    this.title = this.activatedRoute.snapshot.data['title'];
  }

  protected setTableStateKey(): void {
    const parent: ActivatedRoute | null | undefined =
      this.activatedRoute.parent?.parent;

    if (!parent) {
      return;
    }

    this.tableStateKey = parent.snapshot.data['featureViews'] + 'Grid';
    this.featureConfiguration = parent.snapshot.data['featureConfiguration'];
  }

  protected getViewPreference(
    stateString: string | null
  ): BiaTableState | undefined {
    if (stateString) {
      const state = JSON.parse(stateString);
      return state;
    }
    return undefined;
  }

  protected getViewPreferenceAsString(): string | null {
    let stateString = sessionStorage.getItem(this.tableStateKey);
    if (stateString) {
      const state = JSON.parse(stateString);
      if (!state.filters) {
        state.filters = {};
      }
      stateString = JSON.stringify(state);
    }

    return stateString;
  }

  onViewSubmitted(view: View) {
    if (view) {
      const json = this.getViewPreferenceAsString();
      if (json) {
        view.preference = json;
        view.tableId = this.tableStateKey;
        this.onSubmitted(view);
      }
    }
  }

  protected navigateBack() {
    this.router.navigate(['../../../'], {
      relativeTo: this.activatedRoute,
      queryParamsHandling: 'preserve',
    });
  }
}
