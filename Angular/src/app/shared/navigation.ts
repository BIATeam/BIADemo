import { BiaNavigation } from 'packages/bia-ng/models/public-api';
import { Permission } from './permission';

export const NAVIGATION: BiaNavigation[] = [
  {
    labelKey: 'app.users',
    permissions: [Permission.User_List_Access],
    path: ['/users'],
    icon: 'pi pi-users',
  },
  {
    labelKey: 'app.sites',
    permissions: [Permission.Site_List_Access],
    path: ['/sites'],
    icon: 'pi pi-home',
  },
  // BIAToolKit - Begin Navigation
  // BIAToolKit - End Navigation

  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial Navigation AircraftMaintenanceCompany
  {
    labelKey: 'app.aircraftMaintenanceCompanies',
    permissions: [Permission.AircraftMaintenanceCompany_List_Access],
    path: ['/aircraft-maintenance-companies'],
    icon: 'pi pi-sitemap',
  },
  // BIAToolKit - End Partial Navigation AircraftMaintenanceCompany
  // BIAToolKit - Begin Partial Navigation MaintenanceTeam
  {
    labelKey: 'app.maintenanceTeams',
    permissions: [Permission.MaintenanceTeam_List_Access],
    path: ['/maintenance-teams'],
    icon: 'pi pi-sitemap',
  },
  // BIAToolKit - End Partial Navigation MaintenanceTeam
  // BIAToolKit - Begin Partial Navigation Plane
  {
    labelKey: 'app.planes',
    permissions: [Permission.Plane_List_Access],
    path: ['/planes'],
    icon: 'pi pi-th-large',
  },
  // BIAToolKit - End Partial Navigation Plane
  // End BIAToolKit Generation Ignore

  // Begin BIADemo
  {
    labelKey: 'app.planesFullCode',
    permissions: [Permission.Plane_List_Access],
    path: ['/planes-full-code'],
    icon: 'pi pi-th-large',
  },
  {
    labelKey: 'app.planesSpecific',
    permissions: [Permission.Plane_List_Access],
    path: ['/planes-specific'],
    icon: 'pi pi-th-large',
  },
  {
    labelKey: 'app.maintenanceContracts',
    permissions: [Permission.MaintenanceContract_List_Access],
    path: ['/maintenance-contracts'],
    icon: 'pi pi-clipboard',
  },
  {
    labelKey: 'app.hangfire',
    permissions: [Permission.Hangfire_Access],
    path: ['/hangfire'],
    icon: 'pi pi-send',
  },
  // End BIADemo
  {
    labelKey: 'bia.administration',
    icon: 'pi pi-wrench',
    permissions: [
      Permission.Background_Task_Admin,
      Permission.Background_Task_Read_Only,
      // Begin BIADemo
      Permission.Airport_List_Access,
      Permission.PlaneType_List_Access,
      // End BIADemo
    ],
    children: [
      {
        labelKey: 'bia.backgroundTaskAdmin',
        permissions: [Permission.Background_Task_Admin],
        path: ['/backgroundtask/admin'],
      },
      {
        labelKey: 'bia.backgroundTaskReadOnly',
        permissions: [Permission.Background_Task_Read_Only],
        path: ['/backgroundtask/readonly'],
      },
      // Begin BIADemo
      {
        labelKey: 'app.airports',
        permissions: [Permission.Airport_List_Access],
        path: ['airports'],
      },
      {
        labelKey: 'app.planesTypes',
        permissions: [Permission.PlaneType_List_Access],
        path: ['planes-types'],
      },
      // End BIADemo
    ],
  },
];
