import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  helpUrl: '',
  reportUrl: '',
  apiUrlDynamic: {
    oldValue: ':4200',
    newValue: ':54321'
  },
  apiUrl: '/api',
  hubUrl: '/HubForClients',
  urlErrorPage: 'http://localhost/static/error.htm',
  useXhrWithCred: false,
  production: false,
  logging: {
    conf: {
      serverLoggingUrl: '/api/logs',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR
    }
  }
};
