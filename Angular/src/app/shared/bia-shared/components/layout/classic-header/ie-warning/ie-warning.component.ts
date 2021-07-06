import {
  ChangeDetectionStrategy,
  Component,
  HostBinding,
  HostListener,
  OnInit
} from '@angular/core';

@Component({
  selector: 'bia-ie-warning',
  template: `
    <ng-container *ngIf="showIEWarning">
      {{ 'biaMsg.ieWarning' | translate }}
    </ng-container>
  `,
  styles: [
    `
      :host {
        display: flex;
        justify-content: center;
        align-items: center;
        cursor: pointer;
      }
    `
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class IeWarningComponent implements OnInit {
  @HostBinding('class.mat-caption') c = true;

  showIEWarning: boolean;

  ngOnInit() {
    if (Math.floor(Math.random() * 5) === 0) {
      // Have fun
      localStorage.removeItem('ie-warning');
    }
    this.showIEWarning = localStorage.getItem('ie-warning') === null;
  }

  @HostListener('click')
  dismissWarning() {
    this.showIEWarning = false;
    localStorage.setItem('ie-warning', 'shown');
  }
}
