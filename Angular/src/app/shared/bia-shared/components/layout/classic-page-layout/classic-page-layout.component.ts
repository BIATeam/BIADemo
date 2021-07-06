import { Component } from '@angular/core';
import { ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'bia-classic-page-layout',
  template: `
    <div class="mat-elevation-z2" fxFlex="95">
      <ng-content></ng-content>
    </div>
  `,
  styleUrls: ['./classic-page-layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClassicPageLayoutComponent {}
