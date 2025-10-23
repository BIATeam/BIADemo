import { BiaPermission } from 'packages/bia-ng/core/public-api';
import { BiaNavigation } from 'packages/bia-ng/models/public-api';
import { Permission } from './permission';

export const NAVIGATION: BiaNavigation[] = [
  {
    labelKey: 'app.users',
    permissions: [BiaPermission.User_List_Access],
    path: ['/users'],
    icon: 'pi pi-users',
  },
  {
    labelKey: 'app.sites',
    permissions: [Permission.Site_List_Access],
    path: ['/sites'],
    icon: 'pi pi-home',
  },
  // BIAToolKit - Begin NavigationDomain
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial NavigationDomain Fleet
  {
    labelKey: 'app.fleet',
    icon: 'pi pi-bars',
    children: [
      // BIAToolKit - Begin NavigationDomainFleetChildren
      // BIAToolKit - Begin Partial NavigationDomainFleetChildren AircraftMaintenanceCompany
      {
        labelKey: 'app.aircraftMaintenanceCompanies',
        permissions: [Permission.AircraftMaintenanceCompany_List_Access],
        path: ['/aircraft-maintenance-companies'],
        icon: 'pi pi-sitemap',
      },
      // BIAToolKit - End Partial NavigationDomainFleetChildren AircraftMaintenanceCompany
      // BIAToolKit - Begin Partial NavigationDomainFleetChildren MaintenanceTeam
      {
        labelKey: 'app.maintenanceTeams',
        permissions: [Permission.MaintenanceTeam_List_Access],
        path: ['/maintenance-teams'],
        icon: 'pi pi-sitemap',
      },
      // BIAToolKit - End Partial NavigationDomainFleetChildren MaintenanceTeam
      // BIAToolKit - Begin Partial NavigationDomainFleetChildren Plane
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/planes'],
        icon: 'pi pi-th-large',
      },
      // BIAToolKit - End Partial NavigationDomainFleetChildren Plane
      // BIAToolKit - Begin Partial NavigationDomainFleetChildren Pilot
      {
        labelKey: 'app.pilots',
        permissions: [Permission.Pilot_List_Access],
        path: ['/pilots'],
        icon: 'pi pi-th-large',
      },
      // BIAToolKit - End Partial NavigationDomainFleetChildren Pilot
      // BIAToolKit - Begin Partial NavigationDomainFleetChildren Flight
      {
        labelKey: 'app.flights',
        permissions: [Permission.Flight_List_Access],
        path: ['/flights'],
        icon: 'pi pi-th-large',
      },
      // BIAToolKit - End Partial NavigationDomainFleetChildren Flight
      // BIAToolKit - End NavigationDomainFleetChildren
      // Begin BIADemo
      {
        labelKey: 'app.maintenanceContracts',
        permissions: [Permission.MaintenanceContract_List_Access],
        path: ['/maintenance-contracts'],
        icon: 'pi pi-clipboard',
      },
      {
        labelKey: 'bia.customCode',
        icon: 'pi pi-code',
        children: [
          {
            labelKey: 'app.planesFullCode',
            permissions: [Permission.Plane_List_Access],
            path: ['/planes-full-code'],
          },
          {
            labelKey: 'app.planesSpecific',
            permissions: [Permission.Plane_List_Access],
            path: ['/planes-specific'],
          },
        ],
      },
      // End BIADemo
    ],
  },
  // BIAToolKit - End Partial NavigationDomain Fleet
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End NavigationDomain

  // Begin BIADemo
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
      BiaPermission.Background_Task_Admin,
      BiaPermission.Background_Task_Read_Only,
      // Begin BIADemo
      Permission.Airport_List_Access,
      Permission.PlaneType_List_Access,
      // End BIADemo
    ],
    children: [
      {
        labelKey: 'bia.backgroundTaskAdmin',
        permissions: [BiaPermission.Background_Task_Admin],
        path: ['/backgroundtask/admin'],
      },
      {
        labelKey: 'bia.backgroundTaskReadOnly',
        permissions: [BiaPermission.Background_Task_Read_Only],
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
