import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { loadAllSuccess } from './users-actions';
import { User } from '../model/user';

// This adapter will allow is to manipulate users (mostly CRUD operations)
export const usersAdapter = createEntityAdapter<User>({
  selectId: (user: User) => user.guid,
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

export interface State extends EntityState<User> {
  // additional props here
}

export const INIT_STATE: State = usersAdapter.getInitialState({
  // additional props default values here
});

export const userReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { users }) => usersAdapter.setAll(users, state)),
);

export const getUserById = (id: number) => (state: State) => state.entities[id];
