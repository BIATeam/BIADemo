import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { annoucementCRUDConfiguration } from '../annoucement.constants';
import { Annoucement } from '../model/annoucement';
import * as fromAnnoucements from './annoucements-reducer';

export namespace FeatureAnnoucementsStore {
  export interface AnnoucementsState {
    annoucements: fromAnnoucements.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(
    state: AnnoucementsState | undefined,
    action: Action
  ) {
    return combineReducers({
      annoucements: fromAnnoucements.annoucementReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getAnnoucementsState = createFeatureSelector<AnnoucementsState>(
    annoucementCRUDConfiguration.storeKey
  );

  export const getAnnoucementsEntitiesState = createSelector(
    getAnnoucementsState,
    state => state.annoucements
  );

  export const getAnnoucementsTotalCount = createSelector(
    getAnnoucementsEntitiesState,
    state => state.totalCount
  );

  export const getCurrentAnnoucement = createSelector(
    getAnnoucementsEntitiesState,
    state => state.currentItem ?? <Annoucement>{}
  );

  export const getCurrentAnnoucementHistorical = createSelector(
    getAnnoucementsEntitiesState,
    state => state.currentItemHistorical
  );

  export const getLastLazyLoadEvent = createSelector(
    getAnnoucementsEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getAnnoucementLoadingGet = createSelector(
    getAnnoucementsEntitiesState,
    state => state.loadingGet
  );

  export const getAnnoucementLoadingGetAll = createSelector(
    getAnnoucementsEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllAnnoucements } =
    fromAnnoucements.annoucementsAdapter.getSelectors(
      getAnnoucementsEntitiesState
    );

  export const getAnnoucementById = (id: number) =>
    createSelector(
      getAnnoucementsEntitiesState,
      fromAnnoucements.getAnnoucementById(id)
    );
}
