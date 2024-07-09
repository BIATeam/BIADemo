import { createAction, props } from '@ngrx/store';
import { Team } from '../model/team';
import { storeKey } from '../team.contants';

export namespace DomainTeamsActions {
  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ teams: Team[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const setDefaultTeam = createAction(
    '[' + storeKey + '] Set default team',
    props<{ teamTypeId: number; teamId: number }>()
  );

  export const setDefaultTeamSuccess = createAction(
    '[' + storeKey + '] Set default team success',
    props<{ teamTypeId: number; teamId: number }>()
  );

  export const setDefaultRoles = createAction(
    '[' + storeKey + '] Set default role',
    props<{ teamId: number; roleIds: number[] }>()
  );

  export const setDefaultRolesSuccess = createAction(
    '[' + storeKey + '] Set default role success',
    props<{ teamId: number; roleIds: number[] }>()
  );
}
