import { AllEnvironments } from 'packages/bia-ng/models/public-api';

export const allEnvironments: AllEnvironments & { [key: string]: any } = {
  appTitle: 'BIADemo',
  companyName: 'TheBIADevCompany',
  enableNotifications: true,
  enableWorkerService: true,
  enableAnnouncements: true,
  urlAuth: '/api/Auth',
  urlLog: '/api/logs',
  urlEnv: '/api/AppSettings/Environment',
  urlAppSettings: 'api/AppSettings',
  urlAppIcon: 'assets/bia/img/AppIcon.svg',
  urlErrorPage: './assets/bia/html/error.html',
  // Except BIADemo version: '0.0.0',
  // Begin BIADemo
  version: '6.0.0-alpha',
  // End BIADemo
};
