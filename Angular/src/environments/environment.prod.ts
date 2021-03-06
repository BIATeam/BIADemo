import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  helpUrl: '',
  reportUrl: '',
  apiUrl: '../WebApi/api',
  hubUrl: '../WebApi/HubForClients',
  urlErrorPage: '/static/error.htm',
  useXhrWithCred: false,
  production: true,
  logging: {
    conf: {
      serverLoggingUrl: '../WebApi/api/logs',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR
    }
  },
  keycloak: {
    conf: {
      realm: 'BIA Realm',
      authServerUrl: 'http://localhost:8080/',
      resource: 'biademo-front'
    },
    login: {
      idpHint: 'darwin'
    }
  }
};
