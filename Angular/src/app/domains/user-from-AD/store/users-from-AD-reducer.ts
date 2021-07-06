import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { UserFromAD } from '../model/user-from-AD';
import { loadAllSuccess } from './users-from-AD-actions';

// This adapter will allow is to manipulate users (mostly CRUD operations)
export const usersFromADAdapter = createEntityAdapter<UserFromAD>({
  selectId: (user: UserFromAD) => user.domain + '\\' + user.login,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<User> {
//   ids: string[] | number[];
//   entities: { [id: string]: User };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<UserFromAD> {
  // additional props here
}

export const INIT_STATE: State = usersFromADAdapter.getInitialState({
  // additional props default values here
});

export const userFromADReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { users }) => usersFromADAdapter.setAll(users, state)),
);
