import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

export interface MenuProfileConfig {
  templateHtml: string;
  translationKeys: string[];
}

@Injectable({
  providedIn: 'root',
})
export class BiaMenuProfileService {
  protected config: MenuProfileConfig = {
    templateHtml: '<strong>{bia.greetings} {displayName}</strong>',
    translationKeys: ['bia.greetings'],
  };

  constructor(protected readonly translateService: TranslateService) {}

  setConfig(config: MenuProfileConfig) {
    this.config = config;
  }

  getMenuProfileHtml(displayName: string) {
    let template = this.config.templateHtml;

    this.config.translationKeys.forEach(key => {
      const translatedValue = this.translateService.instant(key);
      template = template.replace(`{${key}}`, translatedValue);
    });

    return template.replace('{displayName}', displayName);
  }
}
