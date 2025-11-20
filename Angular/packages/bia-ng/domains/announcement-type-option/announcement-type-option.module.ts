import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './announcement-type-option.constants';
import { DomainAnnouncementTypeOptionsStore } from './store/announcement-type-option.state';
import { AnnouncementTypeOptionsEffects } from './store/announcement-type-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(
      storeKey,
      DomainAnnouncementTypeOptionsStore.reducers
    ),
    EffectsModule.forFeature([AnnouncementTypeOptionsEffects]),
  ],
})
export class AnnouncementTypeOptionModule {}
