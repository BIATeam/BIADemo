import {
  ErrorHandler,
  LOCALE_ID,
  enableProdMode,
  importProvidersFrom,
} from '@angular/core';

import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { BrowserModule, bootstrapApplication } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import {
  TranslateLoader,
  TranslateModule,
  TranslateStore,
} from '@ngx-translate/core';
import { LoggerModule, TOKEN_LOGGER_SERVER_SERVICE } from 'ngx-logger';
import { AppRoutingModule } from './app/app-routing.module';
import { AppComponent } from './app/app.component';
import { buildSpecificModules } from './app/build-specifics/bia-build-specifics';
import { BiaEnvironmentService } from './app/core/bia-core/services/bia-environment.service';
import { BiaNgxLoggerServerService } from './app/core/bia-core/services/bia-ngx-logger-server.service';
import { BiaSignalRService } from './app/core/bia-core/services/bia-signalr.service';
import { BiaTranslateHttpLoader } from './app/core/bia-core/services/bia-translate-http-loader';
import { getCurrentCulture } from './app/core/bia-core/services/bia-translation.service';
import { BiaErrorHandler } from './app/core/bia-core/shared/bia-error-handler';
import { CoreModule } from './app/core/core.module';
import { HomeModule } from './app/features/home/home.module';
import { reducers } from './app/shared/bia-shared/features/view/store/view.state';
import { ViewsEffects } from './app/shared/bia-shared/features/view/store/views-effects';
import { appConfig } from './app/shared/theme';
import { ROOT_REDUCERS, metaReducers } from './app/store/state';
import { environment } from './environments/environment';

export function createTranslateLoader(http: HttpClient, store: TranslateStore) {
  return new BiaTranslateHttpLoader(http, store, './assets/i18n/app/');
}

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(
      LoggerModule.forRoot(BiaEnvironmentService.getLoggingConf(), {
        serverProvider: {
          provide: TOKEN_LOGGER_SERVER_SERVICE,
          useClass: BiaNgxLoggerServerService,
        },
      }),
      BrowserModule,
      StoreModule.forRoot(ROOT_REDUCERS, {
        metaReducers,
        runtimeChecks: {
          strictActionImmutability: false,
        },
      }) /* Initialise the Central Store with Application's main reducer*/,
      buildSpecificModules,
      EffectsModule.forRoot([]) /* Start monitoring app's side effects */,
      AppRoutingModule,
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: createTranslateLoader,
          deps: [HttpClient, TranslateStore],
        },
      }),
      CoreModule,
      HomeModule,
      ServiceWorkerModule.register('ngsw-worker.js', {
        enabled: environment.production,
        // Register the ServiceWorker as soon as the app is stable
        // or after 30 seconds (whichever comes first).
        registrationStrategy: 'registerWhenStable:30000',
      }),
      StoreModule.forFeature('views', reducers),
      EffectsModule.forFeature([ViewsEffects])
    ),
    DatePipe,
    CurrencyPipe,
    DecimalPipe,
    { provide: LOCALE_ID, useFactory: getCurrentCulture },
    { provide: ErrorHandler, useClass: BiaErrorHandler },
    BiaSignalRService,
    provideAnimations(),
    ...appConfig.providers,
  ],
}).catch(err => console.error(err));
