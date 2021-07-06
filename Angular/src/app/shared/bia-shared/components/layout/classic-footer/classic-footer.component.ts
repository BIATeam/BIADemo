import { ChangeDetectionStrategy } from '@angular/core';
import { Component, Input } from '@angular/core';
import { FRAMEWORK_VERSION } from '../../../framework-version';
import { BiaClassicLayoutService } from '../classic-layout/bia-classic-layout.service';

@Component({
  selector: 'bia-classic-footer',
  templateUrl: './classic-footer.component.html',
  styleUrls: ['./classic-footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ClassicFooterComponent {
  @Input() companyName: string;
  @Input() logo: string;

  frameworkVersion = FRAMEWORK_VERSION;

  constructor(public layoutService: BiaClassicLayoutService) {}
}
