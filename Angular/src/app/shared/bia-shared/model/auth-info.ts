import { OptionDto } from './option-dto';
import { RoleDto } from './role';

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

export interface CurrentTeamDto {
  teamTypeId: number;
  currentTeamId: number;
  currentTeamTitle: string;
  defaultTeamId: number;
  sites: OptionDto[];
  currentRoleIds: number[];
  defaultRoleIds: number[];
  roles: RoleDto[];
}

export interface AdditionalInfos {
  userInfo: UserInfo;
  userProfile: UserProfile;
  userData: UserData;
}

export interface AuthInfo {
  token: string;
  permissions: string[];
  additionalInfos: AdditionalInfos;
}
