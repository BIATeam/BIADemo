import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { DomainSiteOptionsActions } from './site-options-actions';

// This adapter will allow is to manipulate sites (mostly CRUD operations)
export const siteOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (site: OptionDto) => site.id,
  sortComparer: false,
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

export type State = EntityState<OptionDto>;

export const INIT_STATE: State = siteOptionsAdapter.getInitialState({
  // additional props default values here
});

export const siteOptionReducers = createReducer<State>(
  INIT_STATE,
  on(DomainSiteOptionsActions.loadAllSuccess, (state, { sites }) =>
    siteOptionsAdapter.setAll(sites, state)
  )
  // on(loadSuccess, (state, { site }) => siteOptionsAdapter.upsertOne(site, state))
);

export const getSiteOptionById = (id: number) => (state: State) =>
  state.entities[id];
