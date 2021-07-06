import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Subscription, Observable } from 'rxjs';
import { View } from '../../model/view';
import { Store, select } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import {
  removeUserView,
  addUserView,
  addSiteView,
  removeSiteView,
  setDefaultUserView,
  setDefaultSiteView,
  closeViewDialog,
  updateUserView,
  assignViewToSite,
  updateSiteView
} from '../../store/views-actions';
import { getAllViews, getDisplayViewDialog } from '../../store/view.state';
import { map, tap } from 'rxjs/operators';
import { ViewType } from 'src/app/shared/constants';
import { SiteView } from '../../model/site-view';
import { DefaultView } from '../../model/default-view';
import { SiteDefaultView } from '../../model/site-default-view';
import { Dialog } from 'primeng/dialog';
import { Site } from 'src/app/domains/site/model/site';
import { getAllSites } from 'src/app/domains/site/store/site.state';
import { AssignViewToSite } from '../../model/assign-view-to-site';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { Confirmation, ConfirmationService } from 'primeng/api';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';
import { loadAllSites } from 'src/app/domains/site/store/sites-actions';

@Component({
  selector: 'app-view-dialog',
  templateUrl: './view-dialog.component.html',
  styleUrls: ['./view-dialog.component.scss'],
  providers: [ConfirmationService]
})
export class ViewDialogComponent implements OnInit, OnDestroy {
  display = false;
  @Input() tableStateKey: string;
  private sub = new Subscription();

  sites$: Observable<Site[]>;
  views$: Observable<View[]>;
  viewSites$: Observable<View[]>;
  viewUsers$: Observable<View[]>;
  userViewSelected: View;
  siteViewSelected: View;
  siteSelected: Site;

  canAddSiteView = false;
  canAddUserView = false;
  canUpdateUserView = false;
  canUpdateSiteView = false;
  canDeleteUserView = false;
  canDeleteSiteView = false;
  canSetDefaultUserView = false;
  canSetDefaultSiteView = false;
  canAssignSiteView = false;

  constructor(
    private store: Store<AppState>,
    private authService: AuthService,
    private biaDialogService: BiaDialogService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.setPermissions();
    this.initDisplay();
    this.initSites();
    this.initViews();
    this.initViewSites();
    this.initViewUsers();
  }

  private initDisplay() {
    this.sub.add(
      this.store
        .select(getDisplayViewDialog)
        .pipe()
        .subscribe((tableStateKeySelected) => (this.display = this.tableStateKey === tableStateKeySelected))
    );
  }

  private initViews() {
    this.views$ = this.store
      .pipe(select(getAllViews))
      .pipe(map((views) => views.filter((view) => view.tableId === this.tableStateKey)));
  }

  private initViewSites() {
    this.viewSites$ = this.views$.pipe(map((views) => views.filter((view) => view.viewType === ViewType.Site)));
  }

  private initViewUsers() {
    this.viewUsers$ = this.views$.pipe(map((views) => views.filter((view) => view.viewType === ViewType.User)));
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  initSites() {
    this.sites$ = this.store.select(getAllSites).pipe(
      map((sites) =>
        sites.filter(
          (site) => this.authService.getCurrentSiteId() < 1 || site.id === this.authService.getCurrentSiteId()
        )
      ),
      tap((sites) => {
        if (sites.length === 1) {
          this.siteSelected = sites[0];
        }
      })
    );
    this.store.dispatch(loadAllSites());
  }

  onClose() {
    this.userViewSelected = <View>{};
    this.siteViewSelected = <View>{};
    this.store.dispatch(closeViewDialog());
  }

  showDialogMaximized(dialog: Dialog) {
    dialog.maximize();
  }

  onAssignViewToSite(dto: AssignViewToSite) {
    this.store.dispatch(assignViewToSite(dto));
  }

  onDeleteUserView(viewId: number) {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.userViewSelected = <View>{};
        this.store.dispatch(removeUserView({ id: viewId }));
      }
    };
    this.confirmationService.confirm(confirmation);
  }

  onDeleteSiteView(viewId: number) {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.siteViewSelected = <View>{};
        this.store.dispatch(removeSiteView({ id: viewId }));
      }
    };
    this.confirmationService.confirm(confirmation);
  }

  onSetDefaultUserView(event: { viewId: number; isDefault: boolean }) {
    const defaultView: DefaultView = { id: event.viewId, isDefault: event.isDefault, tableId: this.tableStateKey };
    this.store.dispatch(setDefaultUserView(defaultView));
  }

  onSetDefaultSiteView(event: { viewId: number; isDefault: boolean }) {
    if (this.siteSelected) {
      const defaultView: SiteDefaultView = {
        id: event.viewId,
        isDefault: event.isDefault,
        tableId: this.tableStateKey,
        siteId: this.siteSelected.id
      };
      this.store.dispatch(setDefaultSiteView(defaultView));
    }
  }

  onSaveUserView(view: View) {
    if (view) {
      const json = this.GetViewPreference();
      if (json) {
        view.preference = json;
        view.tableId = this.tableStateKey;
        if (view.id > 0) {
          this.store.dispatch(updateUserView(view));
        } else {
          this.store.dispatch(addUserView(view));
        }
      }
    }
  }

  onSaveSiteView(view: SiteView) {
    if (view && this.siteSelected) {
      const json = this.GetViewPreference();
      if (json) {
        view.preference = json;
        view.tableId = this.tableStateKey;
        view.siteId = this.siteSelected.id;
        if (view.id > 0) {
          this.store.dispatch(updateSiteView(view));
        } else {
          this.store.dispatch(addSiteView(view));
        }
      }
    }
  }

  onUserViewSelected(view: View) {
    this.userViewSelected = view;
  }

  onSiteViewSelected(view: View) {
    this.siteViewSelected = view;
  }

  private GetViewPreference(): string | null {
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

  canSetUserView() {
    return this.canAddUserView || this.canDeleteUserView || this.canSetDefaultUserView || this.canUpdateUserView;
  }

  canSetSiteView() {
    return (
      this.canAddSiteView ||
      this.canDeleteSiteView ||
      this.canSetDefaultSiteView ||
      this.canUpdateSiteView ||
      this.canAssignSiteView
    );
  }

  private setPermissions() {
    this.canAddSiteView = this.authService.hasPermission(Permission.View_AddSiteView);
    this.canAddUserView = this.authService.hasPermission(Permission.View_AddUserView);
    this.canUpdateUserView = this.authService.hasPermission(Permission.View_UpdateUserView);
    this.canUpdateSiteView = this.authService.hasPermission(Permission.View_UpdateSiteView);
    this.canDeleteUserView = this.authService.hasPermission(Permission.View_DeleteUserView);
    this.canDeleteSiteView = this.authService.hasPermission(Permission.View_DeleteSiteView);
    this.canSetDefaultUserView = this.authService.hasPermission(Permission.View_SetDefaultUserView);
    this.canSetDefaultSiteView = this.authService.hasPermission(Permission.View_SetDefaultSiteView);
    this.canAssignSiteView = this.authService.hasPermission(Permission.View_AssignToSite);
  }
}
