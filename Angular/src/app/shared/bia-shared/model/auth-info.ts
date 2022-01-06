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
  currentSiteId: number;
  currentSiteTitle: string;
  defaultSiteId: number;
  sites: OptionDto[];
  currentRoleIds: number[];
  defaultRoleId: number;
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
