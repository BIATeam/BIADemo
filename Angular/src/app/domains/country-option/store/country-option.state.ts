import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../country-option.contants';
import * as fromCountryOptions from './country-options-reducer';

export interface CountryOptionsState {
  countryOptions: fromCountryOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(
  state: CountryOptionsState | undefined,
  action: Action
) {
  return combineReducers({
    countryOptions: fromCountryOptions.countryOptionReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getCountriesState =
  createFeatureSelector<CountryOptionsState>(storeKey);

export const getCountryOptionsEntitiesState = createSelector(
  getCountriesState,
  state => state.countryOptions
);

export const { selectAll: getAllCountryOptions } =
  fromCountryOptions.countryOptionsAdapter.getSelectors(
    getCountryOptionsEntitiesState
  );

export const getCountryOptionById = (id: number) =>
  createSelector(
    getCountryOptionsEntitiesState,
    fromCountryOptions.getCountryOptionById(id)
  );
