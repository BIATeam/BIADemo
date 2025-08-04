import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { DomainNotificationTypesStore } from './store/notification-type-option.state';
import { NotificationTypeOptionsEffects } from './store/notification-type-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(
      'domain-notification-type-options',
      DomainNotificationTypesStore.reducers
    ),
    EffectsModule.forFeature([NotificationTypeOptionsEffects]),
  ],
})
export class NotificationTypeOptionModule {}
