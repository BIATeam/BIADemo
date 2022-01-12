import { Component, HostBinding, Inject, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { APP_SUPPORTED_TRANSLATIONS } from '../../../constants';
import { AuthInfo, UserData } from '../../model/auth-info';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { NavigationService } from 'src/app/core/bia-core/services/navigation.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaNavigation } from '../../model/bia-navigation';
import { NAVIGATION } from 'src/app/shared/navigation';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../store/state';
import { setDefaultRole, setDefaultSite } from 'src/app/domains/site/store/sites-actions';
import { getLocaleId } from 'src/app/app.module';
import { APP_BASE_HREF } from '@angular/common';

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
      [userData]="userData"
      [companyName]="companyName"
      (siteChange)="onSiteChange($event)"
      (roleChange)="onRoleChange($event)"
      (setDefaultSite)="onSetDefaultSite($event)"
      (setDefaultRole)="onSetDefaultRole($event)"
      class="p-input-filled"
    >
      <router-outlet></router-outlet>
    </bia-classic-layout>
  `
})
export class LayoutComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;
  isLoadingUserInfo = false;

  menus = new Array<BiaNavigation>();
  version = environment.version;
  appTitle = environment.appTitle;
  companyName = environment.companyName;
  helpUrl = environment.helpUrl;
  reportUrl = environment.reportUrl;
  enableNotifications = environment.enableNotifications;
  username = '';
  headerLogos: string[];
  footerLogo = 'assets/bia/Footer.png';
  supportedLangs = APP_SUPPORTED_TRANSLATIONS;
  userData: UserData | null;

  constructor(
    public biaTranslationService: BiaTranslationService,
    private navigationService: NavigationService,
    private authService: AuthService,
    private biaThemeService: BiaThemeService,
    private store: Store<AppState>,
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

  onSiteChange(siteId: number) {
    this.authService.setCurrentSiteId(siteId);
    location.assign(this.baseHref);
  }

  onRoleChange(roleId: number) {
    this.authService.setCurrentRoleId(roleId);
    location.assign(this.baseHref);
  }

  onSetDefaultSite(siteId: number) {
    this.store.dispatch(setDefaultSite({ id: siteId }));
  }

  onSetDefaultRole(roleId: number) {
    this.store.dispatch(setDefaultRole({ id: roleId, siteId: this.authService.getCurrentSiteId() }));
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
        this.setUserData(authInfo);
        this.setUserName(authInfo);
        this.filterNavByRole(authInfo);
        this.setTheme(authInfo);
      }
      this.isLoadingUserInfo = false;
    });
  }

  private setUserData(authInfo: AuthInfo) {
    if (authInfo && authInfo.additionalInfos && authInfo.additionalInfos.userData) {
      this.userData = authInfo.additionalInfos.userData;
    } else {
      this.userData = null;
    }
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
