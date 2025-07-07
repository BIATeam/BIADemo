import { APP_BASE_HREF, NgIf } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
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
import { SpinnerComponent } from '../spinner/spinner.component';
import { BiaLayoutService } from './services/layout.service';
import { BiaUltimaLayoutComponent } from './ultima/layout/ultima-layout.component';

@Component({
  selector: 'bia-layout',
  templateUrl: './layout.component.html',
  imports: [NgIf, SpinnerComponent, BiaUltimaLayoutComponent],
})
export class LayoutComponent implements OnInit {
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
  lastname?: string;
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
  ) {}

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
      this.menus = this.navigationService.filterNavByRole(authInfo, NAVIGATION);
    }
  }
}
