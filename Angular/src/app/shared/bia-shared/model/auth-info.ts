import { Team } from 'src/app/domains/team/model/team';
import { RoleMode } from '../../constants';

export interface TokenAndTeamsDto {
  token: AuthInfo;
  allTeams: Team[];
}

export class  TeamLoginDto {
  teamTypeId: number;
  teamId: number;
  roleMode: RoleMode;
  roleIds: number[];
  useDefaultRoles: boolean;
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

export interface CurrentTeamDto {
  teamTypeId: number;
  currentTeamId: number;
  currentTeamTitle: string;
  currentRoleIds: number[];
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
