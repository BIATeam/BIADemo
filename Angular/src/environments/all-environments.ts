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
  version: '5.0.0-alpha',
  // End BIADemo

  teams: [
    {
      teamTypeId: TeamTypeId.Site,
      roleMode: RoleMode.AllRoles,
      inHeader: true,
      label: 'site.headerLabel',
    },
    // Begin BIADemo
    {
      teamTypeId: TeamTypeId.AircraftMaintenanceCompany,
      roleMode: RoleMode.MultiRoles,
      inHeader: true,
      displayOne: true,
      label: 'aircraftMaintenanceCompany.headerLabel',
    },
    {
      teamTypeId: TeamTypeId.MaintenanceTeam,
      roleMode: RoleMode.AllRoles,
      inHeader: true,
      displayAlways: true,
      label: 'maintenanceTeam.headerLabel',
      displayLabel: true,
      teamSelectionCanBeEmpty: true,
    },
    // End BIADemo
    // BIAToolKit - Begin AllEnvironment
    // BIAToolKit - End AllEnvironment
  ],
};
