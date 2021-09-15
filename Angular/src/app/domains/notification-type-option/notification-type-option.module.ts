import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/notification-type-option.state';
import { NotificationTypeOptionsEffects } from './store/notification-type-options-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-notification-type-options', reducers),
    EffectsModule.forFeature([NotificationTypeOptionsEffects]),
  ]
})
export class NotificationTypeOptionModule {}


















