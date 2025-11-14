import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { bannerMessageCRUDConfiguration } from '../banner-message.constants';
import { BannerMessage } from '../model/banner-message';
import * as fromBannerMessages from './banner-messages-reducer';

export namespace FeatureBannerMessagesStore {
  export interface BannerMessagesState {
    bannerMessages: fromBannerMessages.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(
    state: BannerMessagesState | undefined,
    action: Action
  ) {
    return combineReducers({
      bannerMessages: fromBannerMessages.bannerMessageReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getBannerMessagesState =
    createFeatureSelector<BannerMessagesState>(
      bannerMessageCRUDConfiguration.storeKey
    );

  export const getBannerMessagesEntitiesState = createSelector(
    getBannerMessagesState,
    state => state.bannerMessages
  );

  export const getBannerMessagesTotalCount = createSelector(
    getBannerMessagesEntitiesState,
    state => state.totalCount
  );

  export const getCurrentBannerMessage = createSelector(
    getBannerMessagesEntitiesState,
    state => state.currentItem ?? <BannerMessage>{}
  );

  export const getCurrentBannerMessageHistorical = createSelector(
    getBannerMessagesEntitiesState,
    state => state.currentItemHistorical
  );

  export const getLastLazyLoadEvent = createSelector(
    getBannerMessagesEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getBannerMessageLoadingGet = createSelector(
    getBannerMessagesEntitiesState,
    state => state.loadingGet
  );

  export const getBannerMessageLoadingGetAll = createSelector(
    getBannerMessagesEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllBannerMessages } =
    fromBannerMessages.bannerMessagesAdapter.getSelectors(
      getBannerMessagesEntitiesState
    );

  export const getBannerMessageById = (id: number) =>
    createSelector(
      getBannerMessagesEntitiesState,
      fromBannerMessages.getBannerMessageById(id)
    );
}
