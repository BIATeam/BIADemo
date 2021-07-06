import { HttpClient } from '@angular/common/http';
import { LOCALE_ID, NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateLoader, TranslateModule, TranslateStore } from '@ngx-translate/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { LoggerModule } from 'ngx-logger';
import { environment } from 'src/environments/environment';
import { HomeModule } from './features/home/home.module';
import { APP_SUPPORTED_TRANSLATIONS } from './shared/constants';
import { BiaErrorHandler } from './core/bia-core/shared/bia-error-handler';
import { getInitialLang } from './core/bia-core/services/bia-translation.service';
import { BiaTranslateHttpLoader } from './core/bia-core/services/bia-translate-http-loader';
import { ROOT_REDUCERS, metaReducers } from './store/state';
import { BiaSignalRService } from './core/bia-core/services/bia-signalr.service';

export const getLocaleId = () => getInitialLang(APP_SUPPORTED_TRANSLATIONS);

export function createTranslateLoader(http: HttpClient, store: TranslateStore) {
  return new BiaTranslateHttpLoader(http, store, './assets/i18n/app/');
}

@NgModule({
  declarations: [AppComponent],
  imports: [
    LoggerModule.forRoot(environment.logging.conf),
    BrowserModule,
    BrowserAnimationsModule,
    StoreModule.forRoot(ROOT_REDUCERS, {
      metaReducers,
      runtimeChecks: {
        strictStateImmutability: false,
        strictActionImmutability: false
      }
    }) /* Initialise the Central Store with Application's main reducer*/,
    EffectsModule.forRoot([]) /* Start monitoring app's side effects */,
    AppRoutingModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: createTranslateLoader,
        deps: [HttpClient, TranslateStore]
      }
    }),
    CoreModule,
    HomeModule
  ],
  providers: [
    { provide: LOCALE_ID, useFactory: getLocaleId },
    { provide: ErrorHandler, useClass: BiaErrorHandler },
    BiaSignalRService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
