import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { TranslateRoleLabelPipe } from 'src/app/shared/bia-shared/pipes/translate-role-label.pipe';
import { reducers } from './store/notification.state';
import { NotificationsEffects } from './store/notifications-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-notifications', reducers),
    EffectsModule.forFeature([NotificationsEffects]),
  ],
  providers: [TranslateRoleLabelPipe]
})
export class NotificationModule {}


















