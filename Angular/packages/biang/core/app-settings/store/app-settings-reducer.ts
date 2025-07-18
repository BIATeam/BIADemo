import { createReducer, on } from '@ngrx/store';
import { CoreAppSettingsActions } from './app-settings-actions';
import { AppSettingsState } from './app-settings.state';

export const initializeAppSettingsState = (): AppSettingsState => {
  return {
    appSettings: null,
  };
};

export const INIT_APPSETTINGS_STATE: AppSettingsState =
  initializeAppSettingsState();

export const appSettingsReducers = createReducer<AppSettingsState>(
  INIT_APPSETTINGS_STATE,
  on(CoreAppSettingsActions.loadAllSuccess, (state, { appSettings }) => {
    return { ...state, appSettings: appSettings };
  })
);
