import { Team } from 'src/app/domains/team/model/team';
import { RoleMode } from '../../constants';


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
  teams: Team[];
}

export interface Token {
  login: string;
  id: number;
  permissions: string[];
  userData: UserData;
}

export interface AuthInfo {
  token: string;
  uncryptedToken: Token;
  additionalInfos: AdditionalInfos;
}