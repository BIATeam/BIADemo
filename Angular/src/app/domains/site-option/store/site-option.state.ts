import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../site-option.contants';
import * as fromSiteOptions from './site-options-reducer';

export interface SiteOptionsState {
  siteOptions: fromSiteOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(
  state: SiteOptionsState | undefined,
  action: Action
) {
  return combineReducers({
    siteOptions: fromSiteOptions.siteOptionReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getSitesState =
  createFeatureSelector<SiteOptionsState>(storeKey);

export const getSiteOptionsEntitiesState = createSelector(
  getSitesState,
  state => state.siteOptions
);

export const { selectAll: getAllSiteOptions } =
  fromSiteOptions.siteOptionsAdapter.getSelectors(
    getSiteOptionsEntitiesState
  );

export const getSiteOptionById = (id: number) =>
  createSelector(
    getSiteOptionsEntitiesState,
    fromSiteOptions.getSiteOptionById(id)
  );
