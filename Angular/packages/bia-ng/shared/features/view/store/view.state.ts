import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { View } from '../model/view';
import * as fromViews from './views-reducer';

export namespace ViewsStore {
  export interface ViewsState {
    views: fromViews.ViewState;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(state: ViewsState | undefined, action: Action) {
    return combineReducers({
      views: fromViews.viewReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getViewsState = createFeatureSelector<ViewsState>('views');

  export const getCurrentView = createSelector(
    getViewsState,
    state => state.views.currentItem ?? <View>{}
  );

  export const getViewsEntitiesState = createSelector(
    getViewsState,
    state => state.views
  );

  export const getViewsTotalCount = createSelector(
    getViewsEntitiesState,
    state => state.totalCount
  );

  export const getLastLazyLoadEvent = createSelector(
    getViewsEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getViewLoadingGet = createSelector(
    getViewsEntitiesState,
    state => state.loadingGet
  );

  export const getViewLoadingGetAll = createSelector(
    getViewsEntitiesState,
    state => state.loadingGetAll
  );

  export const getViewCurrentPreferences = createSelector(
    getViewsEntitiesState,
    state => state.currentPreferences
  );

  export const { selectAll: getAllPlanes } =
    fromViews.viewsAdapter.getSelectors(getViewsEntitiesState);

  export const getPlaneById = (id: number) =>
    createSelector(getViewsEntitiesState, fromViews.getViewById(id));

  export const { selectAll: getAllViews } = fromViews.viewsAdapter.getSelectors(
    getViewsEntitiesState
  );

  export const getDisplayViewDialog = createSelector(
    getViewsEntitiesState,
    state => state.displayViewDialog
  );

  export const getLastViewChanged = createSelector(
    getViewsEntitiesState,
    state => state.lastViewChanged
  );

  export const getDataLoaded = createSelector(
    getViewsEntitiesState,
    state => state.dataLoaded
  );

  export const getDataLoadedAndViews = createSelector(
    getDataLoaded,
    getAllViews,
    (dataLoaded, views) => ({
      dataLoaded,
      views,
    })
  );

  export const getViewById = (id: number) =>
    createSelector(getViewsEntitiesState, fromViews.getViewById(id));
}
