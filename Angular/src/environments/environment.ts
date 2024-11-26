import { Capacitor } from '@capacitor/core';
import { NgxLoggerLevel } from 'ngx-logger';

const isAndroid = Capacitor.getPlatform() === 'android';
const serverUrl = isAndroid ? '10.0.2.2' : '127.0.0.1';

export const environment = {
  helpUrl: '',
  reportUrl: '',
  apiUrlDynamic: {
    oldValue: '',
    newValue: '',
  },
  apiUrl: `http://${serverUrl}:32128/BIADemo/WebApi/api`,
  hubUrl: `http://${serverUrl}:32128/BIADemo/WebApi/HubForClients`,
  useXhrWithCred: true,
  production: false,
  logging: {
    conf: {
      serverLoggingUrl: `http://${serverUrl}:32128/BIADemo/WebApi/api/logs`,
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR,
      withCredentials: true,
    },
  },
};
