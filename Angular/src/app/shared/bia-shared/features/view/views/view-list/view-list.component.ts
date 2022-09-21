import { Component, OnInit, OnDestroy, EventEmitter, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { SelectItemGroup, TableState } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { Subscription, combineLatest } from 'rxjs';
import { View } from '../../model/view';
import { ViewType, DEFAULT_VIEW, TeamTypeId, TeamTypeRightPrefixe } from 'src/app/shared/constants';
import { Store, select } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { getAllViews, getLastViewChanged, getDataLoaded } from '../../store/view.state';
import { map, skip } from 'rxjs/operators';
import { openViewDialog } from '../../store/views-actions';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { ActivatedRoute } from '@angular/router';
import { QUERY_STRING_VIEW } from '../../model/view.constants';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';

@Component({
  selector: 'bia-view-list',
  templateUrl: './view-list.component.html',
  styleUrls: ['./view-list.component.scss']
})
export class ViewListComponent implements OnInit, OnChanges, OnDestroy {
  groupedViews: SelectItemGroup[];
  translateKeys: string[] = ['bia.views.current', 'bia.views.system', 'bia.views.default', 'bia.views.team', 'bia.views.user'];
  translations: any;
  views: View[];
  selectedView: number;
  defaultView: number;
  currentView = -1;
  urlView: number | null = null;
  private sub = new Subscription();
  @Input() tableStateKey: string;
  @Input() tableState: string;
  @Input() useViewTeamWithTypeId: TeamTypeId | null;
  @Input() displayedColumns: string[]; 
  @Input() columns: KeyValuePair[]; 
  @Output() viewChange = new EventEmitter<string>();

  constructor(
    private store: Store<AppState>,
    public translateService: TranslateService,
    private authService: AuthService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    const dataLoaded$ = this.store.pipe(select(getDataLoaded));
    const allView$ = this.store
      .pipe(select(getAllViews))
      .pipe(map((views) => views.filter((view) => view.tableId === this.tableStateKey)));
    const lastViewChanged$ = this.store.pipe(select(getLastViewChanged));

    this.sub.add(
      combineLatest([dataLoaded$, allView$, lastViewChanged$]).pipe(skip(1)).subscribe(([dataLoaded, views, view]) => {
        if (dataLoaded === true && views && view) {
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
      this.translateService.stream(this.translateKeys).subscribe((translations) => {
        this.translations = translations;
        this.updateGroupedViews();
      })
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    this.onTableStateChange(changes);
    this.onDisplayedColumnsChange(changes)
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  protected onDisplayedColumnsChange(changes: SimpleChanges) {
    if (changes.displayedColumns && changes.displayedColumns.isFirstChange() !== true) {
      setTimeout(() => {
          let tableStateStr = this.getViewState();
          if (tableStateStr)
          {
            this.AutoSelectView(tableStateStr);
          }
      });
  }
  }

  protected onTableStateChange(changes: SimpleChanges) {
    if (changes.tableState && changes.tableState.isFirstChange() !== true) {
      this.AutoSelectView(changes.tableState.currentValue);
    }
  }

  private AutoSelectView(tableStateStr: string) {
    this.selectedView =  this.GetCorrespondingViewId(tableStateStr);
  }

  private GetCorrespondingViewId(preference: string) : number {
    let pref: TableState = JSON.parse(preference);
    pref.selection = null;
    pref.columnWidths = undefined;
    if (pref.first===0 &&
        (pref.filters === undefined || JSON.stringify(pref.filters) === "{}") &&
        JSON.stringify(pref.columnOrder) === JSON.stringify(this.columns.map((c) => c.key)) &&
        pref.rows === 10 &&
        pref.sortField === this.columns[0].key &&
        pref.sortOrder === 1)
    {
      return 0;
    }
    else 
    {
      console.log ("GetCorrespondingViewId: pref" + JSON.stringify(pref) + " >> " + JSON.stringify(pref.filters) + " >> " + JSON.stringify(this.columns.map((c) => c.key)) + " >> " + this.columns[0].key)
      console.log ("GetCorrespondingViewId: 1 >> " + (pref.filters === undefined || JSON.stringify(pref.filters) === "{}"));
      console.log ("GetCorrespondingViewId: 2 >> " + (JSON.stringify(pref.columnOrder) === JSON.stringify(this.columns.map((c) => c.key))));
      console.log ("GetCorrespondingViewId: 3 >> " + (pref.sortField === this.columns[0].key));
    }

    let prefString =  JSON.stringify(pref);
    // console.log("GetCorrespondingView : " + prefString  )
    if (this.views)
    {
      let correspondingView = this.views.find(v => {
        const viewPref: TableState = JSON.parse(v.preference);
        viewPref.selection = null;
        let correspondFind = prefString === JSON.stringify(viewPref)
        return correspondFind;
      });
      if (correspondingView)
      {
        return correspondingView.id
      }
    }
    return this.currentView;
  }

  onViewChange(event: any) {
    this.selectedView = event.value;
    this.updateFilterValues(null, true);
  }

  private updateGroupedViews() {
    if (!this.views || !this.translations) {
      return;
    }

    this.groupedViews = [
      {
        label: this.translations['bia.views.system'],
        items: [{ label: this.translations['bia.views.default'], value: 0 }]
      }
    ];

    let defaultView = 0;
    const currentTeamId = (this.useViewTeamWithTypeId == null) ? -1 : this.authService.getCurrentTeamId(this.useViewTeamWithTypeId);
    const systemViews = this.views.filter(
      (v) =>
        v.viewType === ViewType.System
    );
    const teamViews = this.views.filter(
      (v) =>
        v.viewType === ViewType.Team && (v.viewTeams.some((vs) => currentTeamId == vs.teamId))
    );
    const userViews = this.views.filter((v) => v.viewType === ViewType.User);
    if (systemViews.length > 0) {
      this.groupedViews = [{
        label: this.translations['bia.views.system'],
        items: systemViews.map((v) => ({ label: this.translations['bia.views.' + v.name], value: v.id }))
      }];

      const systemDefault = systemViews.filter((v) =>
        v.name === 'default')[0];
      if (systemDefault) {
        defaultView = systemDefault.id;
      }
    } else {
      this.groupedViews = [
        {
          label: this.translations['bia.views.system'],
          items: [{ label: this.translations['bia.views.default'], value: 0 }]
        }
      ];
    }

    if (teamViews.length > 0) {
      this.groupedViews.push({
        label: this.translations['bia.views.team'],
        items: teamViews.map((v) => {
          return { label: v.name, value: v.id };
        })
      });

      const teamDefault = teamViews.filter((v) =>
        v.viewTeams.some((y) => currentTeamId == y.teamId && y.isDefault === true)
      )[0];
      if (teamDefault) {
        defaultView = teamDefault.id;
      }
    }

    if (userViews.length > 0) {
      this.groupedViews.push({
        label: this.translations['bia.views.user'],
        items: userViews.map((v) => {
          return { label: v.name, value: v.id };
        })
      });
    }

    const userDefault = this.views.filter((v) => v.isUserDefault)[0];
    if (userDefault) {
      defaultView = userDefault.id;
    }

    this.defaultView = defaultView;

    this.groupedViews[0].items.push({ label: this.translations['bia.views.current'], value: this.currentView });
    
  }
  protected initViewByQueryParam(views: View[]) {
    //<a [routerLink]="['/examples/planes-view']" [queryParams]="{ view: 'test2' }">link to plane view</a>
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
  private updateFilterValues(preference: string | null, manualChange:boolean) {
    setTimeout(() => {
      if (!manualChange) this.initViewByQueryParam(this.views);
      if (preference && !this.urlView) {
        const correspondingViewId = this.GetCorrespondingViewId(preference);
        if (!this.isFirstEmitDone || this.selectedView !== correspondingViewId)
        {
          this.selectedView = correspondingViewId;
          this.isFirstEmitDone = true;
          this.viewChange.emit(preference);
        }
      } else {
        if (this.selectedView == undefined)
        {
          this.selectedView =
          this.selectedView !== 0 && this.views.some((x) => x.id === this.selectedView) === true
            ? this.selectedView
            : this.defaultView;
        }
        if (this.selectedView !== 0) {
          const view = this.views.find((v) => v.id === this.selectedView);
          if (view) {
            this.saveViewState(view.preference);
            this.isFirstEmitDone = true;
            this.viewChange.emit(view.preference);
          }
        } else {
          this.isFirstEmitDone = true;
          this.viewChange.emit(DEFAULT_VIEW);
        }
      }
    });
  }

  private saveViewState(stateString: string) {
    if (stateString) {
      const state = JSON.parse(stateString);
      if (state && !state.filters) {
        state.filters = {};
      }
      stateString = JSON.stringify(state);
      sessionStorage.setItem(this.tableStateKey, stateString);
    }
  }

  private getViewState(): string | null {
    return sessionStorage.getItem(this.tableStateKey);
  }

  onManageView() {
    this.store.dispatch(openViewDialog({ tableStateKey: this.tableStateKey }));
  }

  showEditButton() {
    let canSetTeamView = false;
    if (this.useViewTeamWithTypeId != null) {
      var teamTypeRightPrefixe = TeamTypeRightPrefixe.find(t => t.key == this.useViewTeamWithTypeId)?.value;
      canSetTeamView = this.authService.hasPermission(teamTypeRightPrefixe + Permission.View_AddTeamViewSuffix) ||
        this.authService.hasPermission(teamTypeRightPrefixe + Permission.View_UpdateTeamViewSuffix) ||
        this.authService.hasPermission(teamTypeRightPrefixe + Permission.View_SetDefaultTeamViewSuffix) ||
        this.authService.hasPermission(teamTypeRightPrefixe + Permission.View_AssignToTeamSuffix) ||
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
