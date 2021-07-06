import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { loadAllView, loadAllSuccess, closeViewDialog, openViewDialog, setViewSuccess } from './views-actions';
import { View } from '../model/view';

// This adapter will allow is to manipulate views (mostly CRUD operations)
export const viewsAdapter = createEntityAdapter<View>({
  selectId: (view: View) => view.id,
  sortComparer: false
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

export interface State extends EntityState<View> {
  // additional props here
  displayViewDialog: string;
  lastViewChanged: View;
  dataLoaded: boolean;
}

export const INIT_STATE: State = viewsAdapter.getInitialState({
  // additional props default values here
  displayViewDialog: '',
  lastViewChanged: <View>{},
  dataLoaded: false
});

export const viewReducers = createReducer<State>(
  INIT_STATE,
  on(openViewDialog, (state, { tableStateKey }) => {
    return { ...state, displayViewDialog: tableStateKey };
  }),
  on(closeViewDialog, (state) => {
    return { ...state, displayViewDialog: '' };
  }),
  on(setViewSuccess, (state, view) => {
    return { ...state, lastViewChanged: view };
  }),
  on(loadAllSuccess, (state, { views }) => {
    const newState = viewsAdapter.setAll(views, state);
    return { ...newState, dataLoaded: true };
  }),
  on(loadAllView, (state) => {
    return { ...state, loadAllView: false };
  }),
);

export const getViewById = (id: number) => (state: State) => state.entities[id];
