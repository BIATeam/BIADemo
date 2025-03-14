// Modules
import { PortalModule } from '@angular/cdk/portal';
import { CommonModule } from '@angular/common';
import {
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import { NotificationsEffects } from 'src/app/domains/bia-domains/notification/store/notifications-effects';
import { TeamModule } from 'src/app/domains/bia-domains/team/team.module';
import { UserOptionModule } from 'src/app/domains/bia-domains/user-option/user-option.module';
import { BiaTeamSelectorModule } from './components/bia-team-selector/bia-team-selector.module';
import { BiaUltimaLayoutModule } from './components/layout/ultima/ultima-layout.module';

// PrimeNG Services
import { MessageService } from 'primeng/api';

// Store
import { reducers as notificationReducers } from '../../domains/bia-domains/notification/store/notification.state';
import { reducers } from './features/view/store/view.state';
import { ViewsEffects } from './features/view/store/views-effects';

const MODULES = [
  CommonModule,
  PortalModule,
  TranslateModule,
  FormsModule,
  ReactiveFormsModule,
  TeamModule,
  BiaTeamSelectorModule,
  BiaUltimaLayoutModule,
  RouterModule,
];

const VIEW_IMPORTS = [
  StoreModule.forFeature('views', reducers),
  EffectsModule.forFeature([ViewsEffects]),
];

const NOTIFICATION_IMPORTS = [
  StoreModule.forFeature('domain-notifications', notificationReducers),
  EffectsModule.forFeature([NotificationsEffects]),
];

const TEAM_ADVANCED_FILTER_IMPORTS = [UserOptionModule];
const SERVICES = [MessageService];

@NgModule({
  imports: [
    ...MODULES,
    ...VIEW_IMPORTS,
    ...NOTIFICATION_IMPORTS,
    ...TEAM_ADVANCED_FILTER_IMPORTS,
  ],
  exports: [...MODULES],
  providers: [...SERVICES, provideHttpClient(withInterceptorsFromDi())],
})

// https://medium.com/@benmohamehdi/angular-best-practices-coremodule-vs-sharedmodule-25f6721aa2ef
export class BiaSharedModule {}
