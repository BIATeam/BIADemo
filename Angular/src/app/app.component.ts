import { Component, OnInit } from '@angular/core';
import { BiaMatomoService } from './core/bia-core/services/matomo/bia-matomo.service';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  styles: [':host { min-height: 100vh; display: flex; }']
})
export class AppComponent implements OnInit {
  constructor(private biaMatomoService: BiaMatomoService) {}

  ngOnInit() {
    this.biaMatomoService.init();
  }
}
