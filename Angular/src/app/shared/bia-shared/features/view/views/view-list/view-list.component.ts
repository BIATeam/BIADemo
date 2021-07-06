import { Component, OnInit, OnDestroy, EventEmitter, Output, Input } from '@angular/core';
import { SelectItemGroup } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { Subscription, combineLatest } from 'rxjs';
import { View } from '../../model/view';
import { ViewType, DEFAULT_VIEW } from 'src/app/shared/constants';
import { Store, select } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { getAllViews, getLastViewChanged, getDataLoaded } from '../../store/view.state';
import { map } from 'rxjs/operators';
import { openViewDialog } from '../../store/views-actions';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'app-view-list',
  templateUrl: './view-list.component.html',
  styleUrls: ['./view-list.component.scss']
})
export class ViewListComponent implements OnInit, OnDestroy {
  groupedViews: SelectItemGroup[];
  translateKeys: string[] = ['bia.views.system', 'bia.views.default', 'bia.views.site', 'bia.views.user'];
  translations: any;
  views: View[];
  selectedView: number;
  defaultView: number;
  private sub = new Subscription();
  @Input() tableStateKey: string;
  @Output() viewChange = new EventEmitter<string>();

  constructor(
    private store: Store<AppState>,
    public translateService: TranslateService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    const dataLoaded$ = this.store.pipe(select(getDataLoaded));
    const allView$ = this.store
      .pipe(select(getAllViews))
      .pipe(map((views) => views.filter((view) => view.tableId === this.tableStateKey)));
    const lastViewChanged$ = this.store.pipe(select(getLastViewChanged));

    this.sub.add(
      combineLatest([dataLoaded$, allView$, lastViewChanged$]).subscribe(([dataLoaded, views, view]) => {
        if (dataLoaded === true && views && view) {
          this.views = views;
          this.selectedView = view.id;
          this.updateGroupedViews();
          this.updateFilterValues(this.getViewState());
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

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }

    const defaultView = this.views.find((v) => v.id === this.defaultView);
    if (defaultView) {
      sessionStorage.setItem(this.tableStateKey, defaultView.preference);
    } else {
      sessionStorage.removeItem(this.tableStateKey);
    }
  }

  onViewChange(event: any) {
    this.selectedView = event.value;
    this.updateFilterValues();
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
    const currentSiteId = this.authService.getCurrentSiteId();
    const siteViews = this.views.filter(
      (v) =>
        v.viewType === ViewType.Site && (currentSiteId < 1 || v.viewSites.some((vs) => vs.siteId === currentSiteId))
    );
    const userViews = this.views.filter((v) => v.viewType === ViewType.User);
    if (siteViews.length > 0) {
      this.groupedViews.push({
        label: this.translations['bia.views.site'],
        items: siteViews.map((v) => {
          return { label: v.name, value: v.id };
        })
      });

      const siteDefault = siteViews.filter((v) =>
        v.viewSites.some((y) => y.siteId === currentSiteId && y.isDefault === true)
      )[0];
      if (siteDefault) {
        defaultView = siteDefault.id;
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

    this.selectedView =
      this.selectedView > 0 && this.views.some((x) => x.id === this.selectedView) === true
        ? this.selectedView
        : defaultView;
    this.defaultView = defaultView;
  }

  private updateFilterValues(preference?: string | null) {
    if (preference) {
      this.selectedView = this.defaultView;
      this.viewChange.emit(preference);
    } else {
      if (this.selectedView > 0) {
        const view = this.views.find((v) => v.id === this.selectedView);
        if (view) {
          this.saveViewState(view.preference);
          this.viewChange.emit(view.preference);
        }
      } else {
        this.viewChange.emit(DEFAULT_VIEW);
      }
    }
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
    return (
      this.authService.hasPermission(Permission.View_AddSiteView) ||
      this.authService.hasPermission(Permission.View_AddUserView) ||
      this.authService.hasPermission(Permission.View_UpdateUserView) ||
      this.authService.hasPermission(Permission.View_UpdateSiteView) ||
      this.authService.hasPermission(Permission.View_DeleteUserView) ||
      this.authService.hasPermission(Permission.View_DeleteSiteView) ||
      this.authService.hasPermission(Permission.View_SetDefaultUserView) ||
      this.authService.hasPermission(Permission.View_SetDefaultSiteView) ||
      this.authService.hasPermission(Permission.View_AssignToSite)
    );
  }
}
