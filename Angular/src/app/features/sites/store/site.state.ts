import * as fromSites from './sites-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface SitesState {
  sites: fromSites.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: SitesState | undefined, action: Action) {
  return combineReducers({
    sites: fromSites.siteReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getSitesState = createFeatureSelector<SitesState>('sites');

export const getSitesEntitiesState = createSelector(
  getSitesState,
  (state) => state.sites
);

export const getSitesTotalCount = createSelector(
  getSitesEntitiesState,
  (state) => state.totalCount
);

export const getCurrentSite = createSelector(
  getSitesEntitiesState,
  (state) => state.currentSite
);

export const getLastLazyLoadEvent = createSelector(
  getSitesEntitiesState,
  (state) => state.lastLazyLoadEvent
);

export const getSiteLoadingGet = createSelector(
  getSitesEntitiesState,
  (state) => state.loadingGet
);

export const getSiteLoadingGetAll = createSelector(
  getSitesEntitiesState,
  (state) => state.loadingGetAll
);

export const getDisplayEditDialog = createSelector(
  getSitesEntitiesState,
  (state) => state.displayEditDialog
);

export const getDisplayNewDialog = createSelector(
  getSitesEntitiesState,
  (state) => state.displayNewDialog
);

export const { selectAll: getAllSites } = fromSites.sitesAdapter.getSelectors(getSitesEntitiesState);

export const getSiteById = (id: number) =>
  createSelector(
    getSitesEntitiesState,
    fromSites.getSiteById(id)
  );
