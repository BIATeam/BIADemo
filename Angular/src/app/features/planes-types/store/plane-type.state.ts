import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { planeTypeCRUDConfiguration } from '../plane-type.constants';
import * as fromPlanesTypes from './planes-types-reducer';

export namespace FeaturePlanesTypesStore {
  export interface PlanesTypesState {
    planesTypes: fromPlanesTypes.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(
    state: PlanesTypesState | undefined,
    action: Action
  ) {
    return combineReducers({
      planesTypes: fromPlanesTypes.planeTypeReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getPlanesTypesState = createFeatureSelector<PlanesTypesState>(
    planeTypeCRUDConfiguration.storeKey
  );

  export const getPlanesTypesEntitiesState = createSelector(
    getPlanesTypesState,
    state => state.planesTypes
  );

  export const getPlanesTypesTotalCount = createSelector(
    getPlanesTypesEntitiesState,
    state => state.totalCount
  );

  export const getCurrentPlaneType = createSelector(
    getPlanesTypesEntitiesState,
    state => state.currentPlaneType
  );

  export const getLastLazyLoadEvent = createSelector(
    getPlanesTypesEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getPlaneTypeLoadingGet = createSelector(
    getPlanesTypesEntitiesState,
    state => state.loadingGet
  );

  export const getPlaneTypeLoadingGetAll = createSelector(
    getPlanesTypesEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllPlanesTypes } =
    fromPlanesTypes.planesTypesAdapter.getSelectors(
      getPlanesTypesEntitiesState
    );

  export const getPlaneTypeById = (id: number) =>
    createSelector(
      getPlanesTypesEntitiesState,
      fromPlanesTypes.getPlaneTypeById(id)
    );
}
