import { Injectable, Injector, OnDestroy } from '@angular/core';
import { Observable, BehaviorSubject, of, NEVER, Subscription } from 'rxjs';
import { map, filter, take, switchMap } from 'rxjs/operators';
import { AbstractDas } from './abstract-das.service';
import { AuthInfo, AdditionalInfos } from 'src/app/shared/bia-shared/model/auth-info';
import { environment } from 'src/environments/environment';
import { BiaMessageService } from './bia-message.service';
import { TranslateService } from '@ngx-translate/core';

const STORAGE_SITEID_KEY = 'currentSiteId';
const STORAGE_RELOADED_KEY = 'isReloaded';
const STORAGE_ROLEID_KEY = 'currentRoleId';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends AbstractDas<AuthInfo> implements OnDestroy {
  protected sub = new Subscription();
  protected authInfoSubject: BehaviorSubject<AuthInfo | null> = new BehaviorSubject<AuthInfo | null>(null);
  public authInfo$: Observable<AuthInfo | null> = this.authInfoSubject
    .asObservable()
    .pipe(filter((authInfo: AuthInfo | null) => authInfo !== null && authInfo !== undefined));

  constructor(
    injector: Injector,
    protected biaMessageService: BiaMessageService,
    protected translateService: TranslateService
  ) {
    super(injector, 'Auth');
    this.init();
  }

  protected init() {
    this.authInfo$.subscribe((authInfo: AuthInfo | null) => {
      if (authInfo && authInfo.additionalInfos && authInfo.additionalInfos.userData) {
        this.setCurrentSiteId(authInfo.additionalInfos.userData.currentSiteId);
        this.setCurrentRoleId(authInfo.additionalInfos.userData.currentRoleId);
      }
    });
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  public logout() {
    this.authInfoSubject.next(null);
  }

  public login(): Observable<AuthInfo> {
    return this.checkFrontEndVersion().pipe(
      take(1),
      switchMap((isCorrectVersion: boolean) => {
        if (isCorrectVersion === true) {
          return this.getAuthInfo();
        } else {
          this.getLatestVersion();
          return NEVER;
        }
      })
    );
  }

  public hasPermission(permission: string): boolean {
    return this.checkPermission(this.authInfoSubject.value, permission);
  }

  public hasPermissionObs(permission: string): Observable<boolean> {
    if (!permission) {
      return of(true);
    }
    return this.authInfo$.pipe(
      map((authInfo: AuthInfo | null) => {
        return this.checkPermission(authInfo, permission);
      })
    );
  }

  public getToken(): string {
    const authInfo = this.authInfoSubject.value;
    if (authInfo) {
      return authInfo.token;
    }
    return '';
  }

  public getAdditionalInfos(): AdditionalInfos {
    const authInfo = this.authInfoSubject.value;
    if (authInfo) {
      return authInfo.additionalInfos;
    }
    return <AdditionalInfos>{};
  }

  public getCurrentSiteId(): number {
    const value = sessionStorage.getItem(STORAGE_SITEID_KEY);
    if (value) {
      const siteId: number = <number>JSON.parse(value);
      return siteId;
    }

    return 0;
  }

  public getCurrentRoleId(): number {
    const value = sessionStorage.getItem(STORAGE_ROLEID_KEY);
    if (value) {
      return +value;
    }

    return 0;
  }

  public setCurrentSiteId(siteId: number) {
    if (siteId > 0) {
      sessionStorage.setItem(STORAGE_SITEID_KEY, siteId.toString());
    } else {
      sessionStorage.removeItem(STORAGE_SITEID_KEY);
    }
  }

  public setCurrentRoleId(roleId: number) {
    if (roleId > 0) {
      sessionStorage.setItem(STORAGE_ROLEID_KEY, roleId.toString());
    } else {
      sessionStorage.removeItem(STORAGE_ROLEID_KEY);
    }
  }

  public getFrontEndVersion(): Observable<string> {
    return this.http.get<string>(`${this.route}frontEndVersion`);
  }

  protected checkPermission(authInfo: AuthInfo | null, permission: string) {
    if (!permission) {
      return true;
    }
    if (authInfo) {
      return authInfo.permissions.some((p) => p === permission) === true;
    }
    return false;
  }

  protected getAuthInfo() {
    const url: string = this.buildUrlLogin();
    return this.http.get<AuthInfo>(url).pipe(
      map((authInfo: AuthInfo) => {
        this.authInfoSubject.next(authInfo);
        return authInfo;
      })
    );
  }

  protected buildUrlLogin() {
    let url: string;
    const siteId = this.getCurrentSiteId();
    const roleId = this.getCurrentRoleId();
    if (siteId > 0) {
      if (roleId > 0) {
        url = `${this.route}login/site/${siteId}/${environment.singleRoleMode}/${roleId}`;
      } else {
        url = `${this.route}login/site/${siteId}/${environment.singleRoleMode}`;
      }
    } else {
      url = `${this.route}login/${environment.singleRoleMode}`;
    }
    return url;
  }

  protected getLatestVersion() {
    const isReloaded = sessionStorage.getItem(STORAGE_RELOADED_KEY);
    // if a refresh has already been done,
    if (isReloaded === String(true)) {
      sessionStorage.removeItem(STORAGE_RELOADED_KEY);
      const httpCodeUpgradeRequired = 426;
      window.location.href = environment.urlErrorPage + '?num=' + httpCodeUpgradeRequired;
    } else {
      const timer = 7000;
      this.biaMessageService.showInfo(this.translateService.instant('biaMsg.infoBeforeGetLatestVersion'), timer);
      setInterval(() => {
        this.refresh();
      }, timer);
    }
  }

  protected refresh() {
    localStorage.clear();
    sessionStorage.clear();
    sessionStorage.setItem(STORAGE_RELOADED_KEY, String(true));
    location.reload();
  }

  protected checkFrontEndVersion(): Observable<boolean> {
    return this.getFrontEndVersion().pipe(
      map((version: string) => {
        return version === environment.version;
      })
    );
  }
}
