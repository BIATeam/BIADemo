import { AsyncPipe, NgClass, NgFor, NgIf } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Input,
} from '@angular/core';
import { Ripple } from 'primeng/ripple';
import { FRAMEWORK_VERSION } from 'src/app/shared/bia-shared/framework-version';
import { BiaLayoutService } from '../../services/layout.service';

@Component({
  selector: 'bia-ultima-footer',
  templateUrl: './ultima-footer.component.html',
  styleUrls: ['./ultima-footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [NgIf, NgClass, Ripple, NgFor, AsyncPipe],
})
export class BiaUltimaFooterComponent {
  @Input() companyName: string;
  @Input() logos: string[];

  frameworkVersion = FRAMEWORK_VERSION;

  constructor(
    protected layoutService: BiaLayoutService,
    public el: ElementRef
  ) {}

  get footerClass() {
    const styleClass: { [key: string]: any } = {
      // eslint-disable-next-line @typescript-eslint/naming-convention
      'footer-overlay': this.layoutService.config().footerMode === 'overlay',
    };
    styleClass['layout-menu-' + this.layoutService.config().colorScheme] = true;

    return styleClass;
  }

  onFooterButtonClick() {
    this.layoutService.onFooterToggle();
  }
}
