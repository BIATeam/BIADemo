import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import {
  CrudState,
  DEFAULT_CRUD_STATE,
} from 'packages/bia-ng/models/public-api';
import { View } from '../model/view';
import { ViewsActions } from './views-actions';

// This adapter will allow is to manipulate views (mostly CRUD operations)
export const viewsAdapter = createEntityAdapter<View>({
  selectId: (view: View) => view.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<View> {
//   ids: string[] | number[];
//   entities: { [id: string]: View };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface ViewState extends CrudState<View>, EntityState<View> {
  // additional props here
  displayViewDialog: string;
  lastViewChanged: View;
  dataLoaded: boolean;
  currentPreferences: string | null;
}

export const INIT_VIEW_STATE: ViewState = viewsAdapter.getInitialState({
  // additional props default values here
  ...DEFAULT_CRUD_STATE(),
  displayViewDialog: '',
  lastViewChanged: <View>{},
  dataLoaded: false,
  currentPreferences: null,
});

export const viewReducers = createReducer<ViewState>(
  INIT_VIEW_STATE,
  on(ViewsActions.clearCurrent, state => {
    return {
      ...state,
      currentItem: <View>{},
      loadingGet: false,
    };
  }),
  on(ViewsActions.openViewDialog, (state, { tableStateKey }) => {
    return { ...state, displayViewDialog: tableStateKey };
  }),
  on(ViewsActions.closeViewDialog, state => {
    return { ...state, displayViewDialog: '' };
  }),
  on(ViewsActions.setViewSuccess, (state, view) => {
    return { ...state, lastViewChanged: view };
  }),
  on(ViewsActions.loadSuccess, (state, { view }) => {
    return { ...state, currentItem: view, loadingGet: false };
  }),
  on(ViewsActions.loadAllSuccess, (state, { views }) => {
    const newState = viewsAdapter.setAll(views, state);
    return { ...newState, dataLoaded: true };
  }),
  on(ViewsActions.loadAllView, state => {
    return { ...state, loadAllView: false };
  }),
  on(ViewsActions.updateCurrentPreferences, (state, { preferences }) => {
    return { ...state, currentPreferences: preferences };
  })
);

export const getViewById = (id: number) => (state: ViewState) =>
  state.entities[id];
