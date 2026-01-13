import { AsyncPipe, NgTemplateOutlet } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Input,
  OnInit,
} from '@angular/core';
import { Observable, timer } from 'rxjs';
import { BiaThemeService } from '../layout/services/bia-theme.service';

@Component({
  selector: 'bia-spinner',
  template: `
    @if (overlay) {
      <div class="overlay">
        <ng-template [ngTemplateOutlet]="picture"></ng-template>
      </div>
    } @else {
      @if ((showSpinner$ | async) !== null) {
        @switch (themeService.isCurrentThemeDark$ | async) {
          @case (true) {
            <picture>
              <source
                type="image/webp"
                srcset="assets/bia/img/spinner_light.webp" />
              <img src="assets/bia/img/spinner_light.gif" />
            </picture>
          }
          @case (false) {
            <picture>
              <source type="image/webp" srcset="assets/bia/img/spinner.webp" />
              <img src="assets/bia/img/spinner.gif" />
            </picture>
          }
        }
      }
    }
    <ng-template #picture>
      @if ((showSpinner$ | async) !== null) {
        @switch (themeService.isCurrentThemeDark$ | async) {
          @case (true) {
            <picture>
              <source
                type="image/webp"
                srcset="assets/bia/img/spinner_light.webp" />
              <img src="assets/bia/img/spinner_light.gif" />
            </picture>
          }
          @case (false) {
            <picture>
              <source type="image/webp" srcset="assets/bia/img/spinner.webp" />
              <img src="assets/bia/img/spinner.gif" />
            </picture>
          }
        }
      }
    </ng-template>
  `,
  styleUrls: ['./spinner.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgTemplateOutlet, AsyncPipe],
})
export class SpinnerComponent implements OnInit {
  @Input()
  set size(size: 'tiny' | 'small') {
    if (size) {
      // Waiting IVY for better impl (https://github.com/angular/angular/issues/7289)
      this.elementRef.nativeElement.classList.add(size);
    }
  }
  @Input() overlay: boolean;
  @Input() showAfter = 100;

  showSpinner$: Observable<any>;

  constructor(
    protected elementRef: ElementRef<HTMLElement>,
    public themeService: BiaThemeService
  ) {}

  ngOnInit() {
    this.showSpinner$ = timer(this.showAfter);
  }
}
