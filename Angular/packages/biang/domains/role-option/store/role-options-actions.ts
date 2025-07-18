import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'biang/models';

export namespace DomainRoleOptionsActions {
  export const loadAll = createAction(
    '[Domain Role Options] Load all',
    props<{ teamTypeId: number }>()
  );

  export const loadAllSuccess = createAction(
    '[Domain Role Options] Load all success',
    props<{ roles: OptionDto[] }>()
  );
  /*
    export const load = createAction('[Domain Role Options] Load', props<{ id: number }>());

    export const loadSuccess = createAction('[Domain Role Options] Load success', props<{ role: RoleOption }>());
    */
  export const failure = createAction(
    '[Domain Role Options] Failure',
    props<{ error: any }>()
  );
}
