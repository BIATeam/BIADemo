export enum EnvironmentType {
  INT = 'INT',
  UAT = 'UAT',
  PRA = 'PRA',
  PRD = 'PRD'
}

export interface AppSettings {
  environment: Environment;
  cultures: Culture[];
}


export interface Environment {
  type: EnvironmentType;
  urlMatomo: string;
}

export interface Culture {
  label: string;
  code: string;
  acceptedCodes: string [];
  isDefaultForCountryCodes: string [];
  dateFormat: string;
  timeFormat: string;
  timeFormatSec: string;
  languageCode: string;
  languageId: number;
}
