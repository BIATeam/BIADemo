import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { BannerMessage } from 'packages/bia-ng/features/public-api';
import { SafeHtmlPipe } from '../../../pipes/safe-html.pipe';

@Component({
  selector: 'bia-banner-message-layout',
  imports: [SafeHtmlPipe],
  templateUrl: './banner-message-layout.component.html',
  styleUrl: './banner-message-layout.component.scss',
})
export class BannerMessageLayoutComponent implements OnChanges {
  @Input() messages: BannerMessage[] | null;
  formatedMessages: string = '';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.messages) {
      this.formatedMessages = this.messages!.map(message =>
        message.rawContent.replace(/<\/?p>/g, '')
      ).join(' - ');
    }
  }
}
