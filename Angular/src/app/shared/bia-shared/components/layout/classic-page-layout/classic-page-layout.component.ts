import { Component } from '@angular/core';
import { ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'bia-classic-page-layout',
  template: `
    <div class="mat-elevation-z2 bia-page-margin" style="width:calc(96vw);margin-left: calc(2vw);">
      <ng-content></ng-content>
    </div>
  `,
  styleUrls: ['./classic-page-layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClassicPageLayoutComponent {}
