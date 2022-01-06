import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AppSettingsEffects } from './store/app-settings-effects';
import { appSettingsReducers } from './store/app-settings-reducer';

@NgModule({
  imports: [
    StoreModule.forFeature('bia-domain-app-settings', appSettingsReducers),
    EffectsModule.forFeature([AppSettingsEffects])
  ]
})
export class AppSettingsModule {}
