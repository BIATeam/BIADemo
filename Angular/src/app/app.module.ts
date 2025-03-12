/* eslint-disable @typescript-eslint/naming-convention */
import { HttpClient } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { TranslateStore } from '@ngx-translate/core';
import { BiaTranslateHttpLoader } from './core/bia-core/services/bia-translate-http-loader';

export function createTranslateLoader(http: HttpClient, store: TranslateStore) {
  return new BiaTranslateHttpLoader(http, store, './assets/i18n/app/');
}

@NgModule(/* TODO(standalone-migration): clean up removed NgModule class manually. 
{
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
        strictActionImmutability: false,
      },
    }) // Initialise the Central Store with Application's main reducer,
    buildSpecificModules,
    EffectsModule.forRoot([]) // Start monitoring app's side effects,
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
    providePrimeNG({
      ripple: true,
      inputStyle: 'filled',
      theme: {
        preset: definePreset(Material, {
          semantic: {
            primary: {
              50: '{slate.50}',
              100: '{slate.100}',
              200: '{slate.200}',
              300: '{slate.300}',
              400: '{slate.400}',
              500: '{slate.500}',
              600: '{slate.600}',
              700: '{slate.700}',
              800: '{slate.800}',
              900: '{slate.900}',
              950: '{slate.950}',
            },
          },
        }),
        options: { darkModeSelector: '.app-dark' },
      },
    }),
  ],
  bootstrap: [AppComponent],
} */)
export class AppModule {}
