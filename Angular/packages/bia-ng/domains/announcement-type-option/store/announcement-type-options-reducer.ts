import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { DomainAnnouncementTypeOptionsActions } from './announcement-type-options-actions';

// This adapter will allow is to manipulate announcementTypes (mostly CRUD operations)
export const announcementTypeOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (announcementType: OptionDto) => announcementType.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<AnnouncementType> {
//   ids: string[] | number[];
//   entities: { [id: string]: AnnouncementType };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export type State = EntityState<OptionDto>;

export const INIT_STATE: State = announcementTypeOptionsAdapter.getInitialState({
  // additional props default values here
});

export const announcementTypeOptionReducers = createReducer<State>(
  INIT_STATE,
  on(
    DomainAnnouncementTypeOptionsActions.loadAllSuccess,
    (state, { announcementTypes }) =>
      announcementTypeOptionsAdapter.setAll(announcementTypes, state)
  )
);

export const getAnnouncementTypeOptionById = (id: number) => (state: State) =>
  state.entities[id];
