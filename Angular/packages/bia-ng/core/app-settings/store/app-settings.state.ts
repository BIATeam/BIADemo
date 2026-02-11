import { AppSettings } from '@bia-team/bia-ng/models';
import { createFeatureSelector, createSelector } from '@ngrx/store';

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
