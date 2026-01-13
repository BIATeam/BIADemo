import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../announcement-type-option.constants';
import * as fromAnnouncementTypeOptions from './announcement-type-options-reducer';

export namespace DomainAnnouncementTypeOptionsStore {
  export interface AnnouncementTypeOptionsState {
    announcementTypeOptions: fromAnnouncementTypeOptions.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(
    state: AnnouncementTypeOptionsState | undefined,
    action: Action
  ) {
    return combineReducers({
      announcementTypeOptions:
        fromAnnouncementTypeOptions.announcementTypeOptionReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getAnnouncementTypesState =
    createFeatureSelector<AnnouncementTypeOptionsState>(storeKey);

  export const getAnnouncementTypeOptionsEntitiesState = createSelector(
    getAnnouncementTypesState,
    state => state.announcementTypeOptions
  );

  export const { selectAll: getAllAnnouncementTypeOptions } =
    fromAnnouncementTypeOptions.announcementTypeOptionsAdapter.getSelectors(
      getAnnouncementTypeOptionsEntitiesState
    );

  export const getAnnouncementTypeOptionById = (id: number) =>
    createSelector(
      getAnnouncementTypeOptionsEntitiesState,
      fromAnnouncementTypeOptions.getAnnouncementTypeOptionById(id)
    );
}
