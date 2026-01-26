import { AllEnvironments } from 'packages/bia-ng/models/public-api';

export const allEnvironments: AllEnvironments & { [key: string]: any } = {
  appTitle: 'BIADemo',
  companyName: 'TheBIADevCompany',
  enableNotifications: true,
  enableWorkerService: true,
  enableAnnouncements: true,
  // Except BIADemo enableOfflineMode: false,
  // Begin BIADemo
  enableOfflineMode: true,
  // End BIADemo
  urlAuth: '/api/Auth',
  urlLog: '/api/logs',
  urlEnv: '/api/AppSettings/Environment',
  urlAppSettings: 'api/AppSettings',
  urlAppIcon: 'assets/bia/img/AppIcon.svg',
  urlErrorPage: './assets/bia/html/error.html',
  // Except BIADemo version: '0.0.0',
  // Begin BIADemo
  version: '6.0.1',
  // End BIADemo
};
