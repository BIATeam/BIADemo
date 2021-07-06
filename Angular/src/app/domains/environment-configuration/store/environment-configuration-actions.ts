import { createAction, props } from '@ngrx/store';
import { EnvironmentConfiguration } from '../model/environment-configuration';

export const loadDomainEnvironmentConfiguration = createAction('[Domain EnvironmentConfigurations] Load');
export const loadDomainEnvironmentConfigurationSuccess = createAction(
  '[Domain EnvironmentConfigurations] Load success',
  props<{ environmentConfiguration: EnvironmentConfiguration }>()
);
