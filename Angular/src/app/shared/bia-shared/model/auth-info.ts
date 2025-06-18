import { Team } from 'src/app/domains/bia-domains/team/model/team';
import { RoleMode } from '../../constants';

export class LoginParamDto {
  currentTeamLogins: CurrentTeamDto[];
  teamsConfig: TeamConfigDto[];
  lightToken: boolean;
  fineGrainedPermission: boolean;
  additionalInfos: boolean;
  isFirstLogin: boolean;
  baseUserLogin?: string;
}

export class TeamConfigDto {
  teamTypeId: number;
  roleMode: RoleMode;
  inHeader: boolean;
  displayOne?: boolean;
  displayAlways?: boolean;
  label?: string;
  displayLabel?: boolean;
  teamSelectionCanBeEmpty?: boolean;
}

export interface UserInfo {
  lastName?: string;
  firstName?: string;
}

export interface UserProfile {
  theme: string;
}

export interface UserData {
  currentTeams: CurrentTeamDto[];
  // Begin BIADemo
  customData?: string | undefined;
  // End BIADemo
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
  // Begin BIADemo
  customInfo?: string | undefined;
  // End BIADemo
}

export interface Token {
  login: string;
  id: number;
  permissions: string[];
  userData: UserData;
}

export class AuthInfo {
  token = '';
  decryptedToken: Token = {
    login: '',
    id: 0,
    permissions: [],
    userData: { currentTeams: [] },
  };
  additionalInfos: AdditionalInfos = {
    userInfo: {},
    teams: [],
    userProfile: { theme: '' },
  };
}
