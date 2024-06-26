import { HttpClient } from '@angular/common/http';
import { LOCALE_ID, NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  TranslateLoader,
  TranslateModule,
  TranslateStore,
} from '@ngx-translate/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { LoggerModule, TOKEN_LOGGER_SERVER_SERVICE } from 'ngx-logger';
import { environment } from 'src/environments/environment';
import { HomeModule } from './features/home/home.module';
import { BiaErrorHandler } from './core/bia-core/shared/bia-error-handler';
import { getCurrentCulture } from './core/bia-core/services/bia-translation.service';
import { BiaTranslateHttpLoader } from './core/bia-core/services/bia-translate-http-loader';
import { ROOT_REDUCERS, metaReducers } from './store/state';
import { BiaSignalRService } from './core/bia-core/services/bia-signalr.service';
import { ServiceWorkerModule } from '@angular/service-worker';
import { buildSpecificModules } from './build-specifics/bia-build-specifics';
import { BiaEnvironmentService } from './core/bia-core/services/bia-environment.service';
import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';
import { BiaNgxLoggerServerService } from './core/bia-core/services/bia-ngx-logger-server.service';

export function createTranslateLoader(http: HttpClient, store: TranslateStore) {
  return new BiaTranslateHttpLoader(http, store, './assets/i18n/app/');
}

@NgModule({
  declarations: [AppComponent],
  imports: [
    LoggerModule.forRoot(BiaEnvironmentService.getLoggingConf(), {
      serverProvider: {
        provide: TOKEN_LOGGER_SERVER_SERVICE,
        useClass: BiaNgxLoggerServerService,
      },
    }),
    BrowserModule,
    BrowserAnimationsModule,
    StoreModule.forRoot(ROOT_REDUCERS, {
      metaReducers,
      runtimeChecks: {
        strictStateImmutability: false,
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
  ],
  providers: [
    DatePipe,
    CurrencyPipe,
    DecimalPipe,
    { provide: LOCALE_ID, useFactory: getCurrentCulture },
    { provide: ErrorHandler, useClass: BiaErrorHandler },
    BiaSignalRService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
