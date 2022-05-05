import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { DomainTeamOptionsActions } from './team-options-actions';

// This adapter will allow is to manipulate teams (mostly CRUD operations)
export const teamOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (team: OptionDto) => team.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Team> {
//   ids: string[] | number[];
//   entities: { [id: string]: Team };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<OptionDto> {
  // additional props here
}

export const INIT_STATE: State = teamOptionsAdapter.getInitialState({
  // additional props default values here
});

export const teamOptionReducers = createReducer<State>(
  INIT_STATE,
  on(DomainTeamOptionsActions.loadAllSuccess, (state, { teams }) => teamOptionsAdapter.setAll(teams, state)),
  // on(loadSuccess, (state, { team }) => teamOptionsAdapter.upsertOne(team, state))
);

export const getTeamOptionById = (id: number) => (state: State) => state.entities[id];


















