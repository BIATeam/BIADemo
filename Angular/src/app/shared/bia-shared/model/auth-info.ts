import { OptionDto } from "./option-dto";

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
  defaultSiteId: number;
  sites: OptionDto[];
  currentRoleIds: number[];
  defaultRoleId: number;
  roles: OptionDto[];
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
