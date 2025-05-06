import { AppConfig } from '../components/layout/services/layout.service';
import { LoginParamDto } from './auth-info';
import { IframeMessage } from './iframe-message';

export interface IframeConfig extends IframeMessage {
  type: 'CONFIG';
  layoutConfig: AppConfig;
  language: string;
  loginParams: LoginParamDto;
}
