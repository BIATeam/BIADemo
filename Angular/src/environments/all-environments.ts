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
    // BIAToolKit - Begin AllEnvironment
    // Begin BIAToolKit Generation Ignore
    // BIAToolKit - Begin Partial AllEnvironment AircraftMaintenanceCompany
    {
      teamTypeId: TeamTypeId.AircraftMaintenanceCompany,
      label: 'aircraftMaintenanceCompany.headerLabel',
      // Begin BIADemo
      roleMode: RoleMode.MultiRoles,
      inHeader: true,
      displayOne: true,
      // End BIADemo
    },
    // BIAToolKit - End Partial AllEnvironment AircraftMaintenanceCompany
    // BIAToolKit - Begin Partial AllEnvironment MaintenanceTeam
    {
      teamTypeId: TeamTypeId.MaintenanceTeam,
      label: 'maintenanceTeam.headerLabel',
      // Begin BIADemo
      roleMode: RoleMode.AllRoles,
      inHeader: true,
      displayAlways: true,
      displayLabel: true,
      teamSelectionCanBeEmpty: true,
      // End BIADemo
    },
    // BIAToolKit - End Partial AllEnvironment MaintenanceTeam
    // End BIAToolKit Generation Ignore
    // BIAToolKit - End AllEnvironment
  ],
};
