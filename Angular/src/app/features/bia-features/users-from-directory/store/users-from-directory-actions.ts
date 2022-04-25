import { createAction, props } from '@ngrx/store';
import { UserFilter } from '../model/user-filter';
import { UserFromDirectory } from '../model/user-from-Directory';



export namespace FeatureUsersFromDirectoryActions
{ 
  export const loadAllByFilter = createAction(
    '[Feature Users From AD] Load all in AD by filter',
    props<{ userFilter: UserFilter }>()
  );

  export const loadAllSuccess = createAction('[Feature Users From AD] Load all success', props<{ users: UserFromDirectory[] }>());

  export const failure = createAction('[Feature Users From AD] Failure', props<{ error: any }>());

  export const addFromDirectory = createAction('[Feature Users From AD] CreateUserFromDirectory', props<{ usersFromDirectory: UserFromDirectory[] }>());
}