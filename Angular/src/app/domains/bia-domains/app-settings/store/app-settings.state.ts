import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AppSettings } from '../model/app-settings';

export interface AppSettingsState {
  appSettings: AppSettings | null;
}

export const getAppSettingsState = createFeatureSelector<AppSettingsState>(
  'bia-domain-app-settings'
);

export const getAppSettings = createSelector(
  getAppSettingsState,
  (state) => state.appSettings
);
