import { LoginParamDto } from './auth-info';
import { IframeMessage } from './iframe-message';
import { AppConfig } from './layout/app-config';

export interface IframeConfig extends IframeMessage {
  type: 'CONFIG';
  layoutConfig: AppConfig;
  language: string;
  loginParams: LoginParamDto;
}
