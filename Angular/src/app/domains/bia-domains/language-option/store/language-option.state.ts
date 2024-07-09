import * as fromLanguageOptions from './language-options-reducer';
import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';

export interface LanguageOptionsState {
  languageOptions: fromLanguageOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(
  state: LanguageOptionsState | undefined,
  action: Action
) {
  return combineReducers({
    languageOptions: fromLanguageOptions.languageOptionReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getLanguagesState = createFeatureSelector<LanguageOptionsState>(
  'domain-language-options'
);

export const getLanguageOptionsEntitiesState = createSelector(
  getLanguagesState,
  state => state.languageOptions
);

export const { selectAll: getAllLanguageOptions } =
  fromLanguageOptions.languageOptionsAdapter.getSelectors(
    getLanguageOptionsEntitiesState
  );

export const getLanguageOptionById = (id: number) =>
  createSelector(
    getLanguageOptionsEntitiesState,
    fromLanguageOptions.getLanguageOptionById(id)
  );
