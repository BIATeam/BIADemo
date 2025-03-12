import { Component, HostBinding } from '@angular/core';
import { ClassicPageLayoutComponent } from './classic-page-layout/classic-page-layout.component';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'bia-page-layout',
    template: `
    <bia-classic-page-layout>
      <router-outlet></router-outlet>
    </bia-classic-page-layout>
  `,
    imports: [ClassicPageLayoutComponent, RouterOutlet]
})
export class PageLayoutComponent {
  @HostBinding('class') classes = 'bia-flex';
}
