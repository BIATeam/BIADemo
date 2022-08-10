export enum EnvironmentType {
  INT = 'INT',
  UAT = 'UAT',
  PRA = 'PRA',
  PRD = 'PRD'
}

export interface AppSettings {
  keycloak: Keycloak;
  environment: Environment;
  cultures: Culture[];
  monitoringUrl: string;
}


export interface Environment {
  type: EnvironmentType;
  urlMatomo: string;
  siteIdMatomo: string
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