import { NgxLoggerLevel } from 'ngx-logger';

export interface AppEnvironment {
  helpUrl: string;
  reportUrl: string;
  apiUrlDynamic?: {
    oldValue: string;
    newValue: string;
  };
  apiUrl: string;
  hubUrl: string;
  useXhrWithCred: boolean;
  production: boolean;
  logging: {
    conf: {
      serverLoggingUrl: string;
      level: NgxLoggerLevel;
      serverLogLevel: NgxLoggerLevel;
      withCredentials: boolean;
    };
  };
}
