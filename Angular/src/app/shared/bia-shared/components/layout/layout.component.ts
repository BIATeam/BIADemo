import { APP_BASE_HREF } from '@angular/common';
import { Component, HostBinding, Inject, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { tap } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import {
  BiaTranslationService,
  getCurrentCulture,
} from 'src/app/core/bia-core/services/bia-translation.service';
import { NavigationService } from 'src/app/core/bia-core/services/navigation.service';
import { EnvironmentType } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import { NAVIGATION } from 'src/app/shared/navigation';
import { allEnvironments } from 'src/environments/all-environments';
import { environment } from 'src/environments/environment';
import { APP_SUPPORTED_TRANSLATIONS } from '../../../constants';
import { AuthInfo } from '../../model/auth-info';
import { BiaNavigation } from '../../model/bia-navigation';
import { BiaLayoutService } from './services/layout.service';

@Component({
  selector: 'bia-layout',
  templateUrl: './layout.component.html',
})
export class LayoutComponent implements OnInit {
  @HostBinding('class.bia-flex') classicStyle = false;

  isLoadingUserInfo = false;
  menus = new Array<BiaNavigation>();
  version = allEnvironments.version;
  appTitle = allEnvironments.appTitle;
  companyName = allEnvironments.companyName;
  helpUrl = environment.helpUrl;
  reportUrl = environment.reportUrl;
  enableNotifications = allEnvironments.enableNotifications;
  login = '';
  username = '';
  headerLogos: string[];
  footerLogo = 'assets/bia/img/Footer.png';
  supportedLangs = APP_SUPPORTED_TRANSLATIONS;

  constructor(
    public biaTranslationService: BiaTranslationService,
    protected navigationService: NavigationService,
    protected authService: AuthService,
    protected readonly layoutService: BiaLayoutService,
    protected readonly store: Store,
    // protected notificationSignalRService: NotificationSignalRService,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {
    this.classicStyle = layoutService.config().classicStyle;
    this.layoutService.configUpdate$
      .pipe(
        tap(update => {
          this.classicStyle = update.classicStyle;
        })
      )
      .subscribe();
  }

  public showEnvironmentMessage(environmentType: EnvironmentType | undefined) {
    return environmentType !== EnvironmentType.PRD;
  }

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
      this.login = authInfo.additionalInfos.userInfo.login;
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
