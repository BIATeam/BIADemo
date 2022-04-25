import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { UserFromDirectory } from '../model/user-from-Directory';
import { DomaineUsersFromDirectoryActions } from './users-from-Directory-actions';

// This adapter will allow is to manipulate users (mostly CRUD operations)
export const usersFromDirectoryAdapter = createEntityAdapter<UserFromDirectory>({
  selectId: (user: UserFromDirectory) => user.domain + '\\' + user.login,
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

export interface State extends EntityState<UserFromDirectory> {
  changeCount : number;
  // additional props here
}

export const INIT_STATE: State = usersFromDirectoryAdapter.getInitialState({
  // additional props default values here
  changeCount:0
});

export const userFromDirectoryReducers = createReducer<State>(
  INIT_STATE,
  on(DomaineUsersFromDirectoryActions.loadAllSuccess, (state, { users }) => usersFromDirectoryAdapter.setAll(users, state)),
  on(DomaineUsersFromDirectoryActions.addFromDirectory, (state, { }) => {
    return { ...state, changeCount: state.changeCount + 1 };
  }),
);