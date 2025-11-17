import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../banner-message-type-option.constants';
import * as fromBannerMessageTypeOptions from './banner-message-type-options-reducer';

export namespace DomainBannerMessageTypeOptionsStore {
  export interface BannerMessageTypeOptionsState {
    bannerMessageTypeOptions: fromBannerMessageTypeOptions.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(
    state: BannerMessageTypeOptionsState | undefined,
    action: Action
  ) {
    return combineReducers({
      bannerMessageTypeOptions:
        fromBannerMessageTypeOptions.bannerMessageTypeOptionReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getBannerMessageTypesState =
    createFeatureSelector<BannerMessageTypeOptionsState>(storeKey);

  export const getBannerMessageTypeOptionsEntitiesState = createSelector(
    getBannerMessageTypesState,
    state => state.bannerMessageTypeOptions
  );

  export const { selectAll: getAllBannerMessageTypeOptions } =
    fromBannerMessageTypeOptions.bannerMessageTypeOptionsAdapter.getSelectors(
      getBannerMessageTypeOptionsEntitiesState
    );

  export const getBannerMessageTypeOptionById = (id: number) =>
    createSelector(
      getBannerMessageTypeOptionsEntitiesState,
      fromBannerMessageTypeOptions.getBannerMessageTypeOptionById(id)
    );
}
