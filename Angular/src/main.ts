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
import {
  BiaEnvironmentService,
  BiaErrorHandler,
  BiaNgxLoggerServerService,
  BiaSignalRService,
  BiaTranslateHttpLoader,
  getCurrentCulture,
} from 'packages/bia-ng/core/public-api';
import { ViewsEffects, ViewsStore } from 'packages/bia-ng/shared/public-api';
import { AppRoutingModule } from './app/app-routing.module';
import { AppComponent } from './app/app.component';
import { buildSpecificModules } from './app/build-specifics/bia-build-specifics';
import { CoreModule } from './app/core/core.module';
import { HomeModule } from './app/features/home/home.module';
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
      StoreModule.forFeature('views', ViewsStore.reducers),
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
