import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNGConfig } from 'primeng/api';
import { BiaMatomoService } from './core/bia-core/services/matomo/bia-matomo.service';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  styles: [':host { min-height: 100vh; display: flex; }']
})
export class AppComponent implements OnInit {
  constructor(
    private biaMatomoService: BiaMatomoService,
    private primeNgConfig: PrimeNGConfig,
    private translateService: TranslateService
  ) {}

  ngOnInit() {
    this.biaMatomoService.init();
    this.translateService.get('primeng').subscribe(res => this.primeNgConfig.setTranslation(res));
  }
}
