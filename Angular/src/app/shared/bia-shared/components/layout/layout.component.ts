import { Component, HostBinding, Inject, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { APP_SUPPORTED_TRANSLATIONS } from '../../../constants';
import { AuthInfo } from '../../model/auth-info';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { NavigationService } from 'src/app/core/bia-core/services/navigation.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaNavigation } from '../../model/bia-navigation';
import { NAVIGATION } from 'src/app/shared/navigation';
import { getLocaleId } from 'src/app/app.module';
import { APP_BASE_HREF } from '@angular/common';
import { allEnvironments } from 'src/environments/all-environments';

@Component({
  selector: 'bia-layout',
  template: `
    <bia-spinner [overlay]="true" *ngIf="isLoadingUserInfo"></bia-spinner>
    <bia-classic-layout
      [menus]="menus"
      [version]="version"
      [username]="username"
      [headerLogos]="headerLogos"
      [footerLogo]="footerLogo"
      [supportedLangs]="supportedLangs"
      [appTitle]="appTitle"
      [helpUrl]="helpUrl"
      [reportUrl]="reportUrl"
      [enableNotifications]="enableNotifications"
      [companyName]="companyName"
      class="p-input-filled"
    >
      <router-outlet></router-outlet>
    </bia-classic-layout>
  `
})
export class LayoutComponent implements OnInit {
  @HostBinding('class') classes = 'bia-flex';
  isLoadingUserInfo = false;

  menus = new Array<BiaNavigation>();
  version = allEnvironments.version;
  appTitle = allEnvironments.appTitle;
  companyName = allEnvironments.companyName;
  helpUrl = environment.helpUrl;
  reportUrl = environment.reportUrl;
  enableNotifications = allEnvironments.enableNotifications;
  username = '';
  headerLogos: string[];
  footerLogo = 'assets/bia/Footer.png';
  supportedLangs = APP_SUPPORTED_TRANSLATIONS;

  constructor(
    public biaTranslationService: BiaTranslationService,
    private navigationService: NavigationService,
    private authService: AuthService,
    private biaThemeService: BiaThemeService,
    // private notificationSignalRService: NotificationSignalRService,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) { }

  ngOnInit() {

    if (this.enableNotifications) {
      // this.initNotificationSignalRService();
    }
    this.setAllParamByUserInfo();
    this.initHeaderLogos();
  }

  private initHeaderLogos() {
    this.headerLogos = [
      'assets/bia/Company.png',
      `assets/bia/Division.gif`
    ];
    /* If image change with the theme :
    this.biaThemeService.isCurrentThemeDark$.subscribe((isThemeDark) => {
      this.headerLogos = [
        'assets/bia/Company.png',
        `assets/bia/themes/${isThemeDark !== true ? THEME_LIGHT : THEME_DARK}/img/Division.gif`
      ];
    });*/
  }

  private setAllParamByUserInfo() {
    this.isLoadingUserInfo = true;
    this.authService.authInfo$.subscribe((authInfo: AuthInfo | null) => {
      if (authInfo) {
        this.setLanguage(authInfo);
        this.setUserName(authInfo);
        this.filterNavByRole(authInfo);
        this.setTheme(authInfo);
      }
      this.isLoadingUserInfo = false;
    });
  }

  private setUserName(authInfo: AuthInfo) {
    if (authInfo && authInfo.additionalInfos && authInfo.additionalInfos.userInfo) {
      this.username = authInfo.additionalInfos.userInfo.firstName
        ? authInfo.additionalInfos.userInfo.firstName
        : authInfo.additionalInfos.userInfo.login;
    } else {
      this.username = '?';
    }
  }

  private setLanguage(authInfo: AuthInfo) {
    const langSelected: string | null = this.biaTranslationService.getLangSelected();
    if (langSelected) {
      this.biaTranslationService.loadAndChangeLanguage(langSelected);
    } else if (authInfo && authInfo.additionalInfos && authInfo.additionalInfos.userInfo) {
      const language: string =
        authInfo.additionalInfos.userInfo.language && authInfo.additionalInfos.userInfo.language.length > 0
          ? authInfo.additionalInfos.userInfo.language
          : getLocaleId();
      this.biaTranslationService.loadAndChangeLanguage(language);
    }
  }

  private filterNavByRole(authInfo: AuthInfo) {
    if (authInfo) {
      this.menus = this.navigationService.filterNavByRole(authInfo, NAVIGATION);
    }
  }

  private setTheme(authInfo: AuthInfo) {
    if (
      !this.biaThemeService.getThemeSelected() &&
      authInfo &&
      authInfo.additionalInfos &&
      authInfo.additionalInfos.userProfile &&
      authInfo.additionalInfos.userProfile.theme
    ) {
      this.biaThemeService.changeTheme(authInfo.additionalInfos.userProfile.theme.toLowerCase());
    }
  }
}
