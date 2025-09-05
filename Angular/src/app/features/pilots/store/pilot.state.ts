import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { Pilot } from '../model/pilot';
import { pilotCRUDConfiguration } from '../pilot.constants';
import * as fromPilots from './pilots-reducer';

export namespace FeaturePilotsStore {
  export interface PilotsState {
    pilots: fromPilots.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(state: PilotsState | undefined, action: Action) {
    return combineReducers({
      pilots: fromPilots.pilotReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getPilotsState = createFeatureSelector<PilotsState>(
    pilotCRUDConfiguration.storeKey
  );

  export const getPilotsEntitiesState = createSelector(
    getPilotsState,
    state => state.pilots
  );

  export const getPilotsTotalCount = createSelector(
    getPilotsEntitiesState,
    state => state.totalCount
  );

  export const getCurrentPilot = createSelector(
    getPilotsEntitiesState,
    state => state.currentItem ?? <Pilot>{ id: '' }
  );

  export const getLastLazyLoadEvent = createSelector(
    getPilotsEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getPilotLoadingGet = createSelector(
    getPilotsEntitiesState,
    state => state.loadingGet
  );

  export const getPilotLoadingGetAll = createSelector(
    getPilotsEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllPilots } =
    fromPilots.pilotsAdapter.getSelectors(getPilotsEntitiesState);

  export const getPilotById = (id: number) =>
    createSelector(getPilotsEntitiesState, fromPilots.getPilotById(id));
}
