import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import {
  Announcement,
  CrudState,
  DEFAULT_CRUD_STATE,
  HistoricalEntryDto,
} from 'packages/bia-ng/models/public-api';
import { FeatureAnnouncementsActions } from './announcements-actions';

// This adapter will allow is to manipulate announcements (mostly CRUD operations)
export const announcementsAdapter = createEntityAdapter<Announcement>({
  selectId: (announcement: Announcement) => announcement.id,
  sortComparer: false,
});

export interface State
  extends CrudState<Announcement>, EntityState<Announcement> {
  // additional props here
  currentItemHistorical: HistoricalEntryDto[];
}

export const INIT_STATE: State = announcementsAdapter.getInitialState({
  ...DEFAULT_CRUD_STATE(),
  currentItemHistorical: [],
  // additional props default values here
});

export const announcementReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureAnnouncementsActions.clearAll, state => {
    const stateUpdated = announcementsAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureAnnouncementsActions.clearCurrent, state => {
    return {
      ...state,
      currentItem: <Announcement>{},
      currentItemHistorical: [],
      actives: [],
    };
  }),
  on(FeatureAnnouncementsActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureAnnouncementsActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(
    FeatureAnnouncementsActions.loadAllByPostSuccess,
    (state, { result, event }) => {
      const stateUpdated = announcementsAdapter.setAll(result.data, state);
      stateUpdated.totalCount = result.totalCount;
      stateUpdated.lastLazyLoadEvent = event;
      stateUpdated.loadingGetAll = false;
      return stateUpdated;
    }
  ),
  on(FeatureAnnouncementsActions.loadSuccess, (state, { announcement }) => {
    return { ...state, currentItem: announcement, loadingGet: false };
  }),
  on(
    FeatureAnnouncementsActions.loadHistoricalSuccess,
    (state, { historical }) => {
      return { ...state, currentItemHistorical: historical };
    }
  ),
  on(FeatureAnnouncementsActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getAnnouncementById = (id: number) => (state: State) =>
  state.entities[id];
