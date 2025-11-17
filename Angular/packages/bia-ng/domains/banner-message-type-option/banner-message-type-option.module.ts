import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './banner-message-type-option.constants';
import { DomainBannerMessageTypeOptionsStore } from './store/banner-message-type-option.state';
import { BannerMessageTypeOptionsEffects } from './store/banner-message-type-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(
      storeKey,
      DomainBannerMessageTypeOptionsStore.reducers
    ),
    EffectsModule.forFeature([BannerMessageTypeOptionsEffects]),
  ],
})
export class BannerMessageTypeOptionModule {}
