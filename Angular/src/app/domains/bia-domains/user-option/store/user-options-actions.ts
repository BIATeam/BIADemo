import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export namespace DomainUserOptionsActions {
  export const loadAll = createAction('[Domain User Options] Load all');

  export const loadAllSuccess = createAction(
    '[Domain User Options] Load all success',
    props<{ users: OptionDto[] }>()
  );

  export const loadAllByFilter = createAction(
    '[Domain User Options] Load all by filter',
    props<{ filter: string }>()
  );

  export const failure = createAction(
    '[Domain User Options] Failure',
    props<{ error: any }>()
  );

  export const userAddedInListSuccess = createAction(
    '[Domain Users Options] Change userlist',
    props<{ usersAdded: OptionDto[] }>()
  );
}
