import { Injectable, Injector, OnDestroy } from '@angular/core';
import { Observable, BehaviorSubject, of, NEVER, Subscription } from 'rxjs';
import { map, filter, take, switchMap, catchError } from 'rxjs/operators';
import { AbstractDas } from './abstract-das.service';
import { AuthInfo, AdditionalInfos, Token, LoginParamDto, CurrentTeamDto } from 'src/app/shared/bia-shared/model/auth-info';
import { environment } from 'src/environments/environment';
import { BiaMessageService } from './bia-message.service';
import { TranslateService } from '@ngx-translate/core';
import { RoleMode, TeamTypeId } from 'src/app/shared/constants';
import { allEnvironments } from 'src/environments/all-environments';
import { DomainTeamsActions } from 'src/app/domains/bia-domains/team/store/teams-actions';
import { AppState } from 'src/app/store/state';
import { Store } from '@ngrx/store';
import { BiaOnlineOfflineService } from './bia-online-offline.service';


const STORAGE_LOGINPARAM_KEY = 'loginParam';
const STORAGE_RELOADED_KEY = 'isReloaded';
const STORAGE_AUTHINFO_KEY = 'AuthInfo';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends AbstractDas<AuthInfo> implements OnDestroy {
  public shouldRefreshToken = false;
  protected sub = new Subscription();
  protected authInfoSubject: BehaviorSubject<AuthInfo | null> = new BehaviorSubject<AuthInfo | null>(null);
  public authInfo$: Observable<AuthInfo | null> = this.authInfoSubject
    .asObservable()
    .pipe(filter((authInfo: AuthInfo | null) => authInfo !== null && authInfo !== undefined));

  constructor(
    injector: Injector,
    protected biaMessageService: BiaMessageService,
    protected translateService: TranslateService,
    private store: Store<AppState>,
  ) {
    super(injector, 'Auth');
    this.init();
  }

  protected init() {
    this.authInfo$.subscribe((authInfo: AuthInfo | null) => {
      if (authInfo && authInfo.additionalInfos && authInfo.uncryptedToken.userData) {
        authInfo.uncryptedToken.userData.currentTeams.forEach(team => {
          this.setCurrentTeamId(team.teamTypeId, team.teamId);
          this.setCurrentRoleIds(team.teamTypeId, team.teamId, team.currentRoleIds);
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
    if (this.shouldRefreshToken)
    {
      return this.login().pipe(
        map((authInfo: AuthInfo | null) => {
          return this.checkPermission(authInfo, permission);
        })
      );     
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

  public getUncryptedToken(): Token {
    const authInfo = this.authInfoSubject.value;
    if (authInfo) {
      return authInfo.uncryptedToken;
    }
    return <Token>{};
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
      loginParam.currentTeamLogins.forEach(tl => {tl.teamId = +tl.teamId; tl.currentRoleIds = tl.currentRoleIds.map(roleId => +roleId)})
      loginParam.teamsConfig = allEnvironments.teams;
      return loginParam;
    }

    return {currentTeamLogins:[], lightToken:false, teamsConfig:allEnvironments.teams };
  }

  public setLoginParameters(loginParam: LoginParamDto) {
    sessionStorage.setItem(STORAGE_LOGINPARAM_KEY, JSON.stringify(loginParam));
  }
  public getCurrentTeams(teamTypeIds: TeamTypeId[]): CurrentTeamDto[] | undefined {
    const teamsLogin = this.getLoginParameters().currentTeamLogins;
    return teamsLogin.filter(i => teamTypeIds.indexOf(i.teamTypeId)>-1);
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
    return teamsLogin.find((i => i.teamTypeId === teamTypeId))
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
      this.shouldRefreshToken = true;
    }
  }

  private setCurrentTeamId(teamTypeId: number, teamId: number): boolean {
    teamId = +teamId;
    let loginParam = this.getLoginParameters();
    let teamsLogin = loginParam.currentTeamLogins;
    let team = teamsLogin.find(i => i.teamTypeId === teamTypeId);
    if (team) {
      if (+team.teamId !== +teamId) {
        if (teamId == 0) {
          // TODO check if there is a remove in array;
          teamsLogin = teamsLogin.filter(i => i.teamTypeId !== teamTypeId);
        }
        else {
          team.teamId = teamId
          team.useDefaultRoles = true;
          team.currentRoleIds = [];
        }
        this.setLoginParameters(loginParam);
        return true;
      }
    }
    else {
      if (teamId != 0) {
        let newTeam = new CurrentTeamDto();
        newTeam.teamTypeId = teamTypeId;
        newTeam.useDefaultRoles = true;
        newTeam.currentRoleIds = [];
        newTeam.teamId = teamId;
        teamsLogin.push(newTeam)
        this.setLoginParameters(loginParam);
        return true;
      }
    }
    return false;
  }

  public changeCurrentRoleIds(teamTypeId: number, teamId: number, roleIds: number[]) {
    if (this.setCurrentRoleIds(teamTypeId, teamId, roleIds)) {
      this.shouldRefreshToken = true;
    }
  }

  private setCurrentRoleIds(teamTypeId: number, teamId: number, roleIds: number[]): boolean {
    roleIds = roleIds.map(roleId => +roleId);
    const roleMode = allEnvironments.teams.find(r => r.teamTypeId == teamTypeId)?.roleMode || RoleMode.AllRoles;
    if (roleMode !== RoleMode.AllRoles) {
      let loginParam = this.getLoginParameters();
      let teamsLogin = loginParam.currentTeamLogins;
      let team = teamsLogin.find((i => i.teamId === teamId))
      if (team) {
        if (+team.currentRoleIds !== +roleIds) {
          team.currentRoleIds = roleIds
          team.useDefaultRoles = false;
          this.setLoginParameters(loginParam);
          return true;
        }
      }
      else {
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
      return authInfo.uncryptedToken.permissions.some((p) => p === permission) === true;
    }
    return false;
  }

  protected getAuthInfo() {
    return this.http.post<AuthInfo>(`${this.route}LoginAndTeams`, this.getLoginParameters()).pipe(
      map((authInfo: AuthInfo) => {
        this.shouldRefreshToken = false;
        this.authInfoSubject.next(authInfo);
        if (BiaOnlineOfflineService.isModeEnabled === true) {
          localStorage.setItem(STORAGE_AUTHINFO_KEY, JSON.stringify(authInfo));
        }

        this.store.dispatch(DomainTeamsActions.loadAllSuccess({ teams:authInfo.additionalInfos.teams }));
        return authInfo;
      }),
      catchError((err) => {
        this.shouldRefreshToken = false;
        let authInfo: AuthInfo = <AuthInfo>{};

        if (BiaOnlineOfflineService.isModeEnabled === true && BiaOnlineOfflineService.isServerAvailable(err) !== true) {
          const jsonAuthInfo: string | null = localStorage.getItem(STORAGE_AUTHINFO_KEY);
          if (jsonAuthInfo) {
            authInfo = JSON.parse(jsonAuthInfo);
          }
        }

        this.authInfoSubject.next(authInfo);
        this.store.dispatch(DomainTeamsActions.loadAllSuccess({ teams:authInfo.additionalInfos.teams }));

        return of(authInfo);
      })
    );
  }

  
  public getLightToken() {
    let loginParam = this.getLoginParameters();
    loginParam.lightToken = true;
    return this.http.post<AuthInfo>(`${this.route}LoginAndTeams`, loginParam).pipe(
      map((authInfo: AuthInfo) => {
        return authInfo;
      }),
      catchError((err) => {
        let authInfo: AuthInfo = <AuthInfo>{};
        return of(authInfo);
      })
    );
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
        return version === allEnvironments.version;
      }),
      catchError((err) => {
        if (BiaOnlineOfflineService.isServerAvailable(err) !== true) {
          return of(true);
        }
        return of(false);
      })
    );
  }
}
