import { registerLocaleData } from '@angular/common';
import localeEs from '@angular/common/locales/es';
import localeFr from '@angular/common/locales/fr';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { RouterModule } from '@angular/router';
import {
  BiaCoreModule,
  BiaTranslationService,
} from 'packages/bia-ng/core/public-api';
import { allEnvironments } from 'src/environments/all-environments';
import { environment } from 'src/environments/environment';
import biaLocaleEn from '../../assets/bia/i18n/en.json';
import biaLocaleEs from '../../assets/bia/i18n/es.json';
import biaLocaleFr from '../../assets/bia/i18n/fr.json';
import {
  APP_SUPPORTED_TRANSLATIONS,
  APP_TANSLATION_IDS_TO_NOT_ADD_MANUALY,
  DEFAULT_PAGE_SIZE,
  DEFAULT_POPUP_MINWIDTH,
  SHOW_FPS,
  TeamTypeRightPrefix,
} from '../shared/constants';
import { NAVIGATION } from '../shared/navigation';

import {
  AppDB,
  biaOnlineOfflineInterceptor,
  BiaOnlineOfflineService,
} from 'packages/bia-ng/core/public-api';
const ONLINEOFFLINE =
  allEnvironments.enableOfflineMode === true
    ? [BiaOnlineOfflineService, biaOnlineOfflineInterceptor, AppDB]
    : [];

@NgModule({
  imports: [
    RouterModule,
    BiaCoreModule.forRoot(
      allEnvironments,
      environment,
      NAVIGATION,
      APP_SUPPORTED_TRANSLATIONS,
      DEFAULT_PAGE_SIZE,
      TeamTypeRightPrefix,
      APP_TANSLATION_IDS_TO_NOT_ADD_MANUALY,
      DEFAULT_POPUP_MINWIDTH,
      SHOW_FPS
    ),
  ],
  providers: [...ONLINEOFFLINE],
})

// https://medium.com/@benmohamehdi/angular-best-practices-coremodule-vs-sharedmodule-25f6721aa2ef
export class CoreModule {
  constructor(
    @Optional() @SkipSelf() parentModule: CoreModule,
    biaTranslation: BiaTranslationService
  ) {
    if (parentModule) {
      throw new Error(
        'CoreModule is already loaded. Import it in the AppModule only'
      );
    }
    biaTranslation.registerLocaleData(biaLocaleEn);
    biaTranslation.registerLocaleData(biaLocaleFr);
    biaTranslation.registerLocaleData(biaLocaleEs);
    registerLocaleData(localeFr, 'fr');
    registerLocaleData(localeEs, 'es');
  }
}
