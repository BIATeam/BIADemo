import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../annoucement-type-option.constants';
import * as fromAnnoucementTypeOptions from './annoucement-type-options-reducer';

export namespace DomainAnnoucementTypeOptionsStore {
  export interface AnnoucementTypeOptionsState {
    annoucementTypeOptions: fromAnnoucementTypeOptions.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(
    state: AnnoucementTypeOptionsState | undefined,
    action: Action
  ) {
    return combineReducers({
      annoucementTypeOptions:
        fromAnnoucementTypeOptions.annoucementTypeOptionReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getAnnoucementTypesState =
    createFeatureSelector<AnnoucementTypeOptionsState>(storeKey);

  export const getAnnoucementTypeOptionsEntitiesState = createSelector(
    getAnnoucementTypesState,
    state => state.annoucementTypeOptions
  );

  export const { selectAll: getAllAnnoucementTypeOptions } =
    fromAnnoucementTypeOptions.annoucementTypeOptionsAdapter.getSelectors(
      getAnnoucementTypeOptionsEntitiesState
    );

  export const getAnnoucementTypeOptionById = (id: number) =>
    createSelector(
      getAnnoucementTypeOptionsEntitiesState,
      fromAnnoucementTypeOptions.getAnnoucementTypeOptionById(id)
    );
}
