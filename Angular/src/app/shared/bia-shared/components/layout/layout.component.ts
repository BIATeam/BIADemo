import { Component, HostBinding, Inject, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { APP_SUPPORTED_TRANSLATIONS } from '../../../constants';
import { AuthInfo } from '../../model/auth-info';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { NavigationService } from 'src/app/core/bia-core/services/navigation.service';
import {
  BiaTranslationService,
  getCurrentCulture,
} from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaNavigation } from '../../model/bia-navigation';
import { NAVIGATION } from 'src/app/shared/navigation';
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
      class="p-input-filled">
      <router-outlet></router-outlet>
    </bia-classic-layout>
  `,
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
  footerLogo = 'assets/bia/img/Footer.png';
  supportedLangs = APP_SUPPORTED_TRANSLATIONS;

  constructor(
    public biaTranslationService: BiaTranslationService,
    protected navigationService: NavigationService,
    protected authService: AuthService,
    // protected notificationSignalRService: NotificationSignalRService,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {}

  ngOnInit() {
    if (this.enableNotifications) {
      // this.initNotificationSignalRService();
    }
    this.setAllParamByUserInfo();
    this.initHeaderLogos();
  }

  protected initHeaderLogos() {
    this.headerLogos = [
      'assets/bia/img/Company.png',
      `assets/bia/img/Division.gif`,
    ];
    /* If image change with the theme :
    this.biaThemeService.isCurrentThemeDark$.subscribe((isThemeDark) => {
      this.headerLogos = [
        'assets/bia/img/Company.png',
        `assets/bia/img/themes/${isThemeDark !== true ? THEME_LIGHT : THEME_DARK}/img/Division.gif`
      ];
    });*/
  }

  protected setAllParamByUserInfo() {
    this.isLoadingUserInfo = true;
    this.authService.authInfo$.subscribe((authInfo: AuthInfo) => {
      if (authInfo && authInfo.token !== '') {
        if (authInfo) {
          this.setLanguage();
          this.setUserName(authInfo);
          this.filterNavByRole(authInfo);
        }
        this.isLoadingUserInfo = false;
      }
    });
  }

  protected setUserName(authInfo: AuthInfo) {
    if (
      authInfo &&
      authInfo.additionalInfos &&
      authInfo.additionalInfos.userInfo
    ) {
      this.username = authInfo.additionalInfos.userInfo.firstName
        ? authInfo.additionalInfos.userInfo.firstName
        : authInfo.additionalInfos.userInfo.login;
    } else {
      this.username = '?';
    }
  }

  protected setLanguage() {
    const langSelected: string | null = getCurrentCulture();
    this.biaTranslationService.loadAndChangeLanguage(langSelected);
  }

  protected filterNavByRole(authInfo: AuthInfo) {
    if (authInfo) {
      this.menus = this.navigationService.filterNavByRole(authInfo, NAVIGATION);
    }
  }
}
