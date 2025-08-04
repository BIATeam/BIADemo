import { RoleMode } from 'packages/bia-ng/models/enum/public-api';
import { Team } from './team';

export class LoginParamDto {
  currentTeamLogins: CurrentTeamDto[];
  teamsConfig: TeamConfigDto[];
  lightToken: boolean;
  fineGrainedPermission: boolean;
  additionalInfos: boolean;
  isFirstLogin: boolean;
  baseUserIdentity?: string;
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

export interface UserProfile {
  theme: string;
}

export interface UserData {
  lastName?: string;
  firstName?: string;
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
  userProfile: UserProfile;
  teams: Team[];
  // Begin BIADemo
  customInfo?: string | undefined;
  // End BIADemo
}

export interface Token {
  identityKey: string;
  id: number;
  permissions: string[];
  userData: UserData;
}

export class AuthInfo {
  token = '';
  decryptedToken: Token = {
    identityKey: '',
    id: 0,
    permissions: [],
    userData: { currentTeams: [] },
  };
  additionalInfos: AdditionalInfos = {
    teams: [],
    userProfile: { theme: '' },
  };
}
