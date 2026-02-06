import { Team } from './team';

export class LoginParamDto {
  currentTeamLogins: CurrentTeamDto[];
  lightToken: boolean;
  fineGrainedPermission: boolean;
  additionalInfos: boolean;
  isFirstLogin: boolean;
  baseUserIdentity?: string;
}

export interface UserProfile {
  theme: string;
}

export interface UserData {
  lastName?: string;
  firstName?: string;
  currentTeams: CurrentTeamDto[];
  crossTeamPermissions?: PermissionTeams[];
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

export class PermissionTeams {
  permission: string;
  teamIds: number[];
  isGlobal: boolean;
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
  permissions: number[];
  userData: UserData;
  expiredAt: number;
}

export class AuthInfo {
  token = '';
  decryptedToken: Token = {
    identityKey: '',
    id: 0,
    permissions: [],
    userData: { currentTeams: [] },
    expiredAt: 0,
  };
  additionalInfos: AdditionalInfos = {
    teams: [],
    userProfile: { theme: '' },
  };
}
