import {
  AsyncPipe,
  NgIf,
  NgSwitch,
  NgSwitchCase,
  NgTemplateOutlet,
} from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Input,
  OnInit,
} from '@angular/core';
import { Observable, timer } from 'rxjs';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';

@Component({
  selector: 'bia-spinner',
  template: `
    <div *ngIf="overlay; else picture" class="overlay">
      <ng-template [ngTemplateOutlet]="picture"></ng-template>
    </div>
    <ng-template #picture>
      <ng-container
        *ngIf="(showSpinner$ | async) !== null"
        [ngSwitch]="themeService.isCurrentThemeDark$ | async">
        <picture *ngSwitchCase="true">
          <source
            type="image/webp"
            srcset="assets/bia/img/spinner_light.webp" />
          <img src="assets/bia/img/spinner_light.gif" />
        </picture>
        <picture *ngSwitchCase="false">
          <source type="image/webp" srcset="assets/bia/img/spinner.webp" />
          <img src="assets/bia/img/spinner.gif" />
        </picture>
      </ng-container>
    </ng-template>
  `,
  styleUrls: ['./spinner.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgIf, NgTemplateOutlet, NgSwitch, NgSwitchCase, AsyncPipe],
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
