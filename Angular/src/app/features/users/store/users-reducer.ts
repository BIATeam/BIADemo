import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { loadSuccess, loadAllByPostSuccess } from './users-actions';
import { LazyLoadEvent } from 'primeng/api';
import { User } from 'src/app/domains/user/model/user';

// This adapter will allow is to manipulate users (mostly CRUD operations)
export const usersAdapter = createEntityAdapter<User>({
  selectId: (user: User) => user.id,
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
  totalCount: number;
  currentUser: User;
  lastLazyLoadEvent: LazyLoadEvent;
}

export const INIT_STATE: State = usersAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentUser: <User>{},
  lastLazyLoadEvent: <LazyLoadEvent>{}
});

export const userReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = usersAdapter.setAll(result.data, state);
    stateUpdated.currentUser = <User>{};
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    return stateUpdated;
  }),
  on(loadSuccess, (state, { user }) => {
    return { ...state, currentUser: user };
  })
);

export const getUserById = (id: number) => (state: State) => state.entities[id];
