import { createAction, props } from '@ngrx/store';
import { UserFilter } from 'src/app/domains/user-from-AD/model/user-filter';
import { UserFromDirectory } from '../model/user-from-AD';



export namespace DomaineUsersFromADActions
{ 
  export const loadAllByFilter = createAction(
    '[Domain Users From AD] Load all in AD by filter',
    props<{ userFilter: UserFilter }>()
  );

  export const loadAllSuccess = createAction('[Domain Users From AD] Load all success', props<{ users: UserFromDirectory[] }>());

  export const failure = createAction('[Domain Users From AD] Failure', props<{ error: any }>());

  export const addFromDirectory = createAction('[Domain Users From AD] CreateUserFromAD', props<{ usersFromDirectory: UserFromDirectory[] }>());

  export const addFromDirectorySuccess = createAction('[Domain Users From AD] Create User From AD success', props<{ users: UserFromDirectory[] }>());
}