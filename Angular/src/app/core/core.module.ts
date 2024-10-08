import { registerLocaleData } from '@angular/common';
import localeEs from '@angular/common/locales/es';
import localeFr from '@angular/common/locales/fr';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { RouterModule } from '@angular/router';
import biaLocaleEn from '../../assets/bia/i18n/en.json';
import biaLocaleEs from '../../assets/bia/i18n/es.json';
import biaLocaleFr from '../../assets/bia/i18n/fr.json';
import { BiaCoreModule } from './bia-core/bia-core.module';
import { BiaTranslationService } from './bia-core/services/bia-translation.service';

// Begin BIADemo
import { AppDB } from './bia-core/db';
import { biaOnlineOfflineInterceptor } from './bia-core/interceptors/bia-online-offline.interceptor';
import { BiaOnlineOfflineService } from './bia-core/services/bia-online-offline.service';
const ONLINEOFFLINE = [
  BiaOnlineOfflineService,
  biaOnlineOfflineInterceptor,
  AppDB,
];
// End BIADemo

@NgModule({
  imports: [RouterModule, BiaCoreModule],
  // Begin BIADemo
  providers: [...ONLINEOFFLINE],
  // End BIADemo
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
