import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { loadAllPermissionOptionsSuccess } from './permission-options-actions';

// This adapter will allow is to manipulate permissions (mostly CRUD operations)
export const permissionOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (permission: OptionDto) => permission.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Permission> {
//   ids: string[] | number[];
//   entities: { [id: string]: Permission };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<OptionDto> {
  // additional props here
}

export const INIT_STATE: State = permissionOptionsAdapter.getInitialState({
  // additional props default values here
});

export const permissionOptionReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllPermissionOptionsSuccess, (state, { permissions }) => permissionOptionsAdapter.setAll(permissions, state)),
);

export const getPermissionOptionById = (id: number) => (state: State) => state.entities[id];


















