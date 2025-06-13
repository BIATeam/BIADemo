// Modules
import {
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import {
  NgModule,
  Optional,
  SkipSelf,
  inject,
  provideAppInitializer,
} from '@angular/core';

// PrimeNG Services
import { MessageService } from 'primeng/api';

// Interceptor
import { biaXhrWithCredInterceptor } from './interceptors/bia-xhr-with-cred-interceptor.service';
import { standardEncodeHttpParamsInterceptor } from './interceptors/standard-encode-http-params-interceptor.service';
import { biaTokenInterceptor } from './interceptors/token.interceptor';

// Services
import { APP_BASE_HREF, PlatformLocation } from '@angular/common';
import { ServiceWorkerModule } from '@angular/service-worker';
import { KeycloakAngularModule } from 'keycloak-angular';
import { AppSettingsModule } from 'src/app/domains/bia-domains/app-settings/app-settings.module';
import { NotificationSignalRService } from 'src/app/domains/bia-domains/notification/services/notification-signalr.service';
import { TeamModule } from 'src/app/domains/bia-domains/team/team.module';
import { AuthService } from './services/auth.service';
import { BiaAppInitService } from './services/bia-app-init.service';
import { BiaThemeService } from './services/bia-theme.service';
import { BiaTranslationService } from './services/bia-translation.service';

export function initializeApp(appInitService: BiaAppInitService) {
  return (): Promise<any> => {
    return appInitService.initAuth();
  };
}

const MODULES = [
  TeamModule,
  AppSettingsModule,
  ServiceWorkerModule,
  // eslint-disable-next-line @typescript-eslint/no-deprecated
  KeycloakAngularModule,
];

/* Warning: the order matters */
const INTERCEPTORS = [
  standardEncodeHttpParamsInterceptor,
  biaXhrWithCredInterceptor,
  biaTokenInterceptor,
];

const SERVICES = [
  BiaAppInitService,
  MessageService,
  AuthService,
  BiaThemeService,
  BiaTranslationService,
  NotificationSignalRService,
];

const BASE_HREF = [
  {
    provide: APP_BASE_HREF,
    useFactory: (s: PlatformLocation) => s.getBaseHrefFromDOM(),
    deps: [PlatformLocation],
  },
];

@NgModule({
  imports: [...MODULES],
  exports: [...MODULES],
  providers: [
    ...INTERCEPTORS,
    ...SERVICES,
    ...BASE_HREF,
    provideAppInitializer(() => {
      const initializerFn = initializeApp(inject(BiaAppInitService));
      return initializerFn();
    }),
    provideHttpClient(withInterceptorsFromDi()),
  ],
})

// https://medium.com/@benmohamehdi/angular-best-practices-coremodule-vs-sharedmodule-25f6721aa2ef
export class BiaCoreModule {
  constructor(@Optional() @SkipSelf() parentModule: BiaCoreModule) {
    if (parentModule) {
      throw new Error(
        'BiaCoreModule is already loaded. Import it in the AppModule only'
      );
    }
  }
}
