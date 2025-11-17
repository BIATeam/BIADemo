import { Component, Input } from '@angular/core';
import { BannerMessage } from 'packages/bia-ng/features/public-api';
import { SafeHtmlPipe } from '../../../pipes/safe-html.pipe';

@Component({
  selector: 'bia-banner-message-layout',
  imports: [SafeHtmlPipe],
  templateUrl: './banner-message-layout.component.html',
  styleUrl: './banner-message-layout.component.scss',
})
export class BannerMessageLayoutComponent {
  @Input() messages: BannerMessage[] | null;
}
