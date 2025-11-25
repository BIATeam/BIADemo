import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { ViewFormComponent } from '../../components/view-form/view-form.component';
import { View } from '../../model/view';
import { ViewsStore } from '../../store/view.state';
import { ViewsActions } from '../../store/views-actions';

@Component({
  selector: 'bia-view-save',
  templateUrl: './view-save.component.html',
  styleUrls: ['./view-save.component.scss'],
  imports: [CommonModule, ViewFormComponent],
})
export class ViewSaveComponent {
  tableStateKey: string;
  viewId: number;
  currentView = this.store.select(ViewsStore.getCurrentView);

  constructor(
    protected readonly store: Store,
    protected readonly activatedRoute: ActivatedRoute
  ) {
    this.viewId = this.activatedRoute.snapshot.params.viewId;
    this.store.dispatch(ViewsActions.load({ id: this.viewId }));
  }

  protected getViewPreference(): string | null {
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
