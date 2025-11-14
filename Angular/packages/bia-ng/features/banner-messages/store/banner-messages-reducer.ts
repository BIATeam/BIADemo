import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { HistoricalEntryDto } from 'packages/bia-ng/models/dto/historical-entry-dto';
import {
  CrudState,
  DEFAULT_CRUD_STATE,
} from 'packages/bia-ng/models/public-api';
import { BannerMessage } from '../model/banner-message';
import { FeatureBannerMessagesActions } from './banner-messages-actions';

// This adapter will allow is to manipulate bannerMessages (mostly CRUD operations)
export const bannerMessagesAdapter = createEntityAdapter<BannerMessage>({
  selectId: (bannerMessage: BannerMessage) => bannerMessage.id,
  sortComparer: false,
});

export interface State
  extends CrudState<BannerMessage>,
    EntityState<BannerMessage> {
  // additional props here
  currentItemHistorical: HistoricalEntryDto[];
}

export const INIT_STATE: State = bannerMessagesAdapter.getInitialState({
  ...DEFAULT_CRUD_STATE(),
  currentItemHistorical: [],
  // additional props default values here
});

export const bannerMessageReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureBannerMessagesActions.clearAll, state => {
    const stateUpdated = bannerMessagesAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureBannerMessagesActions.clearCurrent, state => {
    return {
      ...state,
      currentItem: <BannerMessage>{},
      currentItemHistorical: [],
    };
  }),
  on(FeatureBannerMessagesActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureBannerMessagesActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(
    FeatureBannerMessagesActions.loadAllByPostSuccess,
    (state, { result, event }) => {
      const stateUpdated = bannerMessagesAdapter.setAll(result.data, state);
      stateUpdated.totalCount = result.totalCount;
      stateUpdated.lastLazyLoadEvent = event;
      stateUpdated.loadingGetAll = false;
      return stateUpdated;
    }
  ),
  on(FeatureBannerMessagesActions.loadSuccess, (state, { bannerMessage }) => {
    return { ...state, currentItem: bannerMessage, loadingGet: false };
  }),
  on(
    FeatureBannerMessagesActions.loadHistoricalSuccess,
    (state, { historical }) => {
      return { ...state, currentItemHistorical: historical };
    }
  ),
  on(FeatureBannerMessagesActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getBannerMessageById = (id: number) => (state: State) =>
  state.entities[id];
