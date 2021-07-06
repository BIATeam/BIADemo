export enum EnvironmentType {
  INT = 'INT',
  UAT = 'UAT',
  PRA = 'PRA',
  PRD = 'PRD'
}

export interface EnvironmentConfiguration {
  type: EnvironmentType;
  urlMatomo: string;
}
