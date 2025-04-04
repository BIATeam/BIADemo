import { RoleMode, TeamTypeId } from 'src/app/shared/constants';

export const allEnvironments = {
  appTitle: 'BIADemo',
  companyName: 'TheBIADevCompany',
  enableNotifications: true,
  urlAuth: '/api/Auth',
  urlLog: '/api/logs',
  urlEnv: '/api/Environment',
  urlAppIcon: 'assets/bia/img/AppIcon.svg',
  urlErrorPage: './assets/bia/html/error.html',
  // Except BIADemo version: '0.0.0',
  // Begin BIADemo
  version: '4.0.4',
  // End BIADemo

  teams: [
    {
      teamTypeId: TeamTypeId.Site,
      roleMode: RoleMode.AllRoles,
      inHeader: true,
    },
    // Begin BIADemo
    {
      teamTypeId: TeamTypeId.AircraftMaintenanceCompany,
      roleMode: RoleMode.MultiRoles,
      inHeader: true,
      displayOne: true,
    },
    // BIAToolKit - Begin Partial AllEnvironment MaintenanceTeam
    {
      teamTypeId: TeamTypeId.MaintenanceTeam,
      roleMode: RoleMode.AllRoles,
      inHeader: true,
      displayAlways: true,
      label: 'maintenanceTeam.headerLabel',
    },
    // BIAToolKit - End Partial AllEnvironment MaintenanceTeam
    // End BIADemo
    // BIAToolKit - Begin AllEnvironment
    // BIAToolKit - End AllEnvironment
  ],
};
