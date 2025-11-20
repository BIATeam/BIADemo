import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { HistoricalEntryDto } from 'packages/bia-ng/models/dto/historical-entry-dto';
import {
  CrudState,
  DEFAULT_CRUD_STATE,
} from 'packages/bia-ng/models/public-api';
import { Annoucement } from '../model/annoucement';
import { FeatureAnnoucementsActions } from './annoucements-actions';

// This adapter will allow is to manipulate annoucements (mostly CRUD operations)
export const annoucementsAdapter = createEntityAdapter<Annoucement>({
  selectId: (annoucement: Annoucement) => annoucement.id,
  sortComparer: false,
});

export interface State
  extends CrudState<Annoucement>,
    EntityState<Annoucement> {
  // additional props here
  currentItemHistorical: HistoricalEntryDto[];
}

export const INIT_STATE: State = annoucementsAdapter.getInitialState({
  ...DEFAULT_CRUD_STATE(),
  currentItemHistorical: [],
  // additional props default values here
});

export const annoucementReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureAnnoucementsActions.clearAll, state => {
    const stateUpdated = annoucementsAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureAnnoucementsActions.clearCurrent, state => {
    return {
      ...state,
      currentItem: <Annoucement>{},
      currentItemHistorical: [],
      actives: [],
    };
  }),
  on(FeatureAnnoucementsActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureAnnoucementsActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(
    FeatureAnnoucementsActions.loadAllByPostSuccess,
    (state, { result, event }) => {
      const stateUpdated = annoucementsAdapter.setAll(result.data, state);
      stateUpdated.totalCount = result.totalCount;
      stateUpdated.lastLazyLoadEvent = event;
      stateUpdated.loadingGetAll = false;
      return stateUpdated;
    }
  ),
  on(FeatureAnnoucementsActions.loadSuccess, (state, { annoucement }) => {
    return { ...state, currentItem: annoucement, loadingGet: false };
  }),
  on(
    FeatureAnnoucementsActions.loadHistoricalSuccess,
    (state, { historical }) => {
      return { ...state, currentItemHistorical: historical };
    }
  ),
  on(FeatureAnnoucementsActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getAnnoucementById = (id: number) => (state: State) =>
  state.entities[id];
