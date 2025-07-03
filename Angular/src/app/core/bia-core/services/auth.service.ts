import { HttpStatusCode } from '@angular/common/http';
import { Injectable, Injector, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, NEVER, Observable, Subscription, of } from 'rxjs';
import {
  catchError,
  filter,
  finalize,
  map,
  skip,
  switchMap,
  take,
} from 'rxjs/operators';
import { DomainTeamsActions } from 'src/app/domains/bia-domains/team/store/teams-actions';
import {
  AdditionalInfos,
  AuthInfo,
  CurrentTeamDto,
  LoginParamDto,
  Token,
} from 'src/app/shared/bia-shared/model/auth-info';
import { RoleMode, TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { allEnvironments } from 'src/environments/all-environments';
import { AbstractDas } from './abstract-das.service';
import { BiaMessageService } from './bia-message.service';
import { BiaOnlineOfflineService } from './bia-online-offline.service';
import { BiaSwUpdateService } from './bia-sw-update.service';
import { RefreshTokenService } from './refresh-token.service';

const STORAGE_LOGINPARAM_KEY = 'loginParam';
const STORAGE_RELOADED_KEY = 'isReloaded';

@Injectable({
  providedIn: 'root',
})
export class AuthService extends AbstractDas<AuthInfo> implements OnDestroy {
  protected sub = new Subscription();
  protected authInfoSubject: BehaviorSubject<AuthInfo> =
    new BehaviorSubject<AuthInfo>(new AuthInfo());
  public authInfo$: Observable<AuthInfo> = this.authInfoSubject
    .asObservable()
    .pipe(
      filter(
        (authInfo: AuthInfo) => authInfo !== null && authInfo !== undefined
      )
    );

  constructor(
    injector: Injector,
    protected biaMessageService: BiaMessageService,
    protected translateService: TranslateService,
    protected store: Store<AppState>,
    protected biaSwUpdateService: BiaSwUpdateService
  ) {
    super(injector, 'Auth');
    RefreshTokenService.shouldRefreshToken = false;
    this.init();
  }

  protected init() {
    this.authInfo$.subscribe((authInfo: AuthInfo | null) => {
      if (
        authInfo &&
        authInfo.additionalInfos &&
        authInfo.decryptedToken.userData
      ) {
        authInfo.decryptedToken.userData.currentTeams.forEach(team => {
          this.setCurrentTeamId(team.teamTypeId, team.teamId);
          this.setCurrentRoleIds(
            team.teamTypeId,
            team.teamId,
            team.currentRoleIds
          );
        });
      }
    });
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  public logout() {
    this.authInfoSubject.next(new AuthInfo());
  }

  protected isInLogin = false;
  public login(): Observable<AuthInfo> {
    if (this.isInLogin) {
      console.info('isInLogin');
      return this.authInfo$.pipe(skip(1), take(1));
    }
    this.isInLogin = true;
    return this.checkFrontEndVersion().pipe(
      take(1),
      switchMap((isCorrectVersion: boolean) => {
        if (isCorrectVersion === true) {
          return this.getAuthInfo();
        } else {
          this.getLatestVersion();
          return NEVER;
        }
      }),
      finalize(() => {
        console.info('Finalize login');
        this.isInLogin = false;
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
    if (RefreshTokenService.shouldRefreshToken) {
      console.info('Login from hasPermissionObs.');
      return this.login().pipe(
        map((authInfo: AuthInfo | null) => {
          return this.checkPermission(authInfo, permission);
        })
      );
    }
    return this.authInfo$.pipe(
      map((authInfo: AuthInfo) => {
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

  public getDecryptedToken(): Token {
    const authInfo = this.authInfoSubject.value;
    if (authInfo) {
      return authInfo.decryptedToken;
    }
    return <Token>{};
  }

  public decodeToken(token: string): Token {
    const objDecodedToken: any = jwtDecode(token);
    const decodedToken = <Token>{
      id: +objDecodedToken[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'
      ],
      identityKey:
        objDecodedToken[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
        ],
      userData: JSON.parse(
        objDecodedToken[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata'
        ]
      ),
      permissions:
        objDecodedToken[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ],
    };

    return decodedToken;
  }

  public getAdditionalInfos(): AdditionalInfos {
    const authInfo = this.authInfoSubject.value;
    if (authInfo) {
      return authInfo.additionalInfos;
    }
    return <AdditionalInfos>{};
  }

  public getLoginParameters(): LoginParamDto {
    const value = sessionStorage.getItem(STORAGE_LOGINPARAM_KEY);
    if (value) {
      const loginParam: LoginParamDto = <LoginParamDto>JSON.parse(value);
      loginParam.currentTeamLogins.forEach(tl => {
        tl.teamId = +tl.teamId;
        tl.currentRoleIds = tl.currentRoleIds.map(roleId => +roleId);
      });
      loginParam.teamsConfig = allEnvironments.teams;
      loginParam.lightToken = false;
      loginParam.fineGrainedPermission = true;
      loginParam.additionalInfos = true;
      loginParam.isFirstLogin = false;
      return loginParam;
    }

    return {
      currentTeamLogins: [],
      lightToken: false,
      fineGrainedPermission: true,
      additionalInfos: true,
      teamsConfig: allEnvironments.teams,
      isFirstLogin: true,
    };
  }

  public setLoginParameters(loginParam: LoginParamDto) {
    sessionStorage.setItem(STORAGE_LOGINPARAM_KEY, JSON.stringify(loginParam));
  }
  public getCurrentTeams(
    teamTypeIds: TeamTypeId[]
  ): CurrentTeamDto[] | undefined {
    const teamsLogin = this.getLoginParameters().currentTeamLogins;
    return teamsLogin.filter(i => teamTypeIds.indexOf(i.teamTypeId) > -1);
  }

  public getCurrentTeamIds(teamTypeId: TeamTypeId[]): number[] {
    const team = this.getCurrentTeams(teamTypeId);
    if (team) {
      return team.map(t => t.teamId);
    }
    return [];
  }

  public getCurrentTeam(teamTypeId: TeamTypeId): CurrentTeamDto | undefined {
    const teamsLogin = this.getLoginParameters().currentTeamLogins;
    return teamsLogin.find(i => i.teamTypeId === teamTypeId);
  }

  public getCurrentTeamId(teamTypeId: TeamTypeId): number {
    const team = this.getCurrentTeam(teamTypeId);
    if (team) {
      return team.teamId;
    }
    return -1;
  }

  public getCurrentRoleIds(teamTypeId: number): number[] {
    const team = this.getCurrentTeam(teamTypeId);
    if (team) {
      return team.currentRoleIds;
    }
    return [];
  }
  public changeCurrentTeamId(teamTypeId: number, teamId: number) {
    if (this.setCurrentTeamId(teamTypeId, teamId)) {
      this.reLogin();
    }
  }

  public reLogin() {
    if (!this.isInLogin) {
      RefreshTokenService.shouldRefreshToken = true;
      this.authInfoSubject.next(new AuthInfo());
      this.login().subscribe(() => {
        console.log('Logged after relogin');
      });
    }
  }

  protected setCurrentTeamId(teamTypeId: number, teamId: number): boolean {
    teamId = +teamId;
    const loginParam = this.getLoginParameters();
    const teamsLogin = loginParam.currentTeamLogins;
    const team = teamsLogin.find(i => i.teamTypeId === teamTypeId);
    if (team) {
      if (+team.teamId !== +teamId) {
        if (teamId === 0) {
          // TODO check if there is a remove in array;
          loginParam.currentTeamLogins = teamsLogin.filter(
            i => i.teamTypeId !== teamTypeId
          );
        } else {
          team.teamId = teamId;
          team.useDefaultRoles = true;
          team.currentRoleIds = [];
        }
        this.setLoginParameters(loginParam);
        return true;
      }
    } else {
      if (teamId !== 0) {
        const newTeam = new CurrentTeamDto();
        newTeam.teamTypeId = teamTypeId;
        newTeam.useDefaultRoles = true;
        newTeam.currentRoleIds = [];
        newTeam.teamId = teamId;
        loginParam.currentTeamLogins.push(newTeam);
        this.setLoginParameters(loginParam);
        return true;
      }
    }
    return false;
  }

  public changeCurrentRoleIds(
    teamTypeId: number,
    teamId: number,
    roleIds: number[]
  ) {
    if (this.setCurrentRoleIds(teamTypeId, teamId, roleIds)) {
      this.reLogin();
    }
  }

  protected setCurrentRoleIds(
    teamTypeId: number,
    teamId: number,
    roleIds: number[]
  ): boolean {
    roleIds = roleIds.map(roleId => +roleId);
    const loginParam = this.getLoginParameters();
    const roleMode =
      loginParam.teamsConfig.find(r => r.teamTypeId === teamTypeId)?.roleMode ||
      RoleMode.AllRoles;
    if (roleMode !== RoleMode.AllRoles) {
      const teamsLogin = loginParam.currentTeamLogins;
      const team = teamsLogin.find(i => i.teamId === teamId);
      if (team) {
        if (+team.currentRoleIds !== +roleIds) {
          team.currentRoleIds = roleIds;
          team.useDefaultRoles = false;
          this.setLoginParameters(loginParam);
          return true;
        }
      } else {
        throw new Error('Error the teamid should be set before roles');
      }
    }
    return false;
  }

  public getFrontEndVersion(): Observable<string> {
    return this.http.get<string>(`${this.route}frontEndVersion`);
  }

  protected checkPermission(authInfo: AuthInfo | null, permission: string) {
    if (!permission) {
      return true;
    }
    if (authInfo) {
      return (
        authInfo.decryptedToken.permissions.some(p => p === permission) === true
      );
    }
    return false;
  }

  protected getAuthInfo() {
    return this.registerToken(
      this.http.post<AuthInfo>(
        `${this.route}LoginAndTeams`,
        this.getLoginParameters()
      )
    );
  }

  protected registerToken(
    authResult: Observable<AuthInfo>
  ): Observable<AuthInfo> {
    return authResult.pipe(
      map((authInfo: AuthInfo) => {
        if (authInfo) {
          authInfo.decryptedToken = this.decodeToken(authInfo.token);
        }
        RefreshTokenService.shouldRefreshToken = false;
        this.authInfoSubject.next(authInfo);

        this.store.dispatch(
          DomainTeamsActions.loadAllSuccess({
            teams: authInfo.additionalInfos.teams,
          })
        );
        return authInfo;
      }),
      catchError(err => {
        if (err.status === HttpStatusCode.Unauthorized) {
          window.location.href =
            allEnvironments.urlErrorPage + '?num=' + err.status;
        }

        RefreshTokenService.shouldRefreshToken = true;
        const authInfo: AuthInfo = <AuthInfo>{};
        this.authInfoSubject.next(authInfo);
        this.store.dispatch(
          DomainTeamsActions.loadAllSuccess({
            teams: authInfo?.additionalInfos?.teams ?? [],
          })
        );

        return of(authInfo);
      })
    );
  }

  public getLightToken() {
    const loginParam = this.getLoginParameters();
    loginParam.lightToken = true;
    return this.http
      .post<AuthInfo>(`${this.route}LoginAndTeams`, loginParam)
      .pipe(
        map((authInfo: AuthInfo) => {
          if (authInfo) {
            authInfo.decryptedToken = this.decodeToken(authInfo.token);
          }
          return authInfo;
        }),
        catchError(() => {
          const authInfo: AuthInfo = <AuthInfo>{};
          return of(authInfo);
        })
      );
  }

  clearSessionExceptLoginInfos() {
    const loginParam = sessionStorage.getItem(STORAGE_LOGINPARAM_KEY);
    sessionStorage.clear();
    if (loginParam) {
      sessionStorage.setItem(STORAGE_LOGINPARAM_KEY, loginParam);
    }
  }

  protected async getLatestVersion() {
    await this.biaSwUpdateService.checkForUpdate();

    setTimeout(async () => {
      const isReloaded = sessionStorage.getItem(STORAGE_RELOADED_KEY);
      // if a refresh has already been done,
      if (
        isReloaded === String(true) &&
        this.biaSwUpdateService.newVersionAvailable !== true
      ) {
        sessionStorage.removeItem(STORAGE_RELOADED_KEY);
        const httpCodeUpgradeRequired = 426;
        window.location.href =
          allEnvironments.urlErrorPage + '?num=' + httpCodeUpgradeRequired;
      } else {
        if (this.biaSwUpdateService.newVersionAvailable === true) {
          await this.biaSwUpdateService.activateUpdate();
        }
        const timer = 5000;
        this.biaMessageService.showInfo(
          this.translateService.instant('biaMsg.infoBeforeGetLatestVersion'),
          timer
        );
        setInterval(() => {
          this.refresh();
        }, timer);
      }
    }, 1000);
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
        return version === allEnvironments.version;
      }),
      catchError(err => {
        if (BiaOnlineOfflineService.isServerAvailable(err) !== true) {
          return of(true);
        }
        return of(false);
      })
    );
  }
}
