import { createAction, props } from '@ngrx/store';
import { AppSettings } from 'biang/models';

export namespace CoreAppSettingsActions {
  export const loadAll = createAction('[BIA Domain AppSettings] Load');
  export const loadAllSuccess = createAction(
    '[BIA Domain AppSettings] Load success',
    props<{ appSettings: AppSettings }>()
  );
  export const failure = createAction(
    '[BIA Domain AppSettings] Failure',
    props<{ error: any }>()
  );
}
