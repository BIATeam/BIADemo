import { createReducer, on } from '@ngrx/store';
import { loadDomainEnvironmentConfigurationSuccess } from './environment-configuration-actions';
import { EnvironmentConfigurationState } from './environment-configuration.state';

export const initializeEnvironmentConfigurationState = (): EnvironmentConfigurationState => {
  return {
    environmentConfiguration: null
  };
};

export const INIT_STATE: EnvironmentConfigurationState = initializeEnvironmentConfigurationState();

export const environmentConfigurationReducers = createReducer<EnvironmentConfigurationState>(
  INIT_STATE,
  on(loadDomainEnvironmentConfigurationSuccess, (state, { environmentConfiguration }) => {
    return { ...state, environmentConfiguration: environmentConfiguration };
  })
);
