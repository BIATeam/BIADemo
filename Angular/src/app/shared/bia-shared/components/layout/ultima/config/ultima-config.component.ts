import { Component, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { take, tap } from 'rxjs';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import {
  BiaLayoutService,
  ColorScheme,
  FooterMode,
  MenuMode,
} from '../../services/layout.service';
import { MenuService } from '../../services/menu.service';

@Component({
  selector: 'bia-ultima-config',
  templateUrl: './ultima-config.component.html',
  styleUrls: ['./ultima-config.component.scss'],
})
export class BiaUltimaConfigComponent {
  @Input() minimal = false;
  @Input() supportedLangs: string[];
  @Input() scales: number[] = [12, 13, 14, 15, 16, 17, 18];

  componentThemes: any[] = [];
  menuThemes: any[] = [];
  topbarThemes: any[] = [];
  urlEditAvatar: string;

  protected _currentCulture: string | null;

  constructor(
    public layoutService: BiaLayoutService,
    protected biaTranslation: BiaTranslationService,
    protected biaTheme: BiaThemeService,
    public menuService: MenuService,
    protected translateService: TranslateService,
    protected readonly appSettingsService: AppSettingsService
  ) {
    this.biaTranslation.currentCulture$
      .pipe(
        take(1),
        tap(value => {
          this._currentCulture = value;
        })
      )
      .subscribe();
    this.urlEditAvatar =
      this.appSettingsService.appSettings.profileConfiguration
        ?.urlEditProfileImage ?? '';
  }

  get visible(): boolean {
    return this.layoutService.state.configSidebarVisible;
  }
  set visible(_val: boolean) {
    this.layoutService.state.configSidebarVisible = _val;
  }

  get scale(): number {
    return this.layoutService.config().scale;
  }
  set scale(_val: number) {
    this.layoutService.config.update(config => ({
      ...config,
      scale: _val,
    }));
  }

  get menuMode(): MenuMode {
    return this.layoutService.config().menuMode;
  }
  set menuMode(_val: MenuMode) {
    this.layoutService.config.update(config => ({
      ...config,
      menuMode: _val,
    }));
    if (this.layoutService.isSlim() || this.layoutService.isHorizontal()) {
      this.menuService.reset();
    }
  }

  get footerMode(): FooterMode {
    return this.layoutService.config().footerMode;
  }
  set footerMode(_val: FooterMode) {
    this.layoutService.config.update(config => ({
      ...config,
      footerMode: _val,
    }));
  }

  get currentCulture(): string {
    return this._currentCulture ?? '';
  }
  set currentCulture(_val: string) {
    this._currentCulture = _val;
    this.biaTranslation.loadAndChangeLanguage(_val);
  }

  getLanguageTranslateKey(lang: string) {
    return 'bia.lang.' + lang.split('-')[1].toLowerCase();
  }

  set menuProfilePosition(_val: string) {
    this.layoutService.config.update(config => ({
      ...config,
      menuProfilePosition: _val,
    }));
    if (
      this.layoutService.isSlimPlus() ||
      this.layoutService.isSlim() ||
      this.layoutService.isHorizontal()
    ) {
      this.menuService.reset();
    }
  }

  get colorScheme(): ColorScheme {
    return this.layoutService.config().colorScheme;
  }
  set colorScheme(_val: ColorScheme) {
    this.biaTheme.changeTheme(_val);
    this.layoutService.config.update(config => ({
      ...config,
      colorScheme: _val,
    }));
  }

  decrementScale() {
    this.scale--;
  }

  incrementScale() {
    this.scale++;
  }

  getSupportedLangsSorted(): string[] {
    this.supportedLangs.sort((a, b) => {
      const langA = this.getLanguageName(a);
      const langB = this.getLanguageName(b);
      return langA.localeCompare(langB);
    });

    return this.supportedLangs;
  }

  getLanguageName(lang: string): string {
    const translateKey = this.getLanguageTranslateKey(lang);
    return this.translateService.instant(translateKey);
  }
}
