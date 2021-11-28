import { createReducer, on } from '@ngrx/store';
import { loadDomainAppSettingsSuccess } from './app-settings-actions';
import { AppSettingsState } from './app-settings.state';

export const initializeAppSettingsState = (): AppSettingsState => {
  return {
    appSettings: null
  };
};

export const INIT_STATE: AppSettingsState = initializeAppSettingsState();

export const appSettingsReducers = createReducer<AppSettingsState>(
  INIT_STATE,
  on(loadDomainAppSettingsSuccess, (state, { appSettings }) => {
    return { ...state, appSettings: appSettings };
  })
);
