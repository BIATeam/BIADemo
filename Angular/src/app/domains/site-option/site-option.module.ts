import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './site-option.contants';
import { reducers } from './store/site-option.state';
import { SiteOptionsEffects } from './store/site-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([SiteOptionsEffects]),
  ],
})
export class SiteOptionModule {}
