import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { DomainBannerMessageTypeOptionsActions } from './banner-message-type-options-actions';

// This adapter will allow is to manipulate bannerMessageTypes (mostly CRUD operations)
export const bannerMessageTypeOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (bannerMessageType: OptionDto) => bannerMessageType.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<BannerMessageType> {
//   ids: string[] | number[];
//   entities: { [id: string]: BannerMessageType };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export type State = EntityState<OptionDto>;

export const INIT_STATE: State =
  bannerMessageTypeOptionsAdapter.getInitialState({
    // additional props default values here
  });

export const bannerMessageTypeOptionReducers = createReducer<State>(
  INIT_STATE,
  on(
    DomainBannerMessageTypeOptionsActions.loadAllSuccess,
    (state, { bannerMessageTypes }) =>
      bannerMessageTypeOptionsAdapter.setAll(bannerMessageTypes, state)
  )
);

export const getBannerMessageTypeOptionById = (id: number) => (state: State) =>
  state.entities[id];
