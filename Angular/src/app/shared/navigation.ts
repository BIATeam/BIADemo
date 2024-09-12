import { BiaNavigation } from './bia-shared/model/bia-navigation';
import { Permission } from './permission';

export const NAVIGATION: BiaNavigation[] = [
  {
    labelKey: 'app.users',
    permissions: [Permission.User_List_Access],
    path: ['/users'],
  },
  {
    labelKey: 'app.sites',
    permissions: [Permission.Site_List_Access],
    path: ['/sites'],
  },
  /// BIAToolKit - Begin Navigation
  /// BIAToolKit - End Navigation
  // Begin BIADemo
  {
    labelKey: 'app.examples',
    permissions: [
      Permission.Plane_List_Access,
      Permission.AircraftMaintenanceCompany_List_Access,
      Permission.Hangfire_Access,
    ],
    children: [
      {
        labelKey: 'app.aircraft-maintenance-companies',
        permissions: [Permission.AircraftMaintenanceCompany_List_Access],
        path: ['/examples/aircraft-maintenance-companies'],
      },
      /// BIAToolKit - Begin Partial Navigation MaintenanceTeam
      /// BIAToolKit - Begin Nested NoParent MaintenanceTeam
      {
        labelKey: 'app.maintenance-teams',
        permissions: [Permission.MaintenanceTeam_List_Access],
        /// TODO after creation of CRUD Team MaintenanceTeam : adapt the path
        path: ['/examples/maintenance-teams'],
      },
      /// BIAToolKit - End Nested NoParent MaintenanceTeam
      /// BIAToolKit - End Partial Navigation MaintenanceTeam
      /// BIAToolKit - Begin Partial Navigation Plane
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes'],
      },
      /// BIAToolKit - End Partial Navigation Plane
      {
        labelKey: 'app.planesFullCode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-full-code'],
      },
      {
        labelKey: 'app.planesSpecific',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-specific'],
      },
      {
        labelKey: 'app.hangfire',
        permissions: [Permission.Hangfire_Access],
        path: ['/examples/hangfire'],
      },
    ],
  },
  // End BIADemo
  {
    labelKey: 'bia.administration',
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
        path: ['/examples/airports'],
      },
      {
        labelKey: 'app.planesTypes',
        permissions: [Permission.PlaneType_List_Access],
        path: ['/examples/planes-types'],
      },
      // End BIADemo
    ],
  },
];
