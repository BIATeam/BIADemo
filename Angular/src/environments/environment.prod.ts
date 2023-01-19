import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  helpUrl: '',
  reportUrl: '',
  apiUrlDynamic: ['https://[FQDN]/WebApi/api','biademo','biademo-api'],
  hubUrlDynamic: ['https://[FQDN]/WebApi/HubForClients','biademo','biademo-api'],
  urlErrorPage: '/static/error.htm',
  useXhrWithCred: false,
  production: true,
  logging: {
    conf: {
      serverLoggingUrlDynamic: ['https://[FQDN]/WebApi/api/logs','biademo','biademo-api'],
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR
    }
  }
};
