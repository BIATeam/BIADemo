import { CommonModule } from '@angular/common';
import { Component, Inject, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import {
  BiaAppConstantsService,
  BiaPermission,
  CoreTeamsStore,
} from 'packages/bia-ng/core/public-api';
import {
  BaseDto,
  BiaTableState,
  PermissionTeams,
  Team,
} from 'packages/bia-ng/models/public-api';
import { ButtonDirective } from 'primeng/button';
import { Observable, map, of } from 'rxjs';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { CrudConfig } from '../../../../feature-templates/crud-items/model/crud-config';
import { CrudItemService } from '../../../../feature-templates/crud-items/services/crud-item.service';
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
  currentView$: Observable<View>;
  viewPreference$: Observable<BiaTableState | undefined>;
  title: string;

  canAddTeamView = false;
  canAddUserView = false;
  canUpdateUserView = false;
  canUpdateTeamView = false;
  teamList$: Observable<Team[]> = of([]);
  permissionAssignTeams?: PermissionTeams;
  submittedView?: View;

  constructor(
    protected injector: Injector,
    protected viewService: ViewService,
    protected readonly store: Store,
    protected readonly activatedRoute: ActivatedRoute,
    @Inject('FEATURE_SERVICE')
    protected readonly featureService: CrudItemService<TDto>
  ) {
    super(injector, viewService);
    this.currentView$ = this.store.select(ViewsStore.getCurrentView);
    this.crudConfiguration = viewCRUDConfiguration;
    this.setTableStateKey();
    this.viewPreference$ = this.store
      .select(ViewsStore.getViewCurrentPreferences)
      .pipe(map((p: string | null) => this.getViewPreference(p)));
    this.title = this.activatedRoute.snapshot.data['title'];
  }

  protected setPermissions() {
    if (this.featureConfiguration?.useViewTeamWithTypeId) {
      const teamTypeRightPrefix =
        BiaAppConstantsService.teamTypeRightPrefix.find(
          t => t.key === this.featureConfiguration?.useViewTeamWithTypeId
        )?.value;
      this.canAddTeamView = this.authService.hasPermission(
        teamTypeRightPrefix + BiaPermission.View_AddTeamViewSuffix
      );
      this.canUpdateTeamView = this.authService.hasPermission(
        teamTypeRightPrefix + BiaPermission.View_UpdateTeamViewSuffix
      );
      this.permissionAssignTeams = this.authService
        .getDecryptedToken()
        .userData.crossTeamPermissions?.find(
          p =>
            p.permission ===
            teamTypeRightPrefix + BiaPermission.View_AssignToTeamSuffix
        );
    }
    this.canAddUserView = this.authService.hasPermission(
      BiaPermission.View_AddUserView
    );
    this.canUpdateUserView = this.authService.hasPermission(
      BiaPermission.View_UpdateUserView
    );
  }

  protected setTableStateKey(): void {
    const parent: ActivatedRoute | null | undefined =
      this.activatedRoute.parent?.parent;

    if (!parent) {
      return;
    }

    this.featureConfiguration = parent.snapshot.data['featureConfiguration'];
    this.tableStateKey =
      this.featureConfiguration?.tableStateKey ??
      parent.snapshot.data['featureStateKey'];
    if (this.featureConfiguration?.useViewTeamWithTypeId) {
      this.teamList$ = this.store.select(
        CoreTeamsStore.getAllTeamsOfType(
          this.featureConfiguration?.useViewTeamWithTypeId
        )
      );
    }
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
    this.submittedView = view;
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
    if (this.submittedView) {
      this.router.navigate(['../../../'], {
        relativeTo: this.activatedRoute,
        queryParams: { view: this.submittedView.name ?? null },
        queryParamsHandling: 'merge',
      });
    } else {
      this.router.navigate(['../../../'], {
        relativeTo: this.activatedRoute,
        queryParamsHandling: 'preserve',
      });
    }
  }
}
