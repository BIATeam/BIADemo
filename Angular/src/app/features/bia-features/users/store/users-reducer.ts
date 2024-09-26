import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { User } from '../model/user';
import { FeatureUsersActions } from './users-actions';

// This adapter will allow is to manipulate users (mostly CRUD operations)
export const usersAdapter = createEntityAdapter<User>({
  selectId: (user: User) => user.id,
  sortComparer: false,
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
  lastLazyLoadEvent: TableLazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State = usersAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentUser: <User>{},
  lastLazyLoadEvent: <TableLazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export const userReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureUsersActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureUsersActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(FeatureUsersActions.save, state => {
    return { ...state, loadingGetAll: true };
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
  on(FeatureUsersActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getUserById = (id: number) => (state: State) => state.entities[id];
