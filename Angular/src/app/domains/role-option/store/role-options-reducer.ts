import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { loadAllRoleOptionsSuccess } from './role-options-actions';

// This adapter will allow is to manipulate roles (mostly CRUD operations)
export const roleOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (role: OptionDto) => role.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Role> {
//   ids: string[] | number[];
//   entities: { [id: string]: Role };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<OptionDto> {
  // additional props here
}

export const INIT_STATE: State = roleOptionsAdapter.getInitialState({
  // additional props default values here
});

export const roleOptionReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllRoleOptionsSuccess, (state, { roles }) => roleOptionsAdapter.setAll(roles, state)),
);

export const getRoleOptionById = (id: number) => (state: State) => state.entities[id];


















