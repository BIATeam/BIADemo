import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { BiaTableState } from 'bia-ng/models';
import { Observable } from 'rxjs';
import { planeCRUDConfiguration } from 'src/app/features/planes/plane.constants';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { CrudItemEditComponent } from '../../../../feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { ViewSaveFormComponent } from '../../components/view-save-form/view-save-form.component';
import { ViewStateRecapComponent } from '../../components/view-state-recap/view-state-recap.component';
import { View } from '../../model/view';
import { ViewsStore } from '../../store/view.state';
import { ViewsActions } from '../../store/views-actions';

@Component({
  selector: 'bia-view-save',
  templateUrl: './view-save.component.html',
  styleUrls: ['./view-save.component.scss'],
  imports: [
    CommonModule,
    ViewSaveFormComponent,
    ViewStateRecapComponent,
    SpinnerComponent,
  ],
})
export class ViewSaveComponent extends CrudItemEditComponent<View> {
  tableStateKey: string = planeCRUDConfiguration.tableStateKey;
  viewId: number;
  currentView: Observable<View> = this.store.select(ViewsStore.getCurrentView);
  viewPreference?: BiaTableState;

  constructor(
    protected readonly store: Store,
    protected readonly activatedRoute: ActivatedRoute
  ) {
    super();
    this.viewId = this.activatedRoute.snapshot.params.viewId;
    this.store.dispatch(ViewsActions.load({ id: this.viewId }));
    this.viewPreference = this.getViewPreference();
  }

  protected getViewPreference(): BiaTableState | undefined {
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
      const json = this.getViewPreferenceAsString();
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
