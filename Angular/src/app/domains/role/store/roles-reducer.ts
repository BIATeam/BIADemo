import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { loadAllSuccess, loadMemberRoles, loadMemberRolesSuccess, loadSuccess } from './roles-actions';
import { Role } from '../model/role';

// This adapter will allow is to manipulate roles (mostly CRUD operations)
export const rolesAdapter = createEntityAdapter<Role>({
  selectId: (role: Role) => role.id,
  sortComparer: false
});

// ------------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Role> {
//   ids: string[] | number[];
//   entities: { [id: string]: Role };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<Role> {
  // additional props here
  memberRoles: Role[] | null;
}

export const INIT_STATE: State = rolesAdapter.getInitialState({
  // additional props default values here
  memberRoles: null
});

export const roleReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { roles }) => rolesAdapter.setAll(roles, state)),
  on(loadSuccess, (state, { role }) => rolesAdapter.upsertOne(role, state)),
  on(loadMemberRoles, (state, { }) => {
    return { ...state, memberRoles: null };
  }),
  on(loadMemberRolesSuccess, (state, { roles }) => {
    return { ...state, memberRoles: roles };
  })
);

export const getRoleById = (id: number) => (state: State) => state.entities[id];
