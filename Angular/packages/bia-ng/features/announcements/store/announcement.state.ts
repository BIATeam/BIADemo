import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { Announcement } from 'packages/bia-ng/models/public-api';
import { announcementCRUDConfiguration } from '../announcement.constants';
import * as fromAnnouncements from './announcements-reducer';

export namespace FeatureAnnouncementsStore {
  export interface AnnouncementsState {
    announcements: fromAnnouncements.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(
    state: AnnouncementsState | undefined,
    action: Action
  ) {
    return combineReducers({
      announcements: fromAnnouncements.announcementReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getAnnouncementsState =
    createFeatureSelector<AnnouncementsState>(
      announcementCRUDConfiguration.storeKey
    );

  export const getAnnouncementsEntitiesState = createSelector(
    getAnnouncementsState,
    state => state.announcements
  );

  export const getAnnouncementsTotalCount = createSelector(
    getAnnouncementsEntitiesState,
    state => state.totalCount
  );

  export const getCurrentAnnouncement = createSelector(
    getAnnouncementsEntitiesState,
    state => state.currentItem ?? <Announcement>{}
  );

  export const getCurrentAnnouncementHistorical = createSelector(
    getAnnouncementsEntitiesState,
    state => state.currentItemHistorical
  );

  export const getLastLazyLoadEvent = createSelector(
    getAnnouncementsEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getAnnouncementLoadingGet = createSelector(
    getAnnouncementsEntitiesState,
    state => state.loadingGet
  );

  export const getAnnouncementLoadingGetAll = createSelector(
    getAnnouncementsEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllAnnouncements } =
    fromAnnouncements.announcementsAdapter.getSelectors(
      getAnnouncementsEntitiesState
    );

  export const getAnnouncementById = (id: number) =>
    createSelector(
      getAnnouncementsEntitiesState,
      fromAnnouncements.getAnnouncementById(id)
    );
}
