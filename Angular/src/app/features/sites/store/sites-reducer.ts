import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Site } from '../model/site/site';
import { SiteInfo } from '../model/site/site-info';
import { LazyLoadEvent } from 'primeng/api';
import {
  loadSuccess,
  loadAllByPostSuccess,
  loadAllByPost,
  load,
  openDialogEdit,
  closeDialogEdit,
  openDialogNew,
  closeDialogNew,
  failure
} from './sites-actions';

// This adapter will allow is to manipulate sites (mostly CRUD operations)
export const sitesAdapter = createEntityAdapter<SiteInfo>({
  selectId: (site: SiteInfo) => site.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Site> {
//   ids: string[] | number[];
//   entities: { [id: string]: Site };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<SiteInfo> {
  // additional props here
  totalCount: number;
  currentSite: Site;
  lastLazyLoadEvent: LazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
  displayEditDialog: boolean;
  displayNewDialog: boolean;
}

export const INIT_STATE: State = sitesAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentSite: <Site>{},
  lastLazyLoadEvent: <LazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
  displayEditDialog: false,
  displayNewDialog: false
});

export const siteReducers = createReducer<State>(
  INIT_STATE,
  on(openDialogNew, (state) => {
    return { ...state, displayNewDialog: true };
  }),
  on(closeDialogNew, (state) => {
    return { ...state, displayNewDialog: false };
  }),
  on(openDialogEdit, (state) => {
    return { ...state, displayEditDialog: true };
  }),
  on(closeDialogEdit, (state) => {
    return { ...state, displayEditDialog: false };
  }),
  on(loadAllByPost, (state, { event }) => {
    return { ...state, loadingGetAll: true };
  }),
  on(load, (state) => {
    return { ...state, loadingGet: true };
  }),
  on(loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = sitesAdapter.setAll(result.data, state);
    stateUpdated.currentSite = <Site>{};
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(loadSuccess, (state, { site }) => {
    return { ...state, currentSite: site, loadingGet: false };
  }),
  on(failure, (state, { error }) => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getSiteById = (id: number) => (state: State) => state.entities[id];
