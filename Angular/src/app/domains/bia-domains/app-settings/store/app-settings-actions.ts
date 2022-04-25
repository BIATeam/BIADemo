import { createAction, props } from '@ngrx/store';
import { AppSettings } from '../model/app-settings';

export namespace DomainAppSettingsActions
{ 
  export const loadAll = createAction('[BIA Domain AppSettings] Load');
  export const loadAllSuccess = createAction('[BIA Domain AppSettings] Load success',props<{ appSettings: AppSettings }>());
  export const failure = createAction('[BIA Domain AppSettings] Failure', props<{ error: any }>());
}
