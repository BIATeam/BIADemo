import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { FeatureUsersActions } from './users-actions';
import { LazyLoadEvent } from 'primeng/api';
import { User } from '../model/user';

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
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State = usersAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentUser: <User>{},
  lastLazyLoadEvent: <LazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export const userReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureUsersActions.loadAllByPost, (state, { event }) => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureUsersActions.load, (state) => {
    return { ...state, loadingGet: true };
  }),
  on(FeatureUsersActions.loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = usersAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(FeatureUsersActions.loadSuccess, (state, { user }) => {
    return { ...state, currentUser: user, loadingGet: false };
  }),
  on(FeatureUsersActions.failure, (state, { error }) => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  }),
);

export const getUserById = (id: number) => (state: State) => state.entities[id];
