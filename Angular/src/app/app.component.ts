import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNGConfig } from 'primeng/api';
import { BiaInjectExternalService } from './core/bia-core/services/bia-inject-external.service';
import { BiaMatomoService } from './core/bia-core/services/matomo/bia-matomo.service';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  styles: [':host { min-height: 100vh; display: flex; flex-direction: column; }']
})
export class AppComponent implements OnInit {
  constructor(
    private biaMatomoService: BiaMatomoService,
    private biaExternalJsService: BiaInjectExternalService,
    private primeNgConfig: PrimeNGConfig,
    private translateService: TranslateService
  ) {}

  ngOnInit() {
    this.biaMatomoService.init();
    this.biaExternalJsService.init();
    this.translateService.get('primeng').subscribe(res => this.primeNgConfig.setTranslation(res));
  }
}
