import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  helpUrl: '',
  reportUrl: '',
  apiUrl: '/api',
  apiUrlDynamic: {
    oldValue: '',
    newValue: '',
  },
  hubUrl: '/HubForClients',
  urlErrorPage: 'https://dmeu.electrical-power.safran/static/error.htm',
  useXhrWithCred: false,
  production: true,
  logging: {
    conf: {
      serverLoggingUrl: '/api/logs',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR,
    },
  },
};
