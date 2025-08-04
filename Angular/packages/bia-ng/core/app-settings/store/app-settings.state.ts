import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AppSettings } from 'bia-ng/models';

export interface AppSettingsState {
  appSettings: AppSettings | null;
}

export const getAppSettingsState = createFeatureSelector<AppSettingsState>(
  'bia-core-app-settings'
);

export const getAppSettings = createSelector(
  getAppSettingsState,
  state => state.appSettings
);
