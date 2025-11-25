import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { planeCRUDConfiguration } from 'src/app/features/planes/plane.constants';
import { ViewFormComponent } from '../../components/view-form/view-form.component';
import { ViewStateRecapComponent } from '../../components/view-state-recap/view-state-recap.component';
import { View } from '../../model/view';
import { ViewsStore } from '../../store/view.state';
import { ViewsActions } from '../../store/views-actions';

@Component({
  selector: 'bia-view-save',
  templateUrl: './view-save.component.html',
  styleUrls: ['./view-save.component.scss'],
  imports: [CommonModule, ViewFormComponent, ViewStateRecapComponent],
})
export class ViewSaveComponent {
  tableStateKey: string = planeCRUDConfiguration.tableStateKey;
  viewId: number;
  currentView = this.store.select(ViewsStore.getCurrentView);
  viewPreference: any;

  constructor(
    protected readonly store: Store,
    protected readonly activatedRoute: ActivatedRoute
  ) {
    this.viewId = this.activatedRoute.snapshot.params.viewId;
    this.store.dispatch(ViewsActions.load({ id: this.viewId }));
    this.viewPreference = this.getViewPreference();
  }

  protected getViewPreference(): any | undefined {
    let stateString = sessionStorage.getItem(this.tableStateKey);
    console.error(stateString, this.tableStateKey);
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

  onSaveUserView(view: View) {
    if (view) {
      const json = this.getViewPreference();
      if (json) {
        view.preference = json;
        view.tableId = this.tableStateKey;
        if (view.id > 0) {
          this.store.dispatch(ViewsActions.updateUserView(view));
        } else {
          this.store.dispatch(ViewsActions.addUserView(view));
        }
      }
    }
  }
}
