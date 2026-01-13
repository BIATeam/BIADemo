import { BiaPermission } from '@bia-team/bia-ng/core';
import { BiaNavigation } from '@bia-team/bia-ng/models';
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
  // BIAToolKit - Begin Partial NavigationDomain Maintenance
  {
    labelKey: 'app.maintenance',
    // Begin BIADemo
    icon: 'pi pi-wrench',
    // End BIADemo
    permissions: [
      // BIAToolKit - Begin NavigationDomainMaintenancePermissions
      // BIAToolKit - Begin Partial NavigationDomainMaintenancePermissions AircraftMaintenanceCompany
      Permission.AircraftMaintenanceCompany_List_Access,
      // BIAToolKit - End Partial NavigationDomainMaintenancePermissions AircraftMaintenanceCompany
      // BIAToolKit - Begin Partial NavigationDomainMaintenancePermissions MaintenanceTeam
      Permission.MaintenanceTeam_List_Access,
      // BIAToolKit - End Partial NavigationDomainMaintenancePermissions MaintenanceTeam
      // BIAToolKit - End NavigationDomainMaintenancePermissions
      // Begin BIADemo
      Permission.MaintenanceContract_List_Access,
      // End BIADemo
    ],
    children: [
      // BIAToolKit - Begin NavigationDomainMaintenanceChildren
      // BIAToolKit - Begin Partial NavigationDomainMaintenanceChildren AircraftMaintenanceCompany
      {
        labelKey: 'app.aircraftMaintenanceCompanies',
        permissions: [Permission.AircraftMaintenanceCompany_List_Access],
        path: ['/maintenance/aircraft-maintenance-companies'],
      },
      // BIAToolKit - End Partial NavigationDomainMaintenanceChildren AircraftMaintenanceCompany
      // BIAToolKit - Begin Partial NavigationDomainMaintenanceChildren MaintenanceTeam
      {
        labelKey: 'app.maintenanceTeams',
        permissions: [Permission.MaintenanceTeam_List_Access],
        path: ['/maintenance/maintenance-teams'],
      },
      // BIAToolKit - End Partial NavigationDomainMaintenanceChildren MaintenanceTeam
      // BIAToolKit - End NavigationDomainMaintenanceChildren
      // Begin BIADemo
      {
        labelKey: 'app.maintenanceContracts',
        permissions: [Permission.MaintenanceContract_List_Access],
        path: ['/maintenance/maintenance-contracts'],
      },
      // End BIADemo
    ],
  },
  // BIAToolKit - End Partial NavigationDomain Maintenance
  // BIAToolKit - Begin Partial NavigationDomain Fleet
  {
    labelKey: 'app.fleet',
    // Begin BIADemo
    icon: 'pi pi-warehouse',
    // End BIADemo
    permissions: [
      // BIAToolKit - Begin NavigationDomainFleetPermissions
      // BIAToolKit - Begin Partial NavigationDomainFleetPermissions Plane
      Permission.Plane_List_Access,
      // BIAToolKit - End Partial NavigationDomainFleetPermissions Plane
      // BIAToolKit - Begin Partial NavigationDomainFleetPermissions Pilot
      Permission.Pilot_List_Access,
      // BIAToolKit - End Partial NavigationDomainFleetPermissions Pilot
      // BIAToolKit - Begin Partial NavigationDomainFleetPermissions Flight
      Permission.Flight_List_Access,
      // BIAToolKit - End Partial NavigationDomainFleetPermissions Flight
      // BIAToolKit - End NavigationDomainFleetPermissions
      // Begin BIADemo
      Permission.Airport_List_Access,
      Permission.PlaneType_List_Access,
      // End BIADemo
    ],
    children: [
      // BIAToolKit - Begin NavigationDomainFleetChildren
      // BIAToolKit - Begin Partial NavigationDomainFleetChildren Plane
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/fleet/planes'],
      },
      // BIAToolKit - End Partial NavigationDomainFleetChildren Plane
      // BIAToolKit - End NavigationDomainFleetChildren
      // Begin BIADemo
      {
        labelKey: 'app.airports',
        permissions: [Permission.Airport_List_Access],
        path: ['/fleet/airports'],
      },
      {
        labelKey: 'app.planesTypes',
        permissions: [Permission.PlaneType_List_Access],
        path: ['/fleet/planes-types'],
      },
      {
        labelKey: 'bia.customCode',
        icon: 'pi pi-code',
        permissions: [Permission.Plane_List_Access],
        children: [
          {
            labelKey: 'app.planesFullCode',
            permissions: [Permission.Plane_List_Access],
            path: ['/fleet/planes-full-code'],
          },
          {
            labelKey: 'app.planesSpecific',
            permissions: [Permission.Plane_List_Access],
            path: ['/fleet/planes-specific'],
          },
        ],
      },
      // End BIADemo
    ],
  },
  // BIAToolKit - End Partial NavigationDomain Fleet
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End NavigationDomain

  // BIAToolKit - Begin Navigation
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial Navigation Pilot
  {
    labelKey: 'app.pilots',
    permissions: [Permission.Pilot_List_Access],
    path: ['/pilots'],
    // Begin BIADemo
    icon: 'pi pi-address-book',
    // End BIADemo
  },
  // BIAToolKit - End Partial Navigation Pilot
  // BIAToolKit - Begin Partial Navigation Flight
  {
    labelKey: 'app.flights',
    permissions: [Permission.Flight_List_Access],
    path: ['/flights'],
    // Begin BIADemo
    icon: 'pi pi-globe',
    // End BIADemo
  },
  // BIAToolKit - End Partial Navigation Flight
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End Navigation

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
    icon: 'pi pi-cog',
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
      {
        labelKey: 'bia.announcements',
        permissions: [BiaPermission.Announcement_List_Access],
        path: ['/announcements'],
      },
      // Begin BIADemo

      // End BIADemo
    ],
  },
];
