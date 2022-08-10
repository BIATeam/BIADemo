import { createReducer, on } from '@ngrx/store';
import { AppSettingsService } from '../services/app-settings.service';
import { DomainAppSettingsActions } from './app-settings-actions';
import { AppSettingsState } from './app-settings.state';

export const initializeAppSettingsState = (): AppSettingsState => {
  return {
    appSettings: null
  };
};

export const INIT_STATE: AppSettingsState = initializeAppSettingsState();

export const appSettingsReducers = createReducer<AppSettingsState>(
  INIT_STATE,
  on(DomainAppSettingsActions.loadAllSuccess, (state, { appSettings }) => {
    AppSettingsService.appSettings = { ...appSettings };
    return { ...state, appSettings: appSettings };
  })
);
