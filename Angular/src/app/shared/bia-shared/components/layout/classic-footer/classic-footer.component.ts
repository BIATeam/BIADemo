import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { FRAMEWORK_VERSION } from '../../../framework-version';
import { BiaLayoutService } from '../services/layout.service';
import { NgIf, AsyncPipe } from '@angular/common';
import { CdkPortalOutlet } from '@angular/cdk/portal';

@Component({
  selector: 'bia-classic-footer',
  templateUrl: './classic-footer.component.html',
  styleUrls: ['./classic-footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [NgIf, CdkPortalOutlet, AsyncPipe],
})
export class ClassicFooterComponent {
  @Input() companyName: string;
  @Input() logo: string;

  frameworkVersion = FRAMEWORK_VERSION;

  constructor(public layoutService: BiaLayoutService) {}
}
