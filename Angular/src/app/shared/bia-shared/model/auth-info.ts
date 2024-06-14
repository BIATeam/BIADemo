import { Team } from 'src/app/domains/bia-domains/team/model/team';
import { RoleMode } from '../../constants';

export class LoginParamDto {
  currentTeamLogins: CurrentTeamDto[];
  teamsConfig: TeamConfigDto[];
  lightToken: boolean;
  fineGrainedPermission: boolean;
  additionalInfos: boolean;
}

export class TeamConfigDto {
  teamTypeId: number;
  roleMode: RoleMode;
  inHeader: boolean;
}

export interface UserInfo {
  id: number;
  lastName?: string;
  firstName?: string;
  login: string;
  country: string;
  language: string;
}

export interface UserProfile {
  theme: string;
}

export interface UserData {
  currentTeams: CurrentTeamDto[];
}

export class CurrentTeamDto {
  teamTypeId: number;
  teamId: number;
  teamTitle: string;
  currentRoleIds: number[];
  useDefaultRoles: boolean;
}

export interface AdditionalInfos {
  userInfo: UserInfo;
  userProfile: UserProfile;
  teams: Team[];
}

export interface Token {
  login: string;
  id: number;
  permissions: string[];
  userData: UserData;
}

export class AuthInfo {
  token = '';
  uncryptedToken: Token = {
    login: '',
    id: 0,
    permissions: [],
    userData: { currentTeams: [] },
  };
  additionalInfos: AdditionalInfos = {
    userInfo: { login: '', country: '', id: 0, language: '' },
    teams: [],
    userProfile: { theme: '' },
  };
}
