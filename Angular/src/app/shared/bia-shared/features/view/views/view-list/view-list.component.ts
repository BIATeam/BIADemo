import { NgIf } from '@angular/common';
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
import { ActivatedRoute } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { FilterMetadata, PrimeTemplate, SelectItemGroup } from 'primeng/api';
import { FloatLabel } from 'primeng/floatlabel';
import { Select } from 'primeng/select';
import { Subscription, combineLatest } from 'rxjs';
import { map, skip } from 'rxjs/operators';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTableState } from 'src/app/shared/bia-shared/model/bia-table-state';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { TableHelperService } from 'src/app/shared/bia-shared/services/table-helper.service';
import {
  TeamTypeId,
  TeamTypeRightPrefix,
  ViewType,
} from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';
import { AppState } from 'src/app/store/state';
import { View } from '../../model/view';
import { QUERY_STRING_VIEW } from '../../model/view.constants';
import {
  getAllViews,
  getDataLoaded,
  getLastViewChanged,
} from '../../store/view.state';
import { openViewDialog } from '../../store/views-actions';
import { ViewDialogComponent } from '../view-dialog/view-dialog.component';

const currentView = -1;
const undefinedView = -2;

@Component({
  selector: 'bia-view-list',
  templateUrl: './view-list.component.html',
  styleUrls: ['./view-list.component.scss'],
  imports: [
    Select,
    FormsModule,
    PrimeTemplate,
    NgIf,
    ViewDialogComponent,
    TranslateModule,
    FloatLabel,
  ],
})
export class ViewListComponent implements OnInit, OnChanges, OnDestroy {
  groupedViews: SelectItemGroup[];
  translateKeys: string[] = [
    'bia.views.current',
    'bia.views.system',
    'bia.views.default',
    'bia.views.team',
    'bia.views.user',
  ];
  translations: any;
  views: View[];
  selectedView: number = undefinedView;
  selectedViewName: string | null = null;
  defaultView: number;
  urlView: number | null = null;
  protected sub = new Subscription();
  @Input() tableStateKey: string;
  @Input() tableState: string;
  @Input() defaultViewPref: BiaTableState;
  @Input() useViewTeamWithTypeId: TeamTypeId | null;
  @Input() displayedColumns: string[];
  @Input() columns: KeyValuePair[];
  @Output() viewChange = new EventEmitter<string>();

  constructor(
    protected store: Store<AppState>,
    public translateService: TranslateService,
    protected authService: AuthService,
    protected route: ActivatedRoute,
    protected tableHelperService: TableHelperService
  ) {}

  ngOnInit() {
    const dataLoaded$ = this.store.pipe(select(getDataLoaded));
    const allView$ = this.store
      .pipe(select(getAllViews))
      .pipe(
        map(views => views.filter(view => view.tableId === this.tableStateKey))
      );
    const lastViewChanged$ = this.store.pipe(select(getLastViewChanged));

    this.sub.add(
      combineLatest([dataLoaded$, allView$, lastViewChanged$])
        .pipe(skip(1))
        .subscribe(([dataLoaded, views, view]) => {
          if (dataLoaded === true && views && view) {
            if (
              this.views === undefined ||
              this.views.length !== views.length
            ) {
              // the list of view change, so we reset the view selection.
              this.selectedView = undefinedView;
            }
            this.views = views;
            if (view && view.id > 0) {
              this.selectedView = view.id;
            }
            this.updateGroupedViews();
            this.updateFilterValues(this.getViewState(), false);
          }
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
    }
  }

  protected autoSelectView(tableStateStr: string) {
    this.selectedView = this.getCorrespondingViewId(tableStateStr);
    this.selectedViewName = this.getCurrentViewName();
  }

  public getCurrentViewName(): string | null {
    let viewName: string | null = null;
    if (this.selectedView > -1 && this.views?.length) {
      this.views.forEach(v => {
        if (v.id === this.selectedView) {
          viewName = v.name;
          return;
        }
      });
    }

    return viewName;
  }

  protected getCorrespondingViewId(preference: string): number {
    const pref: BiaTableState = JSON.parse(preference);
    pref.columnWidths = undefined;
    if (this.defaultViewPref !== undefined) {
      if (this.areViewsEgals(pref, this.defaultViewPref)) {
        return 0;
      }
    } else {
      console.log('ViewList component Error: defaultViewPref is not defined');
    }

    // let prefString =  JSON.stringify(pref);
    // console.log("GetCorrespondingView : " + prefString  )
    if (this.views) {
      const correspondingViews = this.views.filter(v => {
        const viewPref: BiaTableState = JSON.parse(v.preference);
        return this.areViewsEgals(pref, viewPref);
      });

      if (correspondingViews?.length > 0) {
        // There may be two identical views.
        let correspondingView = correspondingViews.find(
          v => v.id === this.selectedView
        );

        if (!correspondingView) {
          correspondingView = correspondingViews[0];
        }

        if (correspondingView) {
          return correspondingView.id;
        }
      }
    }

    return currentView;
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
        if (!TableHelperService.isEmptyFilter(element)) {
          standardized.push(this.tableHelperService.cleanFilter(element));
        }
      });
    }

    if (!TableHelperService.isEmptyFilter(filterMetadata as FilterMetadata)) {
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
        ? -1
        : this.authService.getCurrentTeamId(this.useViewTeamWithTypeId);
    const systemViews = this.views.filter(v => v.viewType === ViewType.System);
    const teamViews = this.views.filter(
      v =>
        v.viewType === ViewType.Team &&
        v.viewTeams.some(vs => currentTeamId === vs.teamId)
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
        v.viewTeams.some(
          y => currentTeamId === y.teamId && y.isDefault === true
        )
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

    this.groupedViews[0].items.push({
      label: this.translations['bia.views.current'],
      value: currentView,
    });
  }
  protected initViewByQueryParam(views: View[]) {
    //<a [routerLink]="['/planes-view']" [queryParams]="{ view: 'test2' }">link to plane view</a>
    if (views?.length > 0) {
      const viewName = this.route.snapshot.queryParamMap.get(QUERY_STRING_VIEW);
      if (viewName && viewName.length > 0) {
        const view = views.find(v => v.name === viewName);
        if (view && view.id > 0) {
          this.urlView = view.id;
          //setTimeout(() => {
          this.selectedView = view.id;
          //});
        }
      }
    }
  }
  isFirstEmitDone = false;
  protected updateFilterValues(
    preference: string | null,
    manualChange: boolean
  ) {
    //setTimeout(() => {
    if (!manualChange) this.initViewByQueryParam(this.views);
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
      if (this.selectedView === undefinedView) {
        this.selectedView = this.defaultView;
      }
      if (this.selectedView !== 0) {
        const view = this.views.find(v => v.id === this.selectedView);
        if (view) {
          //this.saveViewState(view.preference);
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
    //});
  }
  /*
    protected saveViewState(stateString: string) {
      if (stateString) {
        const state = JSON.parse(stateString);
        if (state && !state.filters) {
          state.filters = {};
        }
        stateString = JSON.stringify(state);
        sessionStorage.setItem(this.tableStateKey, stateString);
      }
    }*/

  protected getViewState(): string | null {
    return sessionStorage.getItem(this.tableStateKey);
  }

  onManageView() {
    this.store.dispatch(openViewDialog({ tableStateKey: this.tableStateKey }));
  }

  showEditButton() {
    let canSetTeamView = false;
    if (this.useViewTeamWithTypeId !== null) {
      const teamTypeRightPrefix = TeamTypeRightPrefix.find(
        t => t.key === this.useViewTeamWithTypeId
      )?.value;
      canSetTeamView =
        this.authService.hasPermission(
          teamTypeRightPrefix + Permission.View_AddTeamViewSuffix
        ) ||
        this.authService.hasPermission(
          teamTypeRightPrefix + Permission.View_UpdateTeamViewSuffix
        ) ||
        this.authService.hasPermission(
          teamTypeRightPrefix + Permission.View_SetDefaultTeamViewSuffix
        ) ||
        this.authService.hasPermission(
          teamTypeRightPrefix + Permission.View_AssignToTeamSuffix
        ) ||
        this.authService.hasPermission(Permission.View_DeleteTeamView);
    }
    return (
      canSetTeamView ||
      this.authService.hasPermission(Permission.View_AddUserView) ||
      this.authService.hasPermission(Permission.View_UpdateUserView) ||
      this.authService.hasPermission(Permission.View_DeleteUserView) ||
      this.authService.hasPermission(Permission.View_SetDefaultUserView)
    );
  }
}
