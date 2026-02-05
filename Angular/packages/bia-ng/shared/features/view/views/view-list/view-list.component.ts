import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import {
  AuthService,
  BiaAppConstantsService,
  BiaPermission,
} from 'packages/bia-ng/core/public-api';
import { ViewType } from 'packages/bia-ng/models/enum/public-api';
import { BiaTableState, KeyValuePair } from 'packages/bia-ng/models/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { FilterMetadata, PrimeTemplate, SelectItemGroup } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { FloatLabel } from 'primeng/floatlabel';
import { Select } from 'primeng/select';
import { Tooltip } from 'primeng/tooltip';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { TableHelperService } from '../../../../services/table-helper.service';
import { DefaultView } from '../../model/default-view';
import { View } from '../../model/view';
import { QUERY_STRING_VIEW } from '../../model/view.constants';
import { ViewsStore } from '../../store/view.state';
import { ViewsActions } from '../../store/views-actions';
import { ManageViewsDialogComponent } from '../manage-views-dialog/manage-views-dialog.component';

@Component({
  selector: 'bia-view-list',
  templateUrl: './view-list.component.html',
  styleUrls: ['./view-list.component.scss'],
  imports: [
    Select,
    FormsModule,
    PrimeTemplate,
    TranslateModule,
    FloatLabel,
    ButtonDirective,
    CommonModule,
    Tooltip,
    ManageViewsDialogComponent,
  ],
})
export class ViewListComponent implements OnInit, OnChanges, OnDestroy {
  readonly currentView = -10000000;
  readonly undefinedView = -10000001;

  groupedViews: SelectItemGroup[];
  translateKeys: string[] = [
    'bia.views.current',
    'bia.views.system',
    'bia.views.default',
    'bia.views.team',
    'bia.views.user',
  ];
  translations: any;
  views?: View[];

  _selectedView: number = this.undefinedView;
  set selectedView(value: number) {
    this._selectedView = value;
    if (
      this._selectedView !== this.currentView &&
      this._selectedView !== this.undefinedView
    ) {
      this.currentSelectedView = value;
    }
  }

  get selectedView(): number {
    return this._selectedView;
  }

  _currentSelectedView: number = this.undefinedView;
  set currentSelectedView(value: number) {
    this._currentSelectedView = value;
    const currentView = this.getCurrentView();
    if (this.tableStateKey) {
      sessionStorage.setItem(
        this.tableStateKey + 'View',
        JSON.stringify(currentView)
      );
    }
    this.selectedViewChanged.emit(currentView?.id !== 0 ? currentView : null);
  }

  get currentSelectedView(): number {
    return this._currentSelectedView;
  }

  defaultView: number;
  urlView: number | null = null;
  protected sub = new Subscription();
  @Input() tableStateKey: string;
  @Input() tableState: string;
  @Input() defaultViewPref: BiaTableState;
  @Input() useViewTeamWithTypeId: number | null;
  @Input() displayedColumns: string[];
  @Input() columns: KeyValuePair[];
  @Output() viewChange = new EventEmitter<string>();
  @Output() selectedViewChanged = new EventEmitter<View | null>();

  constructor(
    protected store: Store<BiaAppState>,
    public translateService: TranslateService,
    protected authService: AuthService,
    protected route: ActivatedRoute,
    protected tableHelperService: TableHelperService,
    protected router: Router,
    protected activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    const dataLoaded$ = this.store.pipe(
      select(ViewsStore.getDataLoadedAndViews)
    );

    this.sub.add(
      dataLoaded$.pipe(skip(1)).subscribe(state => {
        if (state.dataLoaded === true && state.views) {
          this.views = state.views.filter(
            v => v.tableId === this.tableStateKey
          );
          this.initViewByQueryParam(this.views);
          this.updateGroupedViews();
          this.selectDefaultView();
          this.updateFilterValues(this.getViewState(), false);
        }
      })
    );

    const lastViewChanged$ = this.store.pipe(
      select(ViewsStore.getLastViewChanged)
    );

    this.sub.add(
      lastViewChanged$.pipe(skip(1)).subscribe(view => {
        this.selectedView =
          view && view.tableId === this.tableStateKey
            ? view.id
            : this.defaultView;
      })
    );

    this.sub.add(
      this.translateService
        .stream(this.translateKeys)
        .subscribe(translations => {
          this.translations = translations;
          this.updateGroupedViews();
        })
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    this.onTableStateChange(changes);
    this.onDisplayedColumnsChange(changes);
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  protected onDisplayedColumnsChange(changes: SimpleChanges) {
    if (
      changes.displayedColumns &&
      changes.displayedColumns.isFirstChange() !== true
    ) {
      setTimeout(() => {
        const tableStateStr = this.getViewState();
        if (tableStateStr) {
          this.autoSelectView(tableStateStr);
        }
      });
    }
  }

  protected onTableStateChange(changes: SimpleChanges) {
    if (changes.tableState && changes.tableState.isFirstChange() !== true) {
      this.autoSelectView(changes.tableState.currentValue);
      this.store.dispatch(
        ViewsActions.updateCurrentPreferences({
          preferences: changes.tableState.currentValue,
        })
      );
    }
  }

  protected autoSelectView(tableStateStr: string) {
    if (this.groupedViews) {
      this.selectedView = this.getCorrespondingViewId(tableStateStr);
    }
  }

  protected selectDefaultView() {
    const currentViewStored = sessionStorage.getItem(
      this.tableStateKey + 'View'
    );
    if (currentViewStored) {
      const view: View | null = JSON.parse(currentViewStored);
      if (
        view &&
        this.groupedViews.find(gv => gv.items.find(v => v.value === view.id))
      ) {
        this.selectedView = view.id;
      } else {
        this.selectedView = this.defaultView;
      }
    } else {
      this.selectedView = this.defaultView;
    }
  }

  public getCurrentView(): View | null {
    let view: View | null = null;
    if (
      this.currentSelectedView !== this.currentView &&
      this.currentSelectedView !== this.undefinedView &&
      this.views?.length
    ) {
      this.views.forEach(v => {
        if (v.id === this.currentSelectedView) {
          view = v;
          return;
        }
      });
    }

    if (view === null && this.currentSelectedView === 0) {
      view = {
        id: 0,
        name: 'default',
        tableId: this.tableStateKey,
        preference: JSON.stringify(this.defaultViewPref),
        isUserDefault: false,
        viewType: ViewType.System,
        viewTeams: [],
      } as unknown as View;
    }

    return view;
  }

  protected getCorrespondingViewId(preference: string): number {
    const pref: BiaTableState = JSON.parse(preference);
    pref.columnWidths = undefined;
    const view = this.views?.find(view => view.id === this.currentSelectedView);
    if (
      (view && this.areViewsEgals(pref, JSON.parse(view.preference))) ||
      (this.currentSelectedView === 0 &&
        this.areViewsEgals(pref, this.defaultViewPref))
    ) {
      return this.currentSelectedView;
    }
    return this.currentView;
  }

  protected areViewsEgals(view1: BiaTableState, view2: BiaTableState) {
    return (
      view1.first === view2.first &&
      this.areFilterEgals(view1.filters, view2.filters) &&
      ((this.isNullUndefEmptyStr(view1.advancedFilter) &&
        this.isNullUndefEmptyStr(view2.advancedFilter)) ||
        JSON.stringify(view1.advancedFilter) ===
          JSON.stringify(view2.advancedFilter)) &&
      JSON.stringify(view1.columnOrder) === JSON.stringify(view2.columnOrder) &&
      view1.rows === view2.rows &&
      ((view1.sortField === view2.sortField &&
        view1.sortOrder === view2.sortOrder &&
        ((this.isNullUndefEmptyStr(view1.multiSortMeta) &&
          this.isNullUndefEmptyStr(view2.multiSortMeta)) ||
          JSON.stringify(view1.multiSortMeta) ===
            JSON.stringify(view2.multiSortMeta))) ||
        (!this.isNullUndefEmptyStr(view1.multiSortMeta) &&
          this.isNullUndefEmptyStr(view2.multiSortMeta) &&
          view1.multiSortMeta?.length === 1 &&
          view2.sortField === view1.multiSortMeta[0].field &&
          view2.sortOrder === view1.multiSortMeta[0].order) ||
        (!this.isNullUndefEmptyStr(view2.multiSortMeta) &&
          this.isNullUndefEmptyStr(view1.multiSortMeta) &&
          view2.multiSortMeta?.length === 1 &&
          view1.sortField === view2.multiSortMeta[0].field &&
          view1.sortOrder === view2.multiSortMeta[0].order))
    );
  }

  protected areFilterEgals(
    filters1: { [s: string]: FilterMetadata | FilterMetadata[] } | undefined,
    filters2: { [s: string]: FilterMetadata | FilterMetadata[] } | undefined
  ): boolean {
    if (
      this.isNullUndefEmptyStr(filters1) &&
      this.isNullUndefEmptyStr(filters2)
    ) {
      return true;
    }
    if (JSON.stringify(filters1) === JSON.stringify(filters2)) {
      return true;
    }

    for (const key in filters1) {
      const value1 = filters1[key];
      const value2 = filters2 ? filters2[key] : undefined;
      if (
        JSON.stringify(this.standardizeFilterMetadata(value1)) !==
        JSON.stringify(this.standardizeFilterMetadata(value2))
      ) {
        return false;
      }
    }
    for (const key in filters2) {
      if (filters1 === undefined || !(key in filters1)) {
        if (
          JSON.stringify(this.standardizeFilterMetadata(filters2[key])) !==
          JSON.stringify([])
        ) {
          return false;
        }
      }
    }

    return true;
  }

  protected standardizeFilterMetadata(
    filterMetadata: FilterMetadata | FilterMetadata[] | undefined
  ): FilterMetadata[] {
    const standardized: FilterMetadata[] = [];
    if (this.isValueNullUndefEmptyStr(filterMetadata)) {
      return standardized;
    }
    if (Array.isArray(filterMetadata)) {
      (filterMetadata as FilterMetadata[]).forEach(element => {
        if (
          !TableHelperService.isEmptyFilter(element) &&
          !TableHelperService.isTodayFilter(element)
        ) {
          standardized.push(this.tableHelperService.cleanFilter(element));
        }
      });
    }

    if (
      !TableHelperService.isEmptyFilter(filterMetadata as FilterMetadata) &&
      !TableHelperService.isTodayFilter(filterMetadata as FilterMetadata)
    ) {
      standardized.push(
        this.tableHelperService.cleanFilter(filterMetadata as FilterMetadata)
      );
    }

    if (standardized.length === 1) {
      standardized[0].operator = 'and';
    }
    return standardized;
  }

  protected isNullUndefEmptyStr(obj: any): boolean {
    if (this.isValueNullUndefEmptyStr(obj)) {
      return true;
    }
    if (JSON.stringify(obj) === '{}') {
      return true;
    }
    return Object.values(obj).every(value => {
      // 👇️ check for multiple conditions
      if (this.isValueNullUndefEmptyStr(value)) {
        return true;
      }
      return false;
    });
  }

  protected isValueNullUndefEmptyStr(obj: any): boolean {
    return obj === null || obj === undefined || obj === '';
  }

  onViewChange(event: any) {
    this.selectedView = event.value;
    this.updateFilterValues(null, true);
  }

  protected updateGroupedViews() {
    if (!this.views || !this.translations) {
      return;
    }

    this.groupedViews = [
      {
        label: this.translations['bia.views.system'],
        items: [{ label: this.translations['bia.views.default'], value: 0 }],
      },
    ];

    let defaultView = 0;
    const currentTeamId =
      this.useViewTeamWithTypeId === null
        ? this.currentView
        : this.authService.getCurrentTeamId(this.useViewTeamWithTypeId);
    const systemViews = this.views.filter(v => v.viewType === ViewType.System);
    const teamViews = this.views.filter(
      v =>
        v.viewType === ViewType.Team &&
        v.viewTeams.some(vs => currentTeamId === vs.id)
    );
    const userViews = this.views.filter(v => v.viewType === ViewType.User);
    if (systemViews.length > 0) {
      this.groupedViews = [
        {
          label: this.translations['bia.views.system'],
          items: systemViews.map(v => ({
            label: this.translations['bia.views.' + v.name],
            value: v.id,
          })),
        },
      ];

      const systemDefault = systemViews.filter(v => v.name === 'default')[0];
      if (systemDefault) {
        defaultView = systemDefault.id;
      }
    } else {
      this.groupedViews = [
        {
          label: this.translations['bia.views.system'],
          items: [{ label: this.translations['bia.views.default'], value: 0 }],
        },
      ];
    }

    if (teamViews.length > 0) {
      this.groupedViews.push({
        label: this.translations['bia.views.team'],
        items: teamViews.map(v => {
          return { label: v.name, value: v.id };
        }),
      });

      const teamDefault = teamViews.filter(v =>
        v.viewTeams.some(y => currentTeamId === y.id && y.isDefault === true)
      )[0];
      if (teamDefault) {
        defaultView = teamDefault.id;
      }
    }

    if (userViews.length > 0) {
      this.groupedViews.push({
        label: this.translations['bia.views.user'],
        items: userViews.map(v => {
          return { label: v.name, value: v.id };
        }),
      });
    }

    const userDefault = this.views.filter(v => v.isUserDefault)[0];
    if (userDefault) {
      defaultView = userDefault.id;
    }

    this.defaultView = defaultView;
  }

  protected initViewByQueryParam(views: View[]) {
    if (views?.length > 0) {
      const viewName = this.route.snapshot.queryParamMap.get(QUERY_STRING_VIEW);
      if (viewName && viewName.length > 0) {
        const view = views.find(v => v.name === viewName);
        if (view && view.id > 0) {
          this.urlView = view.id;
          this.selectedView = view.id;
        }
      }
    }
  }

  isFirstEmitDone = false;

  protected updateFilterValues(
    preference: string | null,
    manualChange: boolean
  ) {
    if (!manualChange && this.views) {
      this.initViewByQueryParam(this.views);
    }
    if (preference && !this.urlView) {
      const correspondingViewId = this.getCorrespondingViewId(preference);
      if (!this.isFirstEmitDone || this.selectedView !== correspondingViewId) {
        this.selectedView = correspondingViewId;
        this.isFirstEmitDone = true;
        setTimeout(() => {
          this.viewChange.emit(preference);
        });
      }
    } else {
      if (this.selectedView === this.undefinedView) {
        this.selectedView = this.defaultView;
      }
      if (this.selectedView !== 0 && this.views) {
        const view = this.views.find(v => v.id === this.selectedView);
        if (view) {
          this.isFirstEmitDone = true;
          setTimeout(() => {
            this.viewChange.emit(view.preference);
          });
        }
      } else {
        this.isFirstEmitDone = true;
        setTimeout(() => {
          this.viewChange.emit(JSON.stringify(this.defaultViewPref));
        });
      }
    }
  }

  protected getViewState(): string | null {
    const preferences = sessionStorage.getItem(this.tableStateKey);
    this.store.dispatch(ViewsActions.updateCurrentPreferences({ preferences }));
    return preferences;
  }

  onManageView() {
    this.store.dispatch(
      ViewsActions.openViewDialog({ tableStateKey: this.tableStateKey })
    );
  }

  showEditButton() {
    let canSetTeamView = false;
    if (this.useViewTeamWithTypeId !== null) {
      const teamTypeRightPrefix =
        BiaAppConstantsService.teamTypeRightPrefix.find(
          t => t.key === this.useViewTeamWithTypeId
        )?.value;
      if (teamTypeRightPrefix) {
        canSetTeamView =
          this.authService.hasPermission(
            teamTypeRightPrefix + BiaPermission.View_AddTeamViewSuffix
          ) ||
          this.authService.hasPermission(
            teamTypeRightPrefix + BiaPermission.View_UpdateTeamViewSuffix
          ) ||
          this.authService.hasPermission(
            teamTypeRightPrefix + BiaPermission.View_SetDefaultTeamViewSuffix
          ) ||
          this.authService.hasPermission(
            teamTypeRightPrefix + BiaPermission.View_AssignToTeamSuffix
          ) ||
          this.authService.hasPermission(BiaPermission.View_DeleteTeamView);
      }
    }
    return (
      canSetTeamView ||
      this.authService.hasPermission(BiaPermission.View_AddUserView) ||
      this.authService.hasPermission(BiaPermission.View_UpdateUserView) ||
      this.authService.hasPermission(BiaPermission.View_DeleteUserView) ||
      this.authService.hasPermission(BiaPermission.View_SetDefaultUserView)
    );
  }

  openSave() {
    const selectedView = this.views?.find(
      v => v.id === this.currentSelectedView
    );

    this.router.navigate(['view', this.currentSelectedView, 'saveView'], {
      relativeTo: this.activatedRoute,
      queryParams: {
        view:
          this.currentSelectedView !== 0 && selectedView
            ? selectedView.name
            : null,
      },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }

  toggleDefault() {
    const defaultView: DefaultView = {
      id: this.currentSelectedView,
      isDefault: this.currentSelectedView !== this.defaultView,
      tableId: this.tableStateKey,
    };
    this.store.dispatch(ViewsActions.setDefaultUserView(defaultView));
  }

  restoreView() {
    this.selectedView = this.currentSelectedView;
    this.updateFilterValues(null, true);
  }
}
