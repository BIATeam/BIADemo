import { EnvironmentType } from 'packages/bia-ng/models/enum/public-api';
import { TeamConfigDto } from './auth-info';

export interface AppSettings {
  keycloak: Keycloak;
  environment: Environment;
  cultures: Culture[];
  monitoringUrl: string;
  profileConfiguration?: ProfileConfiguration;
  iframeConfiguration?: IframeConfiguration;
  teamsConfig: TeamConfigDto[];
}

export interface Environment {
  type: EnvironmentType;
  urlMatomo: string;
  siteIdMatomo: string;
  urlsAdditionalJS: string[];
  urlsAdditionalCSS: string[];
}

export interface Culture {
  label: string;
  code: string;
  acceptedCodes: string[];
  isDefaultForCountryCodes: string[];
  dateFormat: string;
  timeFormat: string;
  timeFormatSec: string;
  languageCode: string;
  languageId: number;
}

export interface Keycloak {
  isActive: boolean;
  baseUrl: string;
  configuration: Configuration;
  api: Api;
}

export interface Configuration {
  idpHint: string;
  realm: string;
  authority: string;
  requireHttpsMetadata: boolean;
  validAudience: string;
}

export interface Api {
  tokenConf: TokenConf;
  searchUserRelativeUrl: string;
}

export interface TokenConf {
  relativeUrl: string;
  clientId: string;
  grantType: string;
}

export interface ProfileConfiguration {
  clientProfileImageGetMode: boolean;
  profileImageUrlOrPath: string;
  editProfileImageUrl: string;
}

export interface IframeConfiguration {
  keepLayout?: boolean;
  allowedIframeHosts?: AllowedHost[];
}

export interface AllowedHost {
  name: string;
  url: string;
}
