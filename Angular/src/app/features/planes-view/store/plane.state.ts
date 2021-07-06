import * as fromPlanes from './planes-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface PlanesState {
  planes: fromPlanes.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: PlanesState | undefined, action: Action) {
  return combineReducers({
    planes: fromPlanes.planeReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getPlanesState = createFeatureSelector<PlanesState>('planes-mode-view');

export const getPlanesEntitiesState = createSelector(
  getPlanesState,
  (state) => state.planes
);

export const getPlanesTotalCount = createSelector(
  getPlanesEntitiesState,
  (state) => state.totalCount
);

export const getCurrentPlane = createSelector(
  getPlanesEntitiesState,
  (state) => state.currentPlane
);

export const getLastLazyLoadEvent = createSelector(
  getPlanesEntitiesState,
  (state) => state.lastLazyLoadEvent
);

export const getPlaneLoadingGet = createSelector(
  getPlanesEntitiesState,
  (state) => state.loadingGet
);

export const getPlaneLoadingGetAll = createSelector(
  getPlanesEntitiesState,
  (state) => state.loadingGetAll
);

export const { selectAll: getAllPlanes } = fromPlanes.planesAdapter.getSelectors(getPlanesEntitiesState);

export const getPlaneById = (id: number) =>
  createSelector(
    getPlanesEntitiesState,
    fromPlanes.getPlaneById(id)
  );
