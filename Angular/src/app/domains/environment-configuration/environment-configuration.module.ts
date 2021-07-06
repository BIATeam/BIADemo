import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { EnvironmentConfigurationsEffects } from './store/environment-configuration-effects';
import { environmentConfigurationReducers } from './store/environment-configuration-reducer';

@NgModule({
  imports: [
    StoreModule.forFeature('domain-environments', environmentConfigurationReducers),
    EffectsModule.forFeature([EnvironmentConfigurationsEffects])
  ]
})
export class EnvironmentConfigurationModule {}
