import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  helpUrl: '',
  reportUrl: '',
  apiUrlDynamic: {
    oldValue: ':4200',
    newValue: ':32128',
  },
  apiUrl: '/api',
  hubUrl: '/HubForClients',
  useXhrWithCred: false,
  production: false,
  logging: {
    conf: {
      serverLoggingUrl: '/api/logs',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR,
      withCredentials: true,
    },
  },
};
