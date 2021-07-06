import { createFeatureSelector, createSelector } from '@ngrx/store';
import { EnvironmentConfiguration } from '../model/environment-configuration';

export interface EnvironmentConfigurationState {
  environmentConfiguration: EnvironmentConfiguration | null;
}

export const getEnvironmentConfigurationState = createFeatureSelector<EnvironmentConfigurationState>(
  'domain-environments'
);

export const getEnvironmentConfiguration = createSelector(
  getEnvironmentConfigurationState,
  (state) => state.environmentConfiguration
);
