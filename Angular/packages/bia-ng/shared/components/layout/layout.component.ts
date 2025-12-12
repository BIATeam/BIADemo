import { APP_BASE_HREF } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  AuthService,
  BiaAppConstantsService,
  BiaPermission,
  BiaTranslationService,
  getCurrentCulture,
  NavigationService,
} from 'packages/bia-ng/core/public-api';
import { EnvironmentType } from 'packages/bia-ng/models/enum/public-api';
import { AuthInfo, BiaNavigation } from 'packages/bia-ng/models/public-api';
import { SpinnerComponent } from '../spinner/spinner.component';
import { BiaLayoutService } from './services/layout.service';
import { BiaUltimaLayoutComponent } from './ultima/layout/ultima-layout.component';

@Component({
  selector: 'bia-layout',
  templateUrl: './layout.component.html',
  imports: [SpinnerComponent, BiaUltimaLayoutComponent],
})
export class LayoutComponent implements OnInit {
  isLoadingUserInfo = false;
  menus = new Array<BiaNavigation>();
  version = BiaAppConstantsService.allEnvironments.version;
  appTitle = BiaAppConstantsService.allEnvironments.appTitle;
  companyName = BiaAppConstantsService.allEnvironments.companyName;
  helpUrl = BiaAppConstantsService.environment.helpUrl;
  reportUrl = BiaAppConstantsService.environment.reportUrl;
  enableNotifications: boolean;
  login = '';
  username = '';
  lastname?: string;
  headerLogos: string[];
  footerLogo = 'assets/bia/img/Footer.png';
  supportedLangs = BiaAppConstantsService.supportedTranslations;

  constructor(
    public biaTranslationService: BiaTranslationService,
    protected navigationService: NavigationService,
    protected authService: AuthService,
    protected readonly layoutService: BiaLayoutService,
    protected readonly store: Store,
    // protected notificationSignalRService: NotificationSignalRService,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {
    this.enableNotifications =
      BiaAppConstantsService.allEnvironments.enableNotifications &&
      this.authService.hasPermission(BiaPermission.Notification_List_Access);
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

          this.enableNotifications =
            BiaAppConstantsService.allEnvironments.enableNotifications &&
            this.authService.hasPermission(
              BiaPermission.Notification_List_Access
            );
        }
        this.isLoadingUserInfo = false;
      }
    });
  }

  protected setUserName(authInfo: AuthInfo) {
    if (
      authInfo &&
      authInfo.decryptedToken &&
      authInfo.decryptedToken.userData
    ) {
      this.login = authInfo.decryptedToken.identityKey;
      this.username = authInfo.decryptedToken.userData.firstName
        ? authInfo.decryptedToken.userData.firstName
        : authInfo.decryptedToken.identityKey;
      this.lastname = authInfo.decryptedToken.userData.lastName;
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
      this.menus = this.navigationService.filterNavByRole(
        authInfo,
        BiaAppConstantsService.navigation
      );
    }
  }
}
