import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { BannerMessage } from 'packages/bia-ng/features/public-api';
import { BiaBannerMessageType } from 'packages/bia-ng/models/enum/bia-banner-message-type.enum';
import { SafeHtmlPipe } from '../../../pipes/safe-html.pipe';

@Component({
  selector: 'bia-banner-message-layout',
  imports: [SafeHtmlPipe, TranslateModule],
  templateUrl: './banner-message-layout.component.html',
  styleUrl: './banner-message-layout.component.scss',
})
export class BannerMessageLayoutComponent implements OnChanges {
  @Input() messages: BannerMessage[] | null;
  formatedMessages: string = '';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.messages) {
      this.formatedMessages = this.messages!.map(message => {
        let iconElement = '';
        switch (message.type.id) {
          case BiaBannerMessageType.Info:
            iconElement =
              '<i class="pi pi-info-circle banner-message-icon-info"></i>';
            break;
          case BiaBannerMessageType.Warning:
            iconElement =
              '<i class="pi pi-exclamation-triangle banner-message-icon-warning"></i>';
            break;
        }
        return iconElement + message.rawContent;
      }).join('<span class="banner-message-separator">|</span>');
    }
  }
}
