import { Injectable, signal } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

export interface MenuProfileConfig {
  templateHtml: string;
  translationKeys: string[];
  roles?: string;
}

@Injectable({
  providedIn: 'root',
})
export class BiaMenuProfileService {
  _config: MenuProfileConfig = {
    templateHtml:
      '<strong>{bia.greetings} {displayName}</strong><small>{roles}<small>',
    translationKeys: ['bia.greetings'],
  };

  constructor(protected readonly translateService: TranslateService) {}

  config = signal<MenuProfileConfig>(this._config);

  setConfig(config: MenuProfileConfig) {
    this.config.set(config);
  }

  getMenuProfileHtml(displayName: string) {
    const config: MenuProfileConfig = this.config();
    let template = config.templateHtml.replace('{roles}', config.roles ?? '');

    config.translationKeys.forEach(key => {
      const translatedValue = this.translateService.instant(key);
      template = template.replace(`{${key}}`, translatedValue);
    });

    return template.replace('{displayName}', displayName);
  }
}
