import { AppConfig } from '../components/layout/services/layout.service';
import { LoginParamDto } from './auth-info';

export interface IframeConfig {
  layoutConfig: AppConfig;
  language: string;
  loginParams: LoginParamDto;
}
