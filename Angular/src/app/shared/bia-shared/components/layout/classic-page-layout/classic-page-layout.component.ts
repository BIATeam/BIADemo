import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'bia-classic-page-layout',
  template: `
    <div class="mat-elevation-z2 bia-page-margin" style="calc(95vw);">
      <ng-content></ng-content>
    </div>
  `,
  styleUrls: ['./classic-page-layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClassicPageLayoutComponent {}
