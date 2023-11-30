import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  helpUrl: '',
  reportUrl: '',
  apiUrlDynamic: {
    oldValue: '',
    newValue: ''
  },
  apiUrl: 'http://localhost/BIADemo/WebApi/api',
  hubUrl: 'http://localhost/BIADemo/WebApi/HubForClients',
  useXhrWithCred: true,
  production: false,
  logging: {
    conf: {
      serverLoggingUrl: 'http://localhost/BIADemo/WebApi/api/logs',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR
    }
  }
};
