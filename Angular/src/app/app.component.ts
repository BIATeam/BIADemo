import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNGConfig } from 'primeng/api';
import { BiaInjectExternalService } from './core/bia-core/services/bia-inject-external.service';
import { BiaMatomoService } from './core/bia-core/services/matomo/bia-matomo.service';
// Begin BIADemo
import { BiaDemoDatabase } from './databases/biademo/biademo.database';
// End BIADemo
import { BiaLayoutService } from './shared/bia-shared/components/layout/services/layout.service';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  styles: [
    ':host { min-height: 100vh; display: flex; flex-direction: column; }',
  ],
})
export class AppComponent implements OnInit {
  constructor(
    private biaMatomoService: BiaMatomoService,
    private biaExternalJsService: BiaInjectExternalService,
    private primeNgConfig: PrimeNGConfig,
    private translateService: TranslateService,
    private layoutService: BiaLayoutService,
    // Begin BIADemo
    private biaDemoDatabase: BiaDemoDatabase
    // End BIADemo
  ) {
    this.layoutService.defaultConfigUpdate({});
    this.layoutService.setConfigDisplay({
      // Begin BIADemo
      showMenuStyle: true,
      showFooterStyle: true,
      showToggleStyle: true,

      // End BIADemo
    });
  }

  ngOnInit() {
    this.biaMatomoService.init();
    this.biaExternalJsService.init();
    this.translateService
      .get('primeng')
      .subscribe(res => this.primeNgConfig.setTranslation(res));

    // Begin BIADemo
    this.biaDemoDatabase.init();
    // End BIADemo
  }
}
